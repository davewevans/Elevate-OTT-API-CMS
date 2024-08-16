namespace OttApiPlatform.WebAPI.Filters;

/// <summary>
/// Custom authorization attribute used to check if the current user has permission to access a resource.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class BpAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    #region Public Methods

    /// <summary>
    /// Checks if the current user has permission to access a resource.
    /// </summary>
    /// <param name="context">The context for the authorization filter.</param>
    public virtual void OnAuthorization(AuthorizationFilterContext context)
    {
        // Define the httpContextAccessor.
        var httpContextAccessor = context.HttpContext.RequestServices.GetRequiredService<IHttpContextAccessor>();

        //var dbContext = context.HttpContext.RequestServices.GetRequiredService<IApplicationDbContext>();

        // Get the route data from the context.
        var routeData = (httpContextAccessor.HttpContext ?? throw new InvalidOperationException()).GetRouteData();

        //var areaName = routeData?.Values["area"]?.ToString();

        // Check if the user is a super admin.
        var isSuperAdmin = context.HttpContext.User.HasClaim(c => c.Type == "IsSuperAdmin" && c.Value == "true");

        if (isSuperAdmin)
            return;

        // Get the controller and action names from the route data.
        var controllerName = routeData.Values["controller"]?.ToString();
        var actionName = routeData.Values["action"]?.ToString();

        // Combine the controller and action names to create the permission name.
        var permission = $"{controllerName}.{actionName}";

        // TODO: Uncomment this section if you want to check whether the anonymous user is allowed to access the resource or not.
        //var allowAnonymous = !await dbContext.ApplicationPermissions.AnyAsync(p => p.Name == permission);
        //if (allowAnonymous)
        //    return;

        // Check if the user is authenticated.
        if (context.HttpContext.User.Identity is not { IsAuthenticated: true })
            throw new UnauthorizedAccessException(string.Format(Resource.You_are_not_authorized, context.HttpContext.Request.Path));

        // Create a claim for the permission.
        var claim = new Claim("permissions", permission);

        // Check if the user has the required permission.
        if (!context.HttpContext.User.HasClaim(c => c.Type == claim.Type && c.Value == claim.Value))
            throw new ForbiddenAccessException(string.Format(Resource.You_are_forbidden, context.HttpContext.Request.Path));
    }

    #endregion Public Methods
}