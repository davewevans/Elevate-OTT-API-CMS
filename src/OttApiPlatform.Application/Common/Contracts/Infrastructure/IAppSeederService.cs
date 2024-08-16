namespace OttApiPlatform.Application.Common.Contracts.Infrastructure;

/// <summary>
/// Seeds the application database with initial data.
/// </summary>
public interface IAppSeederService
{
    #region Public Methods

    /// <summary>
    /// Seeds the application with data required for its initial setup in single tenant mode.
    /// </summary>
    /// <returns>A boolean value indicating whether the seeding operation succeeded.</returns>
    Task<bool> SeedSingleTenantModeApp();

    /// <summary>
    /// Seeds the host with data required for its initial setup in multi-tenant mode.
    /// </summary>
    /// <returns>A boolean value indicating whether the seeding operation succeeded.</returns>
    Task<bool> SeedHostApp();

    /// <summary>
    /// Seeds the tenant with data required for its initial setup in shared database strategy.
    /// </summary>
    /// <returns>A boolean value indicating whether the seeding operation succeeded.</returns>
    Task<bool> SeedTenantWithSharedDatabaseStrategy();

    /// <summary>
    /// Seeds the tenant with data required for its initial setup in separate database strategy.
    /// </summary>
    /// <returns>A boolean value indicating whether the seeding operation succeeded.</returns>
    Task<bool> SeedTenantWithSeparateDatabaseStrategy();

    #endregion Public Methods
}