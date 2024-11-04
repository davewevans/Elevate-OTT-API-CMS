namespace OttApiPlatform.Application.Common.Managers;

/// <summary>
/// Manages users in the application.
/// </summary>
public class ApplicationUserManager : UserManager<ApplicationUser>
{
    #region Private Fields

    private readonly IApplicationDbContext _dbContext;
    private readonly IConfigReaderService _configReaderService;
    private readonly INotificationService _notificationService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITenantResolver _tenantResolver;

    #endregion Private Fields

    #region Public Constructors

    public ApplicationUserManager(IUserStore<ApplicationUser> store,
                                  IOptions<IdentityOptions> optionsAccessor,
                                  IPasswordHasher<ApplicationUser> passwordHasher,
                                  IEnumerable<IUserValidator<ApplicationUser>> userValidators,
                                  IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators,
                                  ILookupNormalizer keyNormalizer,
                                  IdentityErrorDescriber errors,
                                  IServiceProvider services,
                                  ILogger<UserManager<ApplicationUser>> logger,
                                  IApplicationDbContext dbContext,
                                  IConfigReaderService configReaderService,
                                  INotificationService notificationService,
                                  IHttpContextAccessor httpContextAccessor,
                                  ITenantResolver tenantResolver) : base(store,
                                                                                       optionsAccessor,
                                                                                       passwordHasher,
                                                                                       userValidators,
                                                                                       passwordValidators,
                                                                                       keyNormalizer,
                                                                                       errors,
                                                                                       services,
                                                                                       logger)
    {
        _dbContext = dbContext;
        _configReaderService = configReaderService;
        _notificationService = notificationService;
        _httpContextAccessor = httpContextAccessor;
        _tenantResolver = tenantResolver;
    }

    #endregion Public Constructors

    #region Public Methods

    /// <summary>
    /// Removes permissions from a user that are excluded for a given user role.
    /// </summary>
    /// <param name="user">The user to remove permissions from.</param>
    /// <param name="removedUserRole">The user role for which to remove excluded permissions.</param>
    public void RemoveExcludedUserPermissions(ApplicationUser user, ApplicationUserRole removedUserRole)
    {
        // Find all permissions that need to be removed.
        var removedExcludedUserPermissions = from c in user.Claims
                                             join r in removedUserRole.Role.RoleClaims on c.ClaimValue equals r.ClaimValue
                                             where c.IsExcluded
                                             select c;

        // Remove all found permissions from the user.
        user.Claims.RemoveAll(r => removedExcludedUserPermissions.Any(up => r.Id == up.Id));
    }

    /// <summary>
    /// Adds or removes permissions for a given application user.
    /// </summary>
    /// <param name="selectedPermissionIds">The selected permission IDs to add or remove.</param>
    /// <param name="user">The application user.</param>
    public async Task AddOrRemovePermissionsForUserAsync(IReadOnlyList<Guid> selectedPermissionIds, ApplicationUser user)
    {
        // Gets the inherited permission IDs for the user.
        var inheritedUserPermissionIds = (await GetInheritedPermissionsForUserAsync(user.Id, _dbContext)).Select(p => p.Id).ToList();

        // Gets the direct permission IDs for the user.
        var directUserPermissionsIds = selectedPermissionIds.Except(inheritedUserPermissionIds).ToList();

        // Adds user permissions that are not already in the user's claims.
        if (directUserPermissionsIds.Any())
        {
            var permissionsInDb = await _dbContext.ApplicationPermissions.ToListAsync();

            bool NotInUserClaims(Guid dup) => user.Claims.All(uc => uc.ClaimValue != permissionsInDb.FirstOrDefault(p => p.Id == dup)?.Name);

            var addedUserPermissionIds = directUserPermissionsIds.Where(NotInUserClaims).ToList();

            var addedUserPermissions = from aud in addedUserPermissionIds
                                       join pid in permissionsInDb on aud equals pid.Id
                                       select new ApplicationUserClaim
                                              {
                                           UserId = user.Id,
                                           ClaimType = "permissions",
                                           ClaimValue = pid.Name,
                                           IsExcluded = false,
                                       };

            // Adds the added user permissions to the user claims.
            user.Claims.AddRange(addedUserPermissions);

            bool NotInDirectUserPermissions(ApplicationUserClaim uc) => directUserPermissionsIds.All(dup => dup != permissionsInDb.FirstOrDefault(c => c.Name == uc.ClaimValue)?.Id);

            // Removes user permissions that are not in the direct user permissions.
            var removedUserPermissions = user.Claims.Where(NotInDirectUserPermissions).ToList();

            foreach (var removedUserPermission in removedUserPermissions)
                user.Claims.Remove(removedUserPermission);
        }
        else
        {
            // Clears all user claims.
            user.Claims.Clear();
        }

        // Removes the inherited permissions for the user.
        RemoveInheritedPermissionsForUserAsync(inheritedUserPermissionIds, selectedPermissionIds, user, _dbContext);
    }

