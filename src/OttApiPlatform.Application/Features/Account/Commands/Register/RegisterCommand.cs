﻿using OttApiPlatform.Application.Features.Tenants.Commands.CreateTenant;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using OttApiPlatform.Application.Common.Contracts;
using OttApiPlatform.Application.Features.AccountInfo.Commands.CreateAccountInfo;

namespace OttApiPlatform.Application.Features.Account.Commands.Register;

public class RegisterCommand : IRequest<Envelope<RegisterResponse>>
{
    #region Public Properties
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public string ReturnUrl { get; set; }
    public string ChannelName { get; set; }
    public string SubDomain { get; set; }
    public bool AcceptTerms { get; set; }

    #endregion Public Properties

    #region Public Methods

    public ApplicationUser MapToEntity()
    {
        var (firstName, lastName) = ExtractFirstAndLastName(FullName);

        return new()
        {
            UserName = Email,
            Email = Email,
            Name = firstName,
            Surname = lastName,
            PhoneNumber = PhoneNumber,
        };
    }

    private (string FirstName, string LastName) ExtractFirstAndLastName(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
        {
            return (string.Empty, string.Empty);
        }

        var nameParts = fullName.Trim().Split(' ');

        if (nameParts.Length == 1)
        {
            return (nameParts[0], string.Empty);
        }

        return (nameParts[0], nameParts[^1]);
    }

    #endregion Public Methods

    #region Public Classes

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Envelope<RegisterResponse>>
    {
        #region Private Fields

        private readonly ILogger _logger;
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

        public RegisterCommandHandler(ILogger<RegisterCommandHandler> logger,
            ApplicationUserManager userManager,
                                      ApplicationRoleManager roleManager,
                                      ITenantResolver tenantResolver,
                                      IApplicationDbContext dbContext,
                                      IAuthenticationService authenticationService,
                                      IAppSettingsReaderService appSettingsReaderService,
                                      ILicenseService licenseService,
                                      IMediator mediator)
        {
            _logger = logger;
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

            // Create tenant and assign to user
            var createTenantResult = await CreateTenantAndAssignToUserAsync(request, user, cancellationToken);

            // If tenant creation failed, return a server error response.
            if (!createTenantResult.IsSuccess)
                return Envelope<RegisterResponse>.Result.ServerError(Resource.Tenant_creation_failed);

            // Create account info for the tenant
            var createAccountInfoResult = await CreateAccountInfoForTenantAsync(request, cancellationToken);

            // If account info creation failed, return a server error response.
            if (!createAccountInfoResult.IsSuccess)
                return Envelope<RegisterResponse>.Result.ServerError(Resource.Account_info_creation_failed);

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
                            return loginResponse.ValidationErrors.Count > 0
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
        /// creation fails, log the error and return a result indicating failure. Otherwise, set the user's TenantId to the newly created tenant's ID
        /// and update the user in the database.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task<CreateTenantResult> CreateTenantAndAssignToUserAsync(RegisterCommand request, ApplicationUser user, CancellationToken cancellationToken)
        {
            var result = new CreateTenantResult();

            if (_tenantResolver.TenantMode != TenantMode.MultiTenant)
            {
                result.IsSuccess = true;
                return result;
            }

            var tenantId = Guid.NewGuid();
            var postfix = $"{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}{DateTime.Now.Hour}{DateTime.Now.Minute}{DateTime.Now.Second}{DateTime.Now.Millisecond}";
            var cleanedChannelName = request.ChannelName.RemoveNonAlphanumericCharsAndSpaces().ToLower();
            var tenantName = $"{cleanedChannelName}_{postfix}";

            var createTenantCommand = new CreateTenantCommand
            {
                Id = tenantId,
                Name = tenantName,
            };

            var createTenantResponse = await _mediator.Send(createTenantCommand, cancellationToken);

            if (createTenantResponse.IsError)
            {
                result.IsSuccess = false;
                _logger.LogError("Tenant creation failed: {Error}", string.Join(", ", createTenantResponse.ValidationErrors.Select(kv => $"{kv.Key}: {kv.Value}")));
                return result;
            }

            //_tenantResolver.SetTenantId(tenantId);
            user.TenantId = tenantId;
            await _userManager.UpdateAsync(user);

            result.IsSuccess = true;
            return result;
        }

        /// <summary>
        /// Creates account information for a tenant. If the tenant ID is invalid, returns a result indicating failure.
        /// Otherwise, generates a license key for the tenant, creates the account information, and logs any errors if the creation fails.
        /// </summary>
        /// <param name="request">The registration command containing user and tenant details.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the tenant creation result.</returns>
        private async Task<CreateTenantResult> CreateAccountInfoForTenantAsync(RegisterCommand request, CancellationToken cancellationToken)
        {
            var result = new CreateTenantResult();

            var tenantId = _tenantResolver.GetTenantId();
            if (tenantId == Guid.Empty || tenantId is null)
            {
                result.IsSuccess = false;
                return result;
            }

            var createAccountInfoCommand = new CreateAccountInfoCommand
            {
                ChannelName = request.ChannelName,
                LicenseKey = _licenseService.GenerateLicenseForTenant(tenantId.Value),
                SubDomain = request.SubDomain,
                TenantId = tenantId.Value,
                StorageFileNamePrefix = tenantId.Value.GetStorageFileNamePrefix()
            };

            var createAccountInfoResponse = await _mediator.Send(createAccountInfoCommand, cancellationToken);

            if (createAccountInfoResponse.IsError)
            {
                result.IsSuccess = false;
                _logger.LogError("AccountInfo creation failed: {Error}", string.Join(", ", createAccountInfoResponse.ValidationErrors.Select(kv => $"{kv.Key}: {kv.Value}")));
                return result;
            }

            result.IsSuccess = true;
            return result;
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