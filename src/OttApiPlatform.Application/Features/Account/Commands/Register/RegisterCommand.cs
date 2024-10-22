using OttApiPlatform.Application.Features.Tenants.Commands.CreateTenant;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using OttApiPlatform.Application.Common.Contracts;

namespace OttApiPlatform.Application.Features.Account.Commands.Register;

public class RegisterCommand : IRequest<Envelope<RegisterResponse>>
{
    #region Public Properties

    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public string ReturnUrl { get; set; }

    #endregion Public Properties

    #region Public Methods

    public ApplicationUser MapToEntity()
    {
        return new()
        {
            UserName = Email,
            Email = Email,
        };
    }

    #endregion Public Methods

    #region Public Classes

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Envelope<RegisterResponse>>
    {
        #region Private Fields

        private readonly ApplicationUserManager _userManager;
        private readonly ApplicationRoleManager _roleManager;
        private readonly ITenantResolver _tenantResolver;
        private readonly IApplicationDbContext _dbContext;
        private readonly IAuthenticationService _authenticationService;
        private readonly IAppSettingsReaderService _appSettingsReaderService;
        private readonly ILicenseService _licenseService;
        private readonly IMediator _mediator;

        #endregion Private Fields

        #region Public Constructors

        public RegisterCommandHandler(ApplicationUserManager userManager,
                                      ApplicationRoleManager roleManager,
                                      ITenantResolver tenantResolver,
                                      IApplicationDbContext dbContext,
                                      IAuthenticationService authenticationService,
                                      IAppSettingsReaderService appSettingsReaderService,
                                      ILicenseService licenseService,
                                      IMediator mediator)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _tenantResolver = tenantResolver;
            _dbContext = dbContext;
            _authenticationService = authenticationService;
            _appSettingsReaderService = appSettingsReaderService;
            _licenseService = licenseService;
            _mediator = mediator;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<RegisterResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            // Create new user entity from the request data.
            var user = request.MapToEntity();

            // Assign default roles to the new user.
            AssignDefaultRolesToUser(user);

            // Set the initial activation status for the new user.
            await SetInitialActivation(user);

            // Attempt to create the new user with the provided password.
            var createUserResult = await _userManager.CreateAsync(user, request.Password);

            // If user creation is not successful, return a server error response.
            if (!createUserResult.Succeeded)
                return Envelope<RegisterResponse>.Result.AddErrors(createUserResult.Errors.ToApplicationResult(),
                                                                   HttpStatusCode.InternalServerError);

            return await CreateTenantAndAssignToUserAsync(request, cancellationToken, user);

            // Attempt to register the new user as a super admin if they are not already registered
            // as one.
            var registerAsSuperAdminEnvelope = await RegisterAsSuperAdminIfNotExist(user);

            // If registration as super admin is not successful, return a server error response.
            if (registerAsSuperAdminEnvelope.IsError)
                return Envelope<RegisterResponse>.Result.AddErrors(createUserResult.Errors.ToApplicationResult(),
                                                                   HttpStatusCode.InternalServerError);

            // Check if email confirmation is required for registration.
            switch (_userManager.Options.SignIn.RequireConfirmedAccount)
            {
                case true:
                    {
                        // If email confirmation is required, send activation email to the user.
                        var callbackUrl = await _userManager.SendActivationEmailAsync(user);

                        // Create a response with the confirmation URL.
                        var registerResponse = new RegisterResponse
                        {
                            RequireConfirmedAccount = true,
                            DisplayConfirmAccountLink = false,
                            Email = user.Email,
                            EmailConfirmationUrl = HttpUtility.UrlEncode(callbackUrl),
                            AuthResponse = null,
                            SuccessMessage = Resource.Verification_email_has_been_sent
                        };

                        return Envelope<RegisterResponse>.Result.Ok(registerResponse);
                    }
                default:
                    {
                        // If email confirmation is not required, log in the user and return a
                        // response with auth tokens.
                        var loginCommand = new LoginCommand
                        {
                            Email = request.Email,
                            Password = request.Password,
                        };

                        // call the Login method passing in the loginCommand and await for the response.
                        var loginResponse = await _authenticationService.Login(loginCommand);

                        // if the response from Login method has an error.
                        if (loginResponse.IsError)
                            return loginResponse.ValidationErrors.Any()
                                ? Envelope<RegisterResponse>.Result.AddErrors(loginResponse.ValidationErrors,
                                                                              HttpStatusCode.InternalServerError,
                                                                              rollbackDisabled: true)
                                : Envelope<RegisterResponse>.Result.ServerError(loginResponse.Title,
                                                                                rollbackDisabled:
                                                                                true); // return an error message based on the presence of ModelStateErrors in the loginResponse.

                        var registerResponse = new RegisterResponse
                        // create a new instance of RegisterResponse and set its properties.
                        {
                            RequireConfirmedAccount = false,
                            DisplayConfirmAccountLink = false,
                            Email = user.Email,
                            EmailConfirmationUrl = null,
                            AuthResponse = new AuthResponse
                            {
                                AccessToken = loginResponse.Payload.AuthResponse
                                                                          .AccessToken,
                                RefreshToken = loginResponse.Payload.AuthResponse
                                                                          .RefreshToken
                            },
                            SuccessMessage = Resource.You_have_successfully_created_a_new_account
                        };

                        // return an OK response with the newly created RegisterResponse instance.
                        return Envelope<RegisterResponse>.Result.Ok(registerResponse);
                    }
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// If the application is in multi-tenant mode, generate a unique tenant name by removing all non-alphanumeric characters
        /// from the email address and appending a timestamp postfix. Then, create a new tenant using this name. If the tenant
        /// creation fails, return an internal server error response. Otherwise, set the user's TenantId to the newly created tenant's ID.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task<Envelope<RegisterResponse>> CreateTenantAndAssignToUserAsync(RegisterCommand request, CancellationToken cancellationToken,
            ApplicationUser user)
        {
            if (_tenantResolver.TenantMode != TenantMode.MultiTenant) return Envelope<RegisterResponse>.Result.Ok();
            var tenantId = Guid.NewGuid();
            var postfix = $"{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}{DateTime.Now.Hour}{DateTime.Now.Minute}{DateTime.Now.Second}{DateTime.Now.Millisecond}";
            var cleanedEmail = EmailHelper.RemoveNonAlphanumericCharacters(request.Email); var tenantName = $"{cleanedEmail}_{postfix}";
            
            var createTenantCommand = new CreateTenantCommand
            {
                Id = tenantId,
                Name = tenantName,
                LicenseKey = _licenseService.GenerateLicenseForTenant(tenantId),
                StorageFileNamePrefix = tenantId.ToString().Replace("-", "")
            };

            var createTenantResponse = await _mediator.Send(createTenantCommand, cancellationToken);

            if (createTenantResponse.IsError)
                return Envelope<RegisterResponse>.Result.AddErrors(createTenantResponse.ValidationErrors,
                    HttpStatusCode.InternalServerError);
            
            user.TenantId = _tenantResolver.GetTenantId();
            await _userManager.UpdateAsync(user);

            return Envelope<RegisterResponse>.Result.Ok();
        }

        private void AssignDefaultRolesToUser(ApplicationUser user)
        {
            // Get the IDs of all roles that are marked as default.
            var defaultRoleIds = _roleManager.Roles.Where(r => r.IsDefault).Select(r => r.Id);

            // For each default role ID, create a new ApplicationUserRole object and add it to the
            // user's UserRoles collection.
            foreach (var defaultRoleId in defaultRoleIds)
                user.UserRoles.Add(new ApplicationUserRole { RoleId = defaultRoleId });
        }

        private async Task SetInitialActivation(ApplicationUser user)
        {
            // Get the identity settings from the app settings use case.
            var identitySettings = await _appSettingsReaderService.GetIdentitySettings();

            // Set the user's IsSuspended property based on whether new users are active by default
            // or not.
            user.IsSuspended = !identitySettings.Payload.UserSettingsForEdit.NewUsersActiveByDefault;
        }

        private async Task<Envelope<ApplicationUser>> RegisterAsSuperAdminIfNotExist(ApplicationUser user)
        {
            // Check if a super admin already exists in the system.
            var isSuperAdminExist = await _userManager.Users.CountAsync(u => u.IsSuperAdmin) > 0;

            // If a super admin already exists, return success envelope.
            if (isSuperAdminExist)
                return Envelope<ApplicationUser>.Result.Ok();

            // Create a new admin role.
            var adminRole = new ApplicationRole
            {
                Name = "Admin",
                IsStatic = true
            };

            // Check if the admin role exists in the system.
            var isAdminRoleExists = await _roleManager.RoleExistsAsync(adminRole.Name);

            // If the admin role does not exist, create it.
            if (!isAdminRoleExists)
            {
                // Grant permissions for the admin role.
                await GrantPermissionsForAdminRole(adminRole);

                // Create the admin role.
                var roleResult = await _roleManager.CreateAsync(adminRole);

                // If the creation of the admin role failed, return an error envelope.
                if (!roleResult.Succeeded)
                    return Envelope<ApplicationUser>.Result.AddErrors(roleResult.Errors.ToApplicationResult(),
                                                                      HttpStatusCode.BadRequest);
            }

            // Get the number of users in the admin role.
            var adminUserCount = await _userManager.GetUsersInRoleCountAsync(adminRole.Name);

            // If there are users in the admin role, return success envelope.
            if (adminUserCount > 0)
                return Envelope<ApplicationUser>.Result.Ok();

            // Set the user properties to make them a super admin.
            user.IsStatic = true;
            user.IsSuperAdmin = true;
            user.JobTitle = "Administrator";

            // Add the user to the admin role.
            var identityResult = await _userManager.AddToRoleAsync(user, adminRole.Name);

            // If adding the user to the admin role failed, return an error envelope.
            if (!identityResult.Succeeded)
                return Envelope<ApplicationUser>.Result.AddErrors(identityResult.Errors.ToApplicationResult(),
                                                                  HttpStatusCode.BadRequest);

            // Return success envelope with the user object.
            return Envelope<ApplicationUser>.Result.Ok(user);
        }

        private async Task GrantPermissionsForAdminRole(ApplicationRole hostAdminRole)
        {
            // get admin permissions based on the current tenant's visibility.
            var adminPermissions = await _dbContext.ApplicationPermissions
                                                   .Where(p => _tenantResolver.IsHost
                                                              ? p.HostVisibility
                                                              : p.TenantVisibility)
                                                   .ToListAsync();

            // add a role claim for each permission to the hostAdminRole.
            foreach (var permission in adminPermissions)
                hostAdminRole.RoleClaims.Add(new ApplicationRoleClaim
                {
                    ClaimType = "permissions",
                    ClaimValue = permission.Name
                });
        }

        #endregion Private Methods
    }

    #endregion Public Classes
}