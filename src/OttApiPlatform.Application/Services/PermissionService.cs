namespace OttApiPlatform.Application.Services;

public class PermissionService : IPermissionService
{
    #region Private Fields

    private readonly IApplicationDbContext _dbContext;
    private readonly SignInManager<ApplicationUser> _signInManager;

    #endregion Private Fields

    #region Public Constructors

    public PermissionService(IApplicationDbContext dbContext, SignInManager<ApplicationUser> signInManager)
    {
        _dbContext = dbContext;
        _signInManager = signInManager;
    }

    #endregion Public Constructors

    #region Public Methods

    public async Task<Envelope<PermissionsResponse>> GetPermissionsOnDemand(GetPermissionsQuery request)
    {
        // Get permissions from the database based on the query
        var permissionItems = await _dbContext.ApplicationPermissions
                                              .Include(p => p.Permissions)
                                              .Where(p => request.Id == null ? p.ParentId == null : p.Id == request.Id)
                                              .OrderBy(p => p.Name)
                                              // Load only the necessary fields for the top-level permissions
                                              .Select(p => PermissionItem.MapFromEntityOnDemand(p))
                                              .ToListAsync();

        // Create a PermissionsResponse object containing the requested permissions
        var permissionsResponse = new PermissionsResponse
        {
            Permissions = request.Id == null
                                          // If requesting all top-level permissions, return all the
                                          // retrieved permission items
                                          ? permissionItems
                                          // If requesting a specific permission, return the nested
                                          // permissions of the first permission item retrieved
                                          : permissionItems.FirstOrDefault()
                                                           ?.Permissions?.OrderBy(p => p.Name)
                                                           .ToList(),
        };

        return Envelope<PermissionsResponse>.Result.Ok(permissionsResponse);
    }

    public async Task<Envelope<PermissionsResponse>> GetAllPermissions()
    {
        // Get the maximum parent count by executing the GetPermissionsMaxParentCount method.
        var maxParentCount = await GetPermissionsMaxParentCount();

        // Retrieve all permissions from the database with the necessary associations.
        var permissions = await _dbContext.ApplicationPermissions.Include(p => p.Permissions).ToListAsync();

        // Filter the permissions to retrieve only the root level permissions (where the Parent is null).
        var rootPermissions = permissions.Where(p => p.Parent == null).ToList();

        // Create a list to store the permission items and Iterate over the root permissions and
        // recursively retrieve the nested permissions.
        var permissionItems = rootPermissions.Select(rootPermission => GetPermissionItem(rootPermission, maxParentCount)).ToList();

        // Create a new PermissionsResponse object and assign the retrieved permission items to it.
        var permissionsResponse = new PermissionsResponse
        {
            Permissions = permissionItems,
        };

        // Return the permissions response wrapped in an Envelope with a success result.
        return Envelope<PermissionsResponse>.Result.Ok(permissionsResponse);
    }

    public async Task<List<PermissionItem>> GetUserNonExcludedPermissions(ApplicationUser user)
    {
        // Get the claims principal for the given user using sign-in manager.
        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(user);

        // Get all the user's claims, including inherited ones.
        var userClaims = claimsPrincipal.Claims.ToList();

        // Join the user's claims with the application permissions to get the granted permissions.
        var grantedPermissions = from uc in userClaims
                                 join ap in _dbContext.ApplicationPermissions
                                     on uc.Value equals ap.Name
                                 select new PermissionItem { Id = ap.Id, Name = ap.Name };

        // Get the user's excluded permissions.
        var excludedPermissions = await (from uc in _dbContext.UserClaims
                                         join ap in _dbContext.ApplicationPermissions
                                             on uc.ClaimValue equals ap.Name
                                         where uc.IsExcluded && uc.UserId == user.Id
                                         select new PermissionItem { Id = ap.Id, Name = ap.Name }).ToListAsync();

        // Exclude the user's excluded permissions from the granted permissions.
        var selectedNonExcludedPermissions = grantedPermissions.Except(excludedPermissions, ProjectionEqualityComparer<PermissionItem>.Create(a => a.Id)).ToList();

        // Return the selected non-excluded permissions.
        return selectedNonExcludedPermissions;
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Retrieves the nested permissions recursively.
    /// </summary>
    /// <param name="permission">The current permission to process.</param>
    /// <param name="maxParentCount">The maximum number of parent levels to retrieve.</param>
    /// <param name="currentParentCount">The current parent level count.</param>
    /// <returns>The PermissionItem representing the current permission and its nested permissions.</returns>
    private PermissionItem GetPermissionItem(ApplicationPermission permission, int maxParentCount, int currentParentCount = 0)
    {
        currentParentCount++;

        var permissionItem = new PermissionItem()
        {
            Id = permission.Id,
            Name = permission.Name,
            ParentId = permission.ParentId,
            HasChildren = permission.Permissions.Count != 0,
            IsRoot = permission.Parent == null
        };

        if (currentParentCount < maxParentCount)
            permissionItem.Permissions = permission.Permissions.Select(p => GetPermissionItem(p, maxParentCount, currentParentCount))
                                                   .OrderBy(p => p.Name)
                                                   .ToList();

        return permissionItem;
    }

    /// <summary>
    /// Retrieves the maximum number of parent permissions.
    /// </summary>
    /// <returns>The maximum number of parent permissions.</returns>
    private async Task<int> GetPermissionsMaxParentCount()
    {
        // Count the number of unique parent IDs in the ApplicationPermissions table.
        var maxParentCount = await _dbContext.ApplicationPermissions.Select(p => p.ParentId).Distinct().CountAsync();

        // Return the maximum number of parent permissions.
        return maxParentCount;
    }

    #endregion Private Methods
}