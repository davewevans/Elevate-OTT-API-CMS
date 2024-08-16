namespace OttApiPlatform.Infrastructure.Persistence;

/// <summary>
/// Provides a method for seeding the application database with default data.
/// </summary>
public static class ApplicationDbContextSeeder
{
    #region Public Methods

    /// <summary>
    /// Seeds the application database with default data.
    /// </summary>
    /// <param name="appSeederService">The application seeder service used to seed the database.</param>
    /// <param name="tenantMode">The tenant mode for the application.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public static async Task SeedAsync(IAppSeederService appSeederService, TenantMode tenantMode)
    {
        // Seeds the application data based on the tenant mode.
        switch (tenantMode)
        {
            case TenantMode.SingleTenant:
                // Seed the application for single tenant mode
                await appSeederService.SeedSingleTenantModeApp();
                break;

            case TenantMode.MultiTenant:
                // Seed the host application for multi-tenant mode
                await appSeederService.SeedHostApp();
                break;

            default:
                // Throw an exception for unsupported tenant mode
                throw new ArgumentOutOfRangeException(nameof(tenantMode), tenantMode, null);
        }
    }

    #endregion Public Methods
}