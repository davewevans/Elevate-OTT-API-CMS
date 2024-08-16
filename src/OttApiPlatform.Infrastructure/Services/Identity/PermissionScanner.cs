namespace OttApiPlatform.Infrastructure.Services.Identity;

public class PermissionScanner : IPermissionScanner
{
    #region Private Fields

    private readonly ApplicationPartManager _partManager;
    private readonly IApplicationDbContext _dbContext;

    #endregion Private Fields

    #region Public Constructors

    public PermissionScanner(ApplicationPartManager partManager, IApplicationDbContext dbContext)
    {
        _partManager = partManager;
        _dbContext = dbContext;
    }

    #endregion Public Constructors

    #region Public Methods

    public async Task ScanAndSeedBuiltInPermissions()
    {
        // Get all the application permissions to be deleted and remove them from the database.
        var permissionsToBeDeleted = await _dbContext.ApplicationPermissions.IgnoreQueryFilters().ToListAsync();
        _dbContext.ApplicationPermissions.RemoveRange(permissionsToBeDeleted);

        // Create a new controller feature and populate it.
        var feature = new ControllerFeature();
        _partManager.PopulateFeature(feature);

        // Select all the controllers and their actions, and filter out the compiler generated ones
        // and allow anonymous actions.
        var controllers = feature.Controllers
            .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
            .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
            .Select(info => new
            {
                Controller = info.DeclaringType?.Name.Replace("Controller", string.Empty),
                Action = info.Name,
                Attributes = string.Join(",", info.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", ""))),
                TypeAttributes = string.Join(",", info.DeclaringType?.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", "")) ?? Array.Empty<string>())
            })
            .Where(c => !c.TypeAttributes.Contains("AllowAnonymous") && c.TypeAttributes.Contains("BpAuthorize"))
            .Where(c => !c.Attributes.Contains("AllowAnonymous"))
            .OrderBy(c => c.Controller).ThenBy(c => c.Action).ToList();

        // Select all the tenant controllers and their actions, and filter out the compiler
        // generated ones and allow anonymous actions.
        var tenantControllers = feature.Controllers
            .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
            .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
            .Select(info => new
            {
                Controller = info.DeclaringType?.Name.Replace("Controller", string.Empty),
                Action = info.Name,
                Attributes = string.Join(",", info.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", ""))),
                TypeAttributes = string.Join(",", info.DeclaringType?.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", "")) ?? throw new InvalidOperationException())
            })
            .Where(c => !c.TypeAttributes.Contains("AllowAnonymous") && c.TypeAttributes.Contains("BpTenantAuthorize"))
            .Where(c => !c.Attributes.Contains("AllowAnonymous"))
            .OrderBy(c => c.Controller).ThenBy(c => c.Action).ToList();

        // Select all the host controllers and their actions, and filter out the compiler generated
        // ones and allow anonymous actions.
        var hostControllers = feature.Controllers
            .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
            .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
            .Select(info => new
            {
                Controller = info.DeclaringType?.Name.Replace("Controller", string.Empty),
                Action = info.Name,
                Attributes = string.Join(",", info.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", ""))),
                TypeAttributes = string.Join(",", info.DeclaringType?.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", "")) ?? throw new InvalidOperationException())
            })
            .Where(c => !c.TypeAttributes.Contains("AllowAnonymous") && c.TypeAttributes.Contains("BpHostAuthorize"))
            .Where(c => !c.Attributes.Contains("AllowAnonymous"))
            .OrderBy(c => c.Controller).ThenBy(c => c.Action).ToList();

        // Add a root node to the database.
        var rootNode = new ApplicationPermission { Name = "Pages", TenantVisibility = true, HostVisibility = true };
        await _dbContext.ApplicationPermissions.AddAsync(rootNode);

        // Create a new list to store root permissions for the application.
        var rootPermissions = new List<ApplicationPermission>();

        // Create a new list to store root permissions for the tenant.
        var rootTenantPermissions = new List<ApplicationPermission>();

        // Create a new list to store root permissions for the host.
        var rootHostPermissions = new List<ApplicationPermission>();

        // Iterate through the controllers list and add root permissions for the host and tenant.
        foreach (var item in controllers)
        {
            // Create a string representing the name of the root permission based on the controller name.
            var rootPermissionName = $"{item.Controller}";

            // Check if the root permission has not already been added to the list of root permissions.
            if (rootPermissions.All(p => p.Name != rootPermissionName))
            {
                // Create a new application permission object.
                var rootPermission = new ApplicationPermission
                {
                    Name = rootPermissionName,
                    ParentId = rootNode.Id == Guid.Empty ? (await _dbContext.ApplicationPermissions.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.Name == "Pages"))?.Id : rootNode.Id,
                    TenantVisibility = true,
                    HostVisibility = true
                };

                // Add the root permission to the list of root permissions.
                rootPermissions.Add(rootPermission);
            }
        }

        // Iterate through the tenantControllers list and add root permissions for the tenant.
        foreach (var item in tenantControllers)
        {
            // Create a name for the new root permission by using the controller name.
            var rootPermissionName = $"{item.Controller}";

            // Check if the rootTenantPermissions list already contains a permission with the same name.
            if (rootTenantPermissions.All(p => p.Name != rootPermissionName))
            {
                // If the list doesn't contain a permission with the same name, create a new root
                // permission object.
                var rootPermission = new ApplicationPermission
                {
                    Name = rootPermissionName,
                    // Set the parent ID to the ID of the "Pages" permission, or to the rootNode ID
                    // If it's not empty.
                    ParentId = rootNode.Id == Guid.Empty ? (await _dbContext.ApplicationPermissions.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.Name == "Pages"))?.Id : rootNode.Id,
                    // Set the TenantVisibility to true and the HostVisibility to false.
                    TenantVisibility = true,
                    HostVisibility = false
                };

                // Add the new root permission to the rootTenantPermissions list.
                rootTenantPermissions.Add(rootPermission);
            }
        }

        // Iterate through the host controllers list and add root permissions for the host.
        foreach (var item in hostControllers)
        {
            // Create a name for the new root permission by using the controller name.
            var rootPermissionName = $"{item.Controller}";

            // Check if the root host permission already exists.
            if (rootHostPermissions.All(p => p.Name != rootPermissionName))
            {
                // Create a new root host permission.
                var rootPermission = new ApplicationPermission
                {
                    Name = rootPermissionName,
                    ParentId = rootNode.Id == Guid.Empty ? (await _dbContext.ApplicationPermissions.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.Name == "Pages"))?.Id : rootNode.Id,
                    TenantVisibility = false,
                    HostVisibility = true
                };
                rootHostPermissions.Add(rootPermission);
            }
        }

        // Iterate through all root permissions.
        foreach (var rootPermission in rootPermissions)
        {
            // Set the parent ID for the root permission.
            rootPermission.ParentId = rootNode.Id == Guid.Empty ? _dbContext.ApplicationPermissions.FirstOrDefault(p => p.Name == "Pages")?.Id : rootNode.Id;

            // Add the root permission to the database.
            await _dbContext.ApplicationPermissions.AddAsync(rootPermission);

            // Iterate through each controller associated with the root permission.
            foreach (var type in controllers.Where(ct => ct.Controller == rootPermission.Name))
            {
                // Create a child permission for the controller action.
                var childPermissionName = $"{type.Controller}.{type.Action}";

                // Get the controller name.
                var controllerName = childPermissionName.Split(".")[0];

                // Add the child permission to the database.
                await _dbContext.ApplicationPermissions.AddAsync(new ApplicationPermission
                {
                    Name = childPermissionName,
                    ParentId = rootPermission.Id == Guid.Empty ? (await _dbContext.ApplicationPermissions.FirstOrDefaultAsync(p => p.Name == controllerName))?.Id : rootPermission.Id,
                    TenantVisibility = true,
                    HostVisibility = true
                });
            }
        }

        // Iterate through all root tenant permissions.
        foreach (var rootTenantPermission in rootTenantPermissions)
        {
            // Set the parent ID for the root tenant permission.
            rootTenantPermission.ParentId = rootNode.Id == Guid.Empty ? _dbContext.ApplicationPermissions.FirstOrDefault(p => p.Name == "Pages")?.Id : rootNode.Id;

            // Add the root tenant permission to the database.
            await _dbContext.ApplicationPermissions.AddAsync(rootTenantPermission);

            // Iterate through each tenant controller associated with the root tenant permission.
            foreach (var type in tenantControllers.Where(ct => ct.Controller == rootTenantPermission.Name))
            {
                // Create a child permission for the controller action.
                var childPermissionName = $"{type.Controller}.{type.Action}";

                // Get the controller name.
                var controllerName = childPermissionName.Split(".")[0];

                // Add the child permission to the database.
                await _dbContext.ApplicationPermissions.AddAsync(new ApplicationPermission
                {
                    Name = childPermissionName,
                    ParentId = rootTenantPermission.Id == Guid.Empty ? (await _dbContext.ApplicationPermissions.FirstOrDefaultAsync(p => p.Name == controllerName))?.Id : rootTenantPermission.Id,
                    TenantVisibility = true
                });
            }
        }

        // Iterate through all root host permissions.
        foreach (var rootHostPermission in rootHostPermissions)
        {
            // Set the parent ID of the root host permission.
            rootHostPermission.ParentId = rootNode.Id == Guid.Empty ? _dbContext.ApplicationPermissions.FirstOrDefault(p => p.Name == "Pages")?.Id : rootNode.Id;

            // Add the root host permission to the context.
            await _dbContext.ApplicationPermissions.AddAsync(rootHostPermission);

            // Iterate through each host controller associated with the root host permission.
            foreach (var type in hostControllers.Where(ct => ct.Controller == rootHostPermission.Name))
            {
                // Create the child permission name using the controller and action.
                var childPermissionName = $"{type.Controller}.{type.Action}";

                // Get the controller name from the child permission name.
                var controllerName = childPermissionName.Split(".")[0];

                // Add the child permission to the context.
                await _dbContext.ApplicationPermissions.AddAsync(new ApplicationPermission
                {
                    Name = childPermissionName,
                    ParentId = rootHostPermission.Id == Guid.Empty ? (await _dbContext.ApplicationPermissions.FirstOrDefaultAsync(p => p.Name == controllerName))?.Id : rootHostPermission.Id,
                    TenantVisibility = false,
                    HostVisibility = true
                });
            }
        }

        try
        {
            // Save all changes made to the underlying database.
            await _dbContext.SaveChangesAsync();
        }
        catch
        {
            throw new DbUpdateException();
        }
    }

    #endregion Public Methods
}