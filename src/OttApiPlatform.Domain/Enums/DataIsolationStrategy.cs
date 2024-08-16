namespace OttApiPlatform.Domain.Enums;

/// <summary>
/// Enumerates the data isolation strategies that can be used in a multi-tenant application.
/// </summary>
public enum DataIsolationStrategy
{
    /// <summary>
    /// All tenants share the same database.
    /// </summary>
    SharedDatabaseForAllTenants = 1,

    /// <summary>
    /// Each tenant has its own separate database.
    /// </summary>
    SeparateDatabasePerTenant = 2
}