    /// <summary>
    /// Generates a random password with the specified length.
    /// </summary>
    /// <param name="passwordLength">The length of the password.</param>
    /// <returns>A randomly generated password.</returns>
    public string GenerateRandomPassword(int passwordLength)
    {
        var rnd = new Random();
        return rnd.Next().ToString();
    }

    /// <summary>
    /// Adds or removes user roles for the specified user.
    /// </summary>
    /// <param name="assignedUserRoleIds">A list of role IDs to be assigned to the user.</param>
    /// <param name="dbUser">The user to add or remove roles for.</param>
    public void AddOrRemoveUserRoles(IReadOnlyList<string> assignedUserRoleIds, ApplicationUser dbUser)
    {
        if (assignedUserRoleIds?.Any() == true)
        {
            // Get the roles that are not already assigned to the user.
            var addedUserRoles = assignedUserRoleIds.Where(aur => dbUser.UserRoles.All(r => r.RoleId != aur)).ToList();

            // Add the new roles to the user.
            foreach (var addedRole in addedUserRoles)
                dbUser.UserRoles.Add(new ApplicationUserRole
                                     {
                                         RoleId = addedRole,
                                         UserId = dbUser.Id,
                                     });

            // Remove any roles that are no longer assigned to the user.
            var removedUserRoles = dbUser.UserRoles.Where(ur => assignedUserRoleIds.All(ar => ar != ur.RoleId)).ToList();

            foreach (var removedUserRole in removedUserRoles)
            {
                dbUser.UserRoles.Remove(removedUserRole);

                RemoveExcludedUserPermissions(dbUser, removedUserRole);
            }
        }
        else
        {
            // Remove all roles from the user.
            foreach (var userRole in dbUser.UserRoles)
                RemoveExcludedUserPermissions(dbUser, userRole);

            dbUser.UserRoles.Clear();
        }
    }

    /// <summary>
    /// Assigns roles to the specified user, adding any default roles that are not already assigned.
    /// </summary>
    /// <param name="assignedRoleIds">A list of role IDs to assign to the user.</param>
    /// <param name="user">The user to assign roles to.</param>
    /// <param name="defaultRoleIds">
    /// A list of default role IDs to assign to the user if not already assigned.
    /// </param>
    public void AssignRolesToUser(IReadOnlyList<string> assignedRoleIds, ApplicationUser user, IReadOnlyList<string> defaultRoleIds)
    {
        if (assignedRoleIds?.Any() == true)
            // Add the assigned roles to the user.
            foreach (var roleId in assignedRoleIds)
                user.UserRoles.Add(new ApplicationUserRole { RoleId = roleId });

        // Add any default roles that are not already assigned to the user.
        foreach (var defaultRoleId in defaultRoleIds)
            if (user.UserRoles.All(ur => ur.RoleId != defaultRoleId))
                user.UserRoles.Add(new ApplicationUserRole { RoleId = defaultRoleId });
    }

    /// <summary>
    /// Sends an activation email to the specified user.
    /// </summary>
    /// <param name="user">The user to send the activation email to.</param>
    /// <returns>The callback URL for the activation email.</returns>
    public async Task<string> SendActivationEmailAsync(ApplicationUser user)
    {
        // Generate email confirmation token for the user.
        var code = await GenerateEmailConfirmationTokenAsync(user);

        // Encode the token with Base64Url.
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        // Get client app options and tenant host name.
        var clientAppOptions = _configReaderService.GetClientAppOptions();
        var tenantHostName = _httpContextAccessor.GetClientAppHostName();

        // Generate callback URL for the activation email.
        var url = $"{tenantHostName}/{clientAppOptions.ConfirmEmailUrl}";
        var callbackUrl = string.Format(url, user.Id, code);

        // Send the activation email to the user.
        await _notificationService.SendEmailAsync(user.Email,
                                                 Resource.Confirm_your_email,
                                                 string.Format(Resource.Please_confirm_your_account_by_clicking_here,
                                                     HtmlEncoder.Default.Encode(callbackUrl)));

        // Return the callback URL for the activation email.
        return callbackUrl;
    }

    /// <summary>
    /// Sends an activation email to the specified user with a new email address.
    /// </summary>
    /// <param name="user">The user to send the activation email to.</param>
    /// <param name="newEmail">The new email address.</param>
    /// <returns>The callback URL for the activation email.</returns>
    public async Task<string> SendActivationEmailAsync(ApplicationUser user, string newEmail)
    {
        var callbackUrl = string.Empty;

        // Check if account confirmation is required.
        if (Options.SignIn.RequireConfirmedAccount)
        {
            // Generate change email token for the user with the new email address.
            var code = await GenerateChangeEmailTokenAsync(user, newEmail);

            // Encode the token with Base64Url.
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            // Get client app options and tenant host name.
            var clientAppOptions = _configReaderService.GetClientAppOptions();
            var tenantHostName = _httpContextAccessor.GetClientAppHostName();

            // Generate callback URL for the activation email.
            var url = $"{tenantHostName}/{clientAppOptions.ConfirmEmailChangeUrl}";
            callbackUrl = string.Format(url, user.Id, newEmail, code);

            // Send the activation email to the new email address.
            await _notificationService.SendEmailAsync(newEmail,
                                                     Resource.Confirm_your_email,
                                                     string.Format(Resource.Please_confirm_your_account_by_clicking_here,
                                                         HtmlEncoder.Default.Encode(callbackUrl)));
        }

        // Return the callback URL for the activation email.
        return callbackUrl;
    }

    /// <summary>
    /// Sends a reset password email to the specified user.
    /// </summary>
    /// <param name="user">The user to send the email to.</param>
    /// <returns>The reset password code.</returns>
    public async Task<string> SendResetPasswordAsync(ApplicationUser user)
    {
        // Generate a password reset token for the user.
        var code = await GeneratePasswordResetTokenAsync(user);

        // Encode the token as a base64 URL string.
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        // Get the client app options from configuration.
        var clientAppOptions = _configReaderService.GetClientAppOptions();

        // Get the tenant mode from the tenant resolver.
        var tenantMode = _tenantResolver.TenantMode;

        // Determine the appropriate URL based on the tenant mode.
        var url = tenantMode switch
        {
            TenantMode.MultiTenant => $"{string.Format(clientAppOptions.MultiTenantHostName, _configReaderService.GetSubDomain())}/{clientAppOptions.ResetPasswordUrl}",
            TenantMode.SingleTenant => $"{clientAppOptions.SingleTenantHostName}/{clientAppOptions.ResetPasswordUrl}",
            _ => throw new ArgumentOutOfRangeException(nameof(tenantMode), tenantMode, null),
        };

        // Format the URL with the reset password code.
        var callbackUrl = string.Format(url, code);

        // Send the reset password email to the user's email address.
        await _notificationService.SendEmailAsync(user.Email,
                                                  Resource.Reset_your_password,
                                                  string.Format(Resource.Please_reset_your_password_by_clicking_here,
                                                      HtmlEncoder.Default.Encode(callbackUrl)));

        // Return the reset password code.
        return code;
    }

    /// <summary>
    /// Gets the number of users in the specified role.
    /// </summary>
    /// <param name="role">The name of the role.</param>
    /// <returns>The number of users in the role.</returns>
    public async Task<int> GetUsersInRoleCountAsync(string role)
    {
        // Query the user roles to get the number of users in the specified role.
        return await _dbContext.UserRoles.Include(ur => ur.Role)
                               .Where(ur => ur.Role.NormalizedName == role.ToUpper())
                               .CountAsync();
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Retrieves a list of permissions inherited by a user from their roles in the application's context.
    /// </summary>
    /// <param name="userId">The ID of the user to retrieve permissions for.</param>
    /// <param name="dbContext">The application's database context.</param>
    /// <returns>A list of ApplicationPermission objects representing the user's inherited permissions.</returns>
    private async Task<List<ApplicationPermission>> GetInheritedPermissionsForUserAsync(string userId, IApplicationDbContext dbContext)
    {
        // Join the relevant tables in the database to retrieve the user's inherited permissions.
        var claims = await dbContext.ApplicationPermissions.FromSqlRaw("SELECT Claims.* FROM AspNetUsers AS Users JOIN AspNetUserRoles AS UserRoles ON Users.Id = UserRoles.UserId JOIN AspNetRoles AS Roles ON UserRoles.RoleId = Roles.Id JOIN AspNetRoleClaims AS RoleClaims ON Roles.Id = RoleClaims.RoleId JOIN AspNetPermissions AS Claims ON RoleClaims.ClaimValue = Claims.Name WHERE Users.Id = {0}", userId).ToListAsync();

        return claims;
    }

    /// <summary>
    /// Removes the inherited permissions from a user's claims that are not in the selected.
    /// permission IDs.
    /// </summary>
    /// <param name="inheritedDbUserPermissions">A list of the user's inherited permission IDs.</param>
    /// <param name="selectedPermissionIds">A list of the permission IDs that should be retained.</param>
    /// <param name="user">
    /// The ApplicationUser object representing the user whose permissions should be updated.
    /// </param>
    /// <param name="dbContext">The database context from which to retrieve the permissions.</param>
    private void RemoveInheritedPermissionsForUserAsync(IReadOnlyList<Guid> inheritedDbUserPermissions,
                                                        IReadOnlyList<Guid> selectedPermissionIds,
                                                        ApplicationUser user,
                                                        IApplicationDbContext dbContext)
    {
        // Determine which permission IDs should be removed from the user's claims.
        var userPermissionIdsToBeRemoved = inheritedDbUserPermissions.Except(selectedPermissionIds).ToList();

        // Retrieve the names of the permissions to be removed.
        var userPermissionsToBeRemoved = (from up in userPermissionIdsToBeRemoved
                                          join ap in dbContext.ApplicationPermissions
                                              on up equals ap.Id
                                          select ap.Name).ToList();

        // Add excluded claims for each of the permissions to be removed.
        user.Claims.AddRange(from removedUserPermission in userPermissionsToBeRemoved
                             select new ApplicationUserClaim
                                    {
                                 ClaimType = "excluded",
                                 ClaimValue = removedUserPermission,
                                 IsExcluded = true,
                             });
    }
}

#endregion Private Methods
