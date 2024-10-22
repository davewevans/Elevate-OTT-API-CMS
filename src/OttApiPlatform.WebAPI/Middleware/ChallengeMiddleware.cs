using Stubble.Core.Contexts;

namespace OttApiPlatform.WebAPI.Middleware;

/// <summary>
/// This middleware enforce tenant access control and intercepts unauthorized requests.
/// </summary>
public class ChallengeMiddleware
{
    #region Private Fields

    private readonly RequestDelegate _request;

    #endregion Private Fields

    #region Public Constructors

    public ChallengeMiddleware(RequestDelegate requestDelegate)
    {
        _request = requestDelegate ?? throw new ArgumentNullException(nameof(requestDelegate));
    }

    #endregion Public Constructors

    #region Public Methods

    public async Task InvokeAsync(HttpContext context)
    {
        // If the context is null, throw an ArgumentNullException with a descriptive message.
        if (context == null)
            throw new ArgumentNullException(nameof(context));

        // Enforce tenant access control.
        EnforceTenantAccessControl(context);

        // Invoke the request and await the result.
        await _request(context);

        // Check the HTTP response status code.
        switch (context.Response.StatusCode)
        {
            // If the status code is 401 (Unauthorized), throw an ApiProblemDetailsException with a
            // message indicating the user is not authorized.
            case 401:
                throw new UnauthorizedAccessException(string.Format(Resource.You_are_not_authorized, context.Request.GetDisplayUrl().Split('?')[0]));

            // If the status code is 403 (Forbidden), throw an ApiProblemDetailsException with a
            // message indicating the user is forbidden.
            case 403:
                throw new ForbiddenAccessException(string.Format(Resource.You_are_forbidden, context.Request.PathBase));
        }
    }

    #endregion Public Methods

    #region Private Methods

    private static void EnforceTenantAccessControl(HttpContext context)
    {
        var isSignalRRequest = context.Request.Headers.TryGetValue("x-signalr-user-agent", out _);

        // Get the X-Tenant header from the request.
        var bpTenantHeader = context.Request.Headers["X-Tenant"];

        // Get the value of the X-Tenant header.
        var tenantHeader = bpTenantHeader.FirstOrDefault();

        if (!context.User.IsAuthenticated() || isSignalRRequest || tenantHeader is null)
            return;

        // Get the tenant name from the user claims.
        var userTenantName = context.User.Claims.FirstOrDefault(c => c.Type == "TenantName")?.Value ?? string.Empty;

        // Check if the tenant in the header matches the user's tenant name. Otherwise, throw UnauthorizedAccessException.
        if (tenantHeader != userTenantName)
            throw new UnauthorizedAccessException(Resource.You_are_not_authorized_to_access_resources_related_to_other_tenants);
    }

    #endregion Private Methods
}