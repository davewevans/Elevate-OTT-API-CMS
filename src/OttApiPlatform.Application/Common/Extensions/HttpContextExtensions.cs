using Microsoft.AspNetCore.Routing;

namespace OttApiPlatform.Application.Common.Extensions;

/// <summary>
/// Provides extension methods for accessing properties of the <see cref="HttpContext"/> class.
/// </summary>
public static class HttpContextExtensions
{
    #region Public Methods

    /// <summary>
    /// Gets the user ID from the <see cref="HttpContext"/>.
    /// </summary>
    /// <param name="httpContextAccessor">The <see cref="IHttpContextAccessor"/> instance.</param>
    /// <returns>The user ID as a string, or an empty string if it is not found.</returns>
    public static string GetUserId(this IHttpContextAccessor httpContextAccessor)
    {
        var userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        return userId ?? string.Empty;
    }

    /// <summary>
    /// Gets the user name from the <see cref="HttpContext"/>.
    /// </summary>
    /// <param name="httpContextAccessor">The <see cref="IHttpContextAccessor"/> instance.</param>
    /// <returns>The user name as a string.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the <see cref="UserManager{TUser}"/> instance is null.
    /// </exception>
    public static string GetUserName(this IHttpContextAccessor httpContextAccessor)
    {
        var userManager = httpContextAccessor.HttpContext?.RequestServices.GetService<UserManager<ApplicationUser>>();

        if (userManager == null)
            throw new ArgumentException(nameof(userManager));

        var userName = userManager.GetUserName(httpContextAccessor.HttpContext.User);

        return userName;
    }

    /// <summary>
    /// Gets the language from the <see cref="HttpContext"/>.
    /// </summary>
    /// <param name="httpContextAccessor">The <see cref="IHttpContextAccessor"/> instance.</param>
    /// <returns>The language as a string.</returns>
    public static string GetLanguage(this IHttpContextAccessor httpContextAccessor)
    {
        var language = httpContextAccessor.HttpContext?.Request.Headers["Accept-Language"].ToString();

        return language;
    }

    /// <summary>
    /// Extension method to get the name of the requested controller from the provided
    /// IHttpContextAccessor object.
    /// </summary>
    /// <param name="httpContextAccessor">The IHttpContextAccessor object.</param>
    /// <returns>The name of the requested controller.</returns>
    public static string GetControllerName(this IHttpContextAccessor httpContextAccessor)
    {
        if (httpContextAccessor.HttpContext == null)
            return string.Empty;

        var controllerName = httpContextAccessor.HttpContext.GetRouteData().Values["controller"]?.ToString();
        return controllerName;
    }

    /// <summary>
    /// Gets the tenant name from the <see cref="HttpContext"/>.
    /// </summary>
    /// <param name="httpContextAccessor">The <see cref="IHttpContextAccessor"/> instance.</param>
    /// <returns>
    /// The tenant name as a string, or an empty string if it is not found. If the
    /// "X-TenantByGatewayClient" header is present, returns its value. Otherwise, returns the
    /// value of the "X-Tenant" header.
    /// </returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static string GetTenantName(this IHttpContextAccessor httpContextAccessor)
    {
        // Check if the HttpContext is available
        if (httpContextAccessor.HttpContext == null)
            return string.Empty;

        // Retrieve the value of the X-Tenant header created by the host owner through the Tenant
        // Portal Client App.
        var tenantName = httpContextAccessor.HttpContext.Request.Headers["X-Tenant"];

        // Retrieve the value of the X-TenantByGatewayClient header created by an external user
        // through the Tenant Gateway Client App.
        var tenantNameByGatewayClient = httpContextAccessor.HttpContext.Request.Headers["X-TenantByGatewayClient"];

        // Check if the X-TenantByGatewayClient header has a value and is not empty
        if (tenantNameByGatewayClient.Count != 0 && !string.IsNullOrWhiteSpace(tenantNameByGatewayClient))
            return tenantNameByGatewayClient;

        // If the X-TenantByGatewayClient header is empty or not present, return the value of the
        // X-Tenant header
        return tenantNameByGatewayClient.Count == 0 ? tenantName : tenantNameByGatewayClient;
    }

    /// <summary>
    /// Gets the client application host name from the <see cref="HttpContext"/>.
    /// </summary>
    /// <param name="httpContextAccessor">The <see cref="IHttpContextAccessor"/> instance.</param>
    /// <returns>The client application host name as a string.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static string GetClientAppHostName(this IHttpContextAccessor httpContextAccessor)
    {
        // Check if the HttpContext is available
        if (httpContextAccessor.HttpContext == null)
            throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));

        var configReader = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IConfigReaderService>();

        var tenantResolver = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<ITenantResolver>();

        var clientAppOptions = configReader.GetClientAppOptions();

        return tenantResolver.TenantMode switch
        {
            TenantMode.MultiTenant => tenantResolver.IsHost ? clientAppOptions.SingleTenantHostName
                : string.Format(clientAppOptions.MultiTenantHostName, configReader.GetSubDomain()),
            _ => clientAppOptions.SingleTenantHostName
        };
    }

    /// <summary>
    /// Retrieves the tenant name from the "X-Tenant" request header in the <see cref="HttpContext"/>.
    /// </summary>
    /// <param name="httpContextAccessor">The <see cref="IHttpContextAccessor"/> instance.</param>
    /// <returns>The tenant name as a string, or an empty string if the header is not present or empty.</returns>
    public static string GetTenantFromRequestHeader(this IHttpContextAccessor httpContextAccessor)
    {
        string tenantName = string.Empty;
        if (httpContextAccessor.HttpContext != null)
            tenantName = httpContextAccessor.HttpContext.Request.Headers["X-Tenant"];

        return (tenantName.Length == 0) ? string.Empty : tenantName;
    }

    #endregion Public Methods
}