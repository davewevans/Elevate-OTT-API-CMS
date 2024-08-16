namespace OttApiPlatform.Application.Common.Helpers.Validators;

///<summary>
///Validator class for multi-tenant roles.
///</summary>
public class MultiTenantRoleValidator : RoleValidator<ApplicationRole>
{
    #region Private Fields

    private readonly ITenantResolver _tenantResolver;

    #endregion Private Fields

    #region Public Constructors

    ///<summary>
    ///Constructor for MultiTenantRoleValidator class.
    ///</summary>
    ///<param name="tenantResolver">The tenant resolver instance</param>
    public MultiTenantRoleValidator(ITenantResolver tenantResolver)
    {
        _tenantResolver = tenantResolver;
    }

    #endregion Public Constructors

    #region Public Methods

    ///<summary>
    ///Validates a given role against the given manager.
    ///</summary>
    ///<param name="manager">The RoleManager instance</param>
    ///<param name="role">The role to be validated</param>
    ///<returns>Returns an IdentityResult based on the validation result</returns>
    public override async Task<IdentityResult> ValidateAsync(RoleManager<ApplicationRole> manager, ApplicationRole role)
    {
        var roleInterfaces = typeof(ApplicationRole).GetInterfaces();

        //Throw an exception if the role is not eligible for multitenancy.
        ThrowExceptionIfNotEligibleForMultitenancy(_tenantResolver, roleInterfaces);

        //Attempt to set the tenant ID of the given role.
        TrySetTenantId(role, roleInterfaces, _tenantResolver);

        //Determine if the operation is an "add" operation.
        var isAddOperation = await manager.FindByIdAsync(role.Id) == null;

        //Check if a combination of the given role and tenant ID already exists.
        var combinationExists = await CheckCombinationExists(manager, role, isAddOperation, _tenantResolver);

        //Return an IdentityResult based on whether or not the combination exists.
        return combinationExists
            ? IdentityResult.Failed(new IdentityError
            {
                Code = "DuplicateRoleName",
                Description = _tenantResolver.TenantMode == TenantMode.MultiTenant && !_tenantResolver.IsHost
                                            ? Resource.The_specified_role_is_already_registered_in_the_given_tenant
                                            : Resource.The_specified_role_is_already_registered
            })
            : IdentityResult.Success;
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Tries to set the TenantId property of the given role based on the role's interfaces and the.
    /// tenant resolver.
    /// </summary>
    /// <param name="role">The role to set the TenantId for.</param>
    /// <param name="roleInterfaces">The interfaces implemented by the role.</param>
    /// <param name="tenantResolver">The tenant resolver to get the tenant ID from.</param>
    private static void TrySetTenantId(ApplicationRole role, Type[] roleInterfaces, ITenantResolver tenantResolver)
    {
        // Check if the role implements IMustHaveTenant.
        if (roleInterfaces.Any(i => !i.IsGenericType && i.Name == nameof(IMustHaveTenant)))
        {
            // Get the tenant ID from the tenant resolver.
            var tenantId = tenantResolver.GetTenantId() ?? throw new ArgumentNullException(nameof(role));

            // Set the TenantId property of the role.
            var propertyInfo = role.GetType().GetProperty("TenantId");
            if (propertyInfo != null)
                propertyInfo.SetValue(role, Convert.ChangeType(tenantId, propertyInfo.PropertyType), null);
        }
        // Check if the role implements IMayHaveTenant.
        else if (roleInterfaces.Any(i => !i.IsGenericType && i.Name == nameof(IMayHaveTenant)))
        {
            // Get the tenant ID from the tenant resolver.
            var tenantId = tenantResolver.GetTenantId();

            // Set the TenantId property of the role if it is null.
            var propertyInfo = role.GetType().GetProperty("TenantId");
            if (propertyInfo != null && propertyInfo.GetValue(role) == null)
            {
                var type = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
                var safeValue = tenantId == null ? null : Convert.ChangeType(tenantId, type);
                propertyInfo.SetValue(role, safeValue, null);
            }
        }
    }

    /// <summary>
    /// Throws an ArgumentException if the given role is not eligible for multitenancy based on the.
    /// tenant resolver and the role's interfaces.
    /// </summary>
    /// <param name="tenantResolver">The tenant resolver to check against.</param>
    /// <param name="roleInterfaces">The interfaces implemented by the role.</param>
    private static void ThrowExceptionIfNotEligibleForMultitenancy(ITenantResolver tenantResolver, Type[] roleInterfaces)
    {
        switch (tenantResolver.TenantMode)
        {
            // Check if the tenant mode is MultiTenant.
            case TenantMode.MultiTenant:
                {
                    // Check if the role implements either IMustHaveTenant or IMayHaveTenant.
                    var roleEntityImplementsIHaveTenant = roleInterfaces.Any(i => i.Name is nameof(IMustHaveTenant) or nameof(IMayHaveTenant));

                    if (!roleEntityImplementsIHaveTenant)
                        throw new ArgumentException("ApplicationRole must implement either IMustHaveTenant or IMayHaveTenant.");
                    break;
                }
        }
    }

    /// <summary>
    /// Checks if a combination of role name and tenant exists in the RoleManager.
    /// </summary>
    /// <param name="roleManager">The RoleManager instance to use for the check.</param>
    /// <param name="role">The ApplicationRole instance to check for.</param>
    /// <param name="isAddOperation">
    /// A boolean value indicating whether the check is for an add operation or not.
    /// </param>
    /// <param name="tenantResolver">The ITenantResolver instance to use for the check.</param>
    /// <returns>A boolean value indicating whether the combination exists or not.</returns>
    private static async Task<bool> CheckCombinationExists(RoleManager<ApplicationRole> roleManager,
                                                           ApplicationRole role,
                                                           bool isAddOperation,
                                                           ITenantResolver tenantResolver)
    {
        bool combinationExists;

        if (isAddOperation)
            // For add operation, check if any existing role has the same name and tenant ID as the
            // given role.
            combinationExists = tenantResolver.TenantMode switch
            {
                TenantMode.MultiTenant => await roleManager.Roles.AnyAsync(r => r.NormalizedName == role.Name.ToUpper()
                    && EF.Property<ApplicationRole>(r, "TenantId") == role.GetType().GetProperty("TenantId").GetValue(role)),

                TenantMode.SingleTenant => await roleManager.Roles.AnyAsync(r => r.NormalizedName == role.Name.ToUpper()),

                // If the tenant mode is not specified, throw an ArgumentOutOfRangeException.
                _ => throw new ArgumentOutOfRangeException(Resource.Please_specify_the_application_tenant_mode)
            };
        else
            // For update operation, check if any existing role (other than the given role) has the
            // same name and tenant ID as the given role.
            combinationExists = tenantResolver.TenantMode switch
            {
                TenantMode.MultiTenant => await roleManager.Roles.Where(r => r.Id != role.Id)
                                                                 .AnyAsync(r => r.NormalizedName == role.Name.ToUpper()
                                                                     && EF.Property<ApplicationRole>(r, "TenantId") == role.GetType().GetProperty("TenantId").GetValue(role)),

                TenantMode.SingleTenant => await roleManager.Roles.Where(r => r.Id != role.Id)
                                                                  .AnyAsync(r => r.NormalizedName == role.Name.ToUpper()),

                // If the tenant mode is not specified, throw an ArgumentOutOfRangeException.
                _ => throw new ArgumentOutOfRangeException(Resource.Please_specify_the_application_tenant_mode)
            };

        return combinationExists;
    }

    #endregion Private Methods
}