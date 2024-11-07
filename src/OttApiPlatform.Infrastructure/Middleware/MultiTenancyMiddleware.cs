using Newtonsoft.Json;
using OttApiPlatform.Application.Features.Account.Commands.ConfirmEmail;

namespace OttApiPlatform.Infrastructure.Middleware;

/// <summary>
/// Middleware for handling tenant resolution logic.
/// </summary>
public class MultiTenancyMiddleware
{
    #region Private Fields

    // The next middleware in the pipeline.
    private readonly RequestDelegate _next;

    #endregion Private Fields

    // The options for this middleware.

    #region Public Constructors

    public MultiTenancyMiddleware(RequestDelegate next)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
    }

    #endregion Public Constructors

    #region Public Methods

    public async Task InvokeAsync(HttpContext httpContext, IApplicationDbContext dbContext, ITenantResolver tenantResolver, ICacheService cacheService)
    {
        // Handle the tenant resolution logic based on the tenant mode.
        switch (tenantResolver.TenantMode)
        {
            case TenantMode.MultiTenant when httpContext == null:
                // Throw an exception if the HTTP context is null in multi-tenant mode.
                throw new ArgumentNullException(nameof(httpContext));

            // If the TenantMode is MultiTenant.
            case TenantMode.MultiTenant:
                {
                    // Check if the request is for email confirmation
                    if (httpContext.Request.Path.StartsWithSegments("/api/account/ConfirmEmail"))
                    {
                        await SetTenantIdForConfirmEmailRequest(httpContext, dbContext, tenantResolver);
                        break;
                    }
                    
                    // Get the tenant name value of the X-Tenant header from the request.
                    var tenantName = GetTenantName(httpContext);

                    // If the X-Tenant header is not present in the request, set the tenant ID to
                    // null and the tenant name to an empty string.
                    if (tenantName is null)
                    {
                        tenantResolver.SetTenantId(null);
                        tenantResolver.SetTenantName(string.Empty);
                    }
                    else
                    {
                        // Try to retrieve the tenant ID from the cache, or retrieves it from the
                        // database if not found.
                        var tenantId = TryGetCachedTenantId(dbContext, cacheService, tenantName);

                        // If the tenant ID is empty and the path value does not contain "hangfire"
                        // or "/Hubs/", throw an exception.
                        if (httpContext.Request.Path.Value is { } pathValue &&
                            tenantId == Guid.Empty &&
                            !pathValue.Contains("hangfire") &&
                            !pathValue.Contains("/Hubs/"))
                            throw new Exception(Resource.Invalid_tenant_name);

                        // Set the tenant ID in the tenant resolver.
                        tenantResolver.SetTenantId(tenantId);
                        tenantResolver.SetTenantName(tenantName);
                    }

                    break;
                }

            // If the TenantMode is SingleTenant.
            case TenantMode.SingleTenant:
                {
                    // Set TenantId to Guid.Empty and the tenant name to an empty string in the tenantResolver.
                    tenantResolver.SetTenantId(Guid.Empty);
                    tenantResolver.SetTenantName(string.Empty);
                    break;
                }
        }

        // Call the next middleware in the pipeline.
        await _next(httpContext);
    }

    private static async Task SetTenantIdForConfirmEmailRequest(HttpContext httpContext, IApplicationDbContext dbContext,
        ITenantResolver tenantResolver)
    {
        // Enable buffering to allow the request body to be read multiple times
        httpContext.Request.EnableBuffering();

        // Extract UserId from the POST request body
        var requestBody = await new StreamReader(httpContext.Request.Body).ReadToEndAsync();
        var confirmEmailRequest = JsonConvert.DeserializeObject<ConfirmEmailCommand>(requestBody);

        if (confirmEmailRequest != null && !string.IsNullOrEmpty(confirmEmailRequest.UserId))
        {
            // Query the database for the user
            var user = await dbContext.Users
                .Include(u => u.Tenant)
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(u => u.Id == confirmEmailRequest.UserId);

            if (user != null)
            {
                // Set the TenantId to the user's TenantId
                tenantResolver.SetTenantId(user.Tenant.Id);
                tenantResolver.SetTenantName(user.Tenant.Name);
            }
        }

        // Reset the request body stream position to the beginning
        httpContext.Request.Body.Position = 0;
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Tries to retrieve the tenant ID from the cache, or retrieves it from the database if not found.
    /// </summary>
    /// <param name="dbContext">The application database context.</param>
    /// <param name="cacheService">The cache service.</param>
    /// <param name="tenantName">The name of the tenant.</param>
    /// <returns>The tenant ID if found; otherwise, null.</returns>
    private static Guid? TryGetCachedTenantId(IApplicationDbContext dbContext, ICacheService cacheService, string tenantName)
    {
        var cacheKey = $"tenant_{tenantName}";

        // Try to retrieve the tenant ID from the cache
        var tenantId = cacheService.Get<Guid>(cacheKey);

        if (tenantId == Guid.Empty)
        {
            // Tenant ID not found in the cache, retrieve it from the database
            var tenant = dbContext.Tenants.FirstOrDefault(t => t.Name == tenantName);

            if (tenant != null)
            {
                // Found the tenant, get the ID and store it in the cache
                tenantId = tenant.Id;
                cacheService.Set(cacheKey, tenantId, TimeSpan.FromMinutes(60));
            }
        }

        return tenantId;
    }

    private string GetTenantName(HttpContext httpContext)
    {
        return httpContext.Request.Headers["X-Tenant"];
    }

    #endregion Private Methods
}