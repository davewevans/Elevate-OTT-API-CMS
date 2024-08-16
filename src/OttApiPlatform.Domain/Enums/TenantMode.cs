namespace OttApiPlatform.Domain.Enums;

/// <summary>
/// The tenant mode options.
/// </summary>
public enum TenantMode
{
    /// <summary>
    /// Indicates that the application is configured to only support a single tenant.
    /// </summary>
    SingleTenant = 1,

    /// <summary>
    /// Indicates that the application is configured to support multiple tenants.
    /// </summary>
    MultiTenant = 2,
}