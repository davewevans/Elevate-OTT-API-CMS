namespace OttApiPlatform.Application.Common.Managers;

/// <summary>
/// Manages roles in the application.
/// </summary>
public class ApplicationRoleManager : RoleManager<ApplicationRole>
{
    #region Private Fields

    private readonly IApplicationDbContext _dbContext;

    #endregion Private Fields

    #region Public Constructors

    public ApplicationRoleManager(IRoleStore<ApplicationRole> store,
                                  IEnumerable<IRoleValidator<ApplicationRole>> roleValidators,
                                  ILookupNormalizer keyNormalizer,
                                  IdentityErrorDescriber errors,
                                  ILogger<RoleManager<ApplicationRole>> logger,
                                  IApplicationDbContext dbContext) : base(store,
                                                                          roleValidators,
                                                                          keyNormalizer,
                                                                          errors,
                                                                          logger)
    {
        _dbContext = dbContext;
    }

    #endregion Public Constructors

    #region Public Methods

    /// <summary>
    /// Add or remove a role permission from the specified role.
    /// </summary>
    /// <param name="assignedRolePermissionIds">The assigned role permission IDs</param>
    /// <param name="dbRole">The role to which the permissions should be assigned or removed</param>
    public async Task AddOrRemoveRolePermission(IReadOnlyList<Guid> assignedRolePermissionIds,
                                                ApplicationRole dbRole)
    {
        if (assignedRolePermissionIds != null)
        {
            // Get all the application permissions from the DbContext.
            var applicationPermissionsList = await _dbContext.ApplicationPermissions.ToListAsync();

            // Determine which permissions have been added to the role.
            var addedRolePermissions = assignedRolePermissionIds.Where(arp => dbRole.RoleClaims.All(rc => rc.ClaimValue != applicationPermissionsList.FirstOrDefault(c => c.Id == arp)?.Name)).ToList();

            // Determine which permissions have been removed from the role.
            var removedRolePermissions = dbRole.RoleClaims.Where(rc => assignedRolePermissionIds.All(arp => arp != applicationPermissionsList.FirstOrDefault(c => c.Name == rc.ClaimValue)?.Id)).ToList();

            // Get the names of the added role permissions.
            var selectedPermissions = _dbContext.ApplicationPermissions.Where(p => addedRolePermissions.Contains(p.Id)).Select(p => p.Name);

            // Add the newly assigned permissions to the role.
            foreach (var addedRolePermission in selectedPermissions)
                dbRole.RoleClaims.Add(new ApplicationRoleClaim
                {
                    ClaimType = "permissions",
                    ClaimValue = addedRolePermission
                });

            // Remove the permissions that have been revoked from the role.
            foreach (var removedRolePermission in removedRolePermissions)
                dbRole.RoleClaims.Remove(removedRolePermission);

            // Remove inherited user permissions that are no longer valid.
            var userClaimsToBeRemoved = await RemoveUserInheritedPermissions(removedRolePermissions);

            _dbContext.UserClaims.RemoveRange(userClaimsToBeRemoved);
        }
        else
        {
            // If no permissions are assigned, clear all the role claims.
            dbRole.RoleClaims.Clear();
        }
    }

    /// <summary>
    /// Removes all user claims that belong to the specified role and have 'IsExcluded' property
    /// set. to true.
    /// </summary>
    /// <param name="role">The role for which the excluded user claims need to be removed.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task RemoveExcludedPermissionsFromAllUsers(ApplicationRole role)
    {
        // Get all user claims from the database.
        var allUsersClaims = await _dbContext.UserClaims.ToListAsync();

        // Find the claims that belong to the specified role and have 'IsExcluded' property set to true.
        var removedExcludedUserPermissions = (from c in allUsersClaims
                                              join r in role.RoleClaims on c.ClaimValue equals r.ClaimValue
                                              where c.IsExcluded
                                              select c).ToList();

        // Remove the found claims from the database.
        _dbContext.UserClaims.RemoveRange(removedExcludedUserPermissions);

        // Save changes to the database.
        await _dbContext.SaveChangesAsync();
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Removes user claims that have 'IsExcluded' property set to true and belong to the roles.
    /// whose permissions are being removed.
    /// </summary>
    /// <param name="removedRolePermissions">The list of role claims that are being removed.</param>
    /// <returns>The list of user claims that need to be removed.</returns>
    private async Task<IEnumerable<ApplicationUserClaim>> RemoveUserInheritedPermissions(List<ApplicationRoleClaim> removedRolePermissions)
    {
        // Get all user claims from the database.
        var userClaims = await _dbContext.UserClaims.ToListAsync();

        // Find the user claims that belong to the roles whose permissions are being removed and
        // have 'IsExcluded' property set to true.
        var userClaimsToBeRemoved = userClaims.Where(uc => removedRolePermissions.Any(rc => rc.ClaimValue == uc.ClaimValue && uc.IsExcluded));

        return userClaimsToBeRemoved;
    }

    #endregion Private Methods
}