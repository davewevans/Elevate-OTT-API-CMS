namespace OttApiPlatform.Application.Common.Helpers.Validators;

/// <summary>
/// User validator for multi-tenant application.
/// </summary>
public class MultiTenantUserValidator : IUserValidator<ApplicationUser>
{
    #region Private Fields

    /// <summary>
    /// Instance of the tenant resolver.
    /// </summary>
    private readonly ITenantResolver _tenantResolver;

    #endregion Private Fields

    #region Public Constructors

    /// <summary>
    /// Initializes a new instance of the MultiTenantUserValidator class.
    /// </summary>
    /// <param name="tenantResolver">Instance of the tenant resolver.</param>
    public MultiTenantUserValidator(ITenantResolver tenantResolver)
    {
        _tenantResolver = tenantResolver;
    }

    #endregion Public Constructors

    #region Public Methods

    /// <summary>
    /// Validates the user asynchronously.
    /// </summary>
    /// <param name="userManager">Instance of the user manager.</param>
    /// <param name="user">Instance of the user being validated.</param>
    /// <returns>The result of the validation.</returns>
    public async Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> userManager, ApplicationUser user)
    {
        // Get all interfaces implemented by ApplicationUser.
        var userInterfaces = typeof(ApplicationUser).GetInterfaces();

        // Check if ApplicationUser is eligible for multitenancy.
        ThrowExceptionIfNotEligibleForMultitenancy(_tenantResolver, userInterfaces);

        // Set tenant ID on the user object.
        TrySetTenantId(user, userInterfaces, _tenantResolver);

        // Check if the user operation is an add operation.
        var isAddOperation = await userManager.FindByIdAsync(user.Id) == null;

        // Check if the combination of user name and tenant ID already exists in the database.
        var combinationExists = await CheckCombinationExists(userManager, user, isAddOperation, _tenantResolver);

        // Return success or failure based on whether the combination already exists.
        return combinationExists
            ? IdentityResult.Failed(new IdentityError
            {
                Code = "DuplicateUserName",
                Description = _tenantResolver.TenantMode == TenantMode.MultiTenant && !_tenantResolver.IsHost
                                            ? Resource.The_specified_username_and_email_are_already_registered_in_the_given_tenant
                                            : Resource.The_specified_username_and_email_are_already_registered
            })
            : IdentityResult.Success;
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Tries to set the TenantId property of the ApplicationUser based on the interfaces.
    /// implemented by the user.
    /// </summary>
    /// <param name="user">The ApplicationUser to set the TenantId property for.</param>
    /// <param name="userInterfaces">The interfaces implemented by the ApplicationUser.</param>
    /// <param name="tenantResolver">The ITenantResolver instance to get the tenant ID from.</param>
    private static void TrySetTenantId(ApplicationUser user, Type[] userInterfaces, ITenantResolver tenantResolver)
    {
        if (userInterfaces.Any(i => !i.IsGenericType && i.Name == nameof(IMustHaveTenant)))
        {
            // The ApplicationUser implements IMustHaveTenant, so the TenantId must be set.
            var tenantId = tenantResolver.GetTenantId() ?? throw new ArgumentNullException(nameof(user));
            var propertyInfo = user.GetType().GetProperty("TenantId");
            if (propertyInfo != null)
                propertyInfo.SetValue(user, Convert.ChangeType(tenantId, propertyInfo.PropertyType), null);
        }
        else if (userInterfaces.Any(i => !i.IsGenericType && i.Name == nameof(IMayHaveTenant)))
        {
            // The ApplicationUser implements IMayHaveTenant, so the TenantId may be null.
            var tenantId = tenantResolver.GetTenantId();
            var propertyInfo = user.GetType().GetProperty("TenantId");
            if (propertyInfo != null && propertyInfo.GetValue(user) == null)
            {
                // The TenantId property is currently null, so it may be set to the retrieved tenant ID.
                var type = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
                var safeValue = tenantId == null ? null : Convert.ChangeType(tenantId, type);
                propertyInfo.SetValue(user, safeValue, null);
            }
        }
    }

    /// <summary>
    /// Throws an exception if the user entity is not eligible for multitenancy.
    /// </summary>
    /// <param name="tenantResolver">The tenant resolver.</param>
    /// <param name="userInterfaces">The user entity interfaces.</param>
    private static void ThrowExceptionIfNotEligibleForMultitenancy(ITenantResolver tenantResolver, Type[] userInterfaces)
    {
        switch (tenantResolver.TenantMode)
        {
            case TenantMode.MultiTenant:
                {
                    // Check if the user entity implements either IMustHaveTenant or IMayHaveTenant.
                    var userEntityImplementsIHaveTenant = userInterfaces.Any(i => i.Name is nameof(IMustHaveTenant) or nameof(IMayHaveTenant));

                    // If the user entity does not implement either IMustHaveTenant or
                    // IMayHaveTenant, throw an ArgumentException.
                    if (!userEntityImplementsIHaveTenant)
                        throw new ArgumentException("ApplicationUser must implement either IMustHaveTenant or IMayHaveTenant.");
                    break;
                }
        }
    }

    /// <summary>
    /// Checks if a user combination exists based on the specified operation and tenant mode.
    /// </summary>
    /// <param name="manager">The <see cref="UserManager{TUser}"/> instance.</param>
    /// <param name="user">The <see cref="ApplicationUser"/> instance.</param>
    /// <param name="isAddOperation">A boolean indicating whether this is an "add" operation.</param>
    /// <param name="tenantResolver">The <see cref="ITenantResolver"/> instance.</param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, containing a boolean.
    /// indicating whether the user combination exists.
    /// </returns>
    private static async Task<bool> CheckCombinationExists(UserManager<ApplicationUser> manager, ApplicationUser user, bool isAddOperation, ITenantResolver tenantResolver)
    {
        bool combinationExists;

        if (isAddOperation)
            // Check if the user combination exists for multi-tenant applications.
            combinationExists = tenantResolver.TenantMode switch
            {
                TenantMode.MultiTenant => await manager.Users.AnyAsync(u => u.NormalizedEmail == user.UserName.ToUpper()
                    && u.NormalizedEmail == user.Email.ToUpper()
                    && EF.Property<ApplicationUser>(u, "TenantId") == user.GetType().GetProperty("TenantId").GetValue(user)),

                // Check if the user combination exists for single-tenant applications.
                TenantMode.SingleTenant => await manager.Users.AnyAsync(u => u.NormalizedEmail == user.UserName.ToUpper()
                    && u.NormalizedEmail == user.Email.ToUpper()),

                // Throw an exception if the tenant mode is not specified.
                _ => throw new ArgumentOutOfRangeException(Resource.Please_specify_the_application_tenant_mode)
            };
        else
            // Check if the user combination exists for multi-tenant applications.
            combinationExists = tenantResolver.TenantMode switch
            {
                TenantMode.MultiTenant => await manager.Users.Where(u => u.Id != user.Id && EF.Property<ApplicationUser>(u, "TenantId") != null)
                                                             .AnyAsync(u => u.NormalizedEmail == user.UserName.ToUpper()
                                                                 && u.NormalizedEmail == user.Email.ToUpper()
                                                                 && EF.Property<ApplicationUser>(u, "TenantId") == user.GetType().GetProperty("TenantId").GetValue(user)),

                // Check if the user combination exists for single-tenant applications.
                TenantMode.SingleTenant => await manager.Users.Where(u => u.Id != user.Id)
                                                              .AnyAsync(u => u.NormalizedEmail == user.UserName.ToUpper() && u.NormalizedEmail == user.Email.ToUpper()),

                // Throw an exception if the tenant mode is not specified.
                _ => throw new ArgumentOutOfRangeException(Resource.Please_specify_the_application_tenant_mode)
            };

        return combinationExists;
    }

    #endregion Private Methods
}