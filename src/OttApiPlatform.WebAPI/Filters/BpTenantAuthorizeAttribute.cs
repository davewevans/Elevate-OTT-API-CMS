namespace OttApiPlatform.WebAPI.Filters;

/// <summary>
/// Custom authorization attribute used to check if the current tenant user has permission to access
/// a resource.
/// </summary>
public class BpTenantAuthorizeAttribute : BpAuthorizeAttribute
{
    #region Public Methods

    /// <summary>
    /// Overrides the default authorization behavior to check if the user is a tenant user.
    /// </summary>
    /// <param name="context">The context for the authorization filter.</param>
    public override void OnAuthorization(AuthorizationFilterContext context)
    {
        // Check if the user is authenticated.
        if (context.HttpContext.User.Identity is not { IsAuthenticated: true })
            throw new UnauthorizedAccessException(string.Format(Resource.You_are_not_authorized, context.HttpContext.Request.Path));

        // Check if the user has the "IsHostUser" claim with a value of "true". If so, throw an exception.
        if (context.HttpContext.User.HasClaim(c => c.Type == "IsHostUser" && c.Value == "true"))
            throw new ForbiddenAccessException(string.Format(Resource.You_are_forbidden, context.HttpContext.Request.Path));

        base.OnAuthorization(context);
    }

    #endregion Public Methods
}