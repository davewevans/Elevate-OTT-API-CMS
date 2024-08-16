namespace OttApiPlatform.Infrastructure.Services;

public class TenantResolver : ITenantResolver
{
    #region Private Fields

    private readonly IConfigReaderService _configReaderService;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;

    private Guid? _tenantId;
    private string _tenantName;

    #endregion Private Fields

    #region Public Constructors

    public TenantResolver(IConfigReaderService configReaderService, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _configReaderService = configReaderService;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    #endregion Public Constructors

    #region Public Properties

    public TenantMode TenantMode => (TenantMode)_configReaderService.GetAppTenantOptions().TenantMode;

    public DataIsolationStrategy DataIsolationStrategy => (DataIsolationStrategy)_configReaderService.GetAppTenantOptions().DataIsolationStrategy;

    public bool IsHost { get; private set; }

    #endregion Public Properties

    #region Public Methods

    public void Validate()
    {
        // Check if host is trying to access Host portal in SeparateDatabasePerTenant mode.
        if (DataIsolationStrategy == DataIsolationStrategy.SeparateDatabasePerTenant && IsHost)
            throw new InvalidOperationException(Resource.The_Host_portal_cannot_be_accessed);

        // Check if host is trying to create a new tenant in SingleTenant mode.
        var controllerName = _httpContextAccessor.GetControllerName() ?? string.Empty;
        if (controllerName.Contains("Host") && TenantMode == TenantMode.SingleTenant)
            throw new InvalidOperationException(Resource.It_is_not_possible_to_create_or_update_or_delete_a_tenant_while_in_single_tenant_mode);
    }

    public Guid? GetTenantId()
    {
        return _tenantId;
    }

    public string GetTenantName()
    {
        return _tenantName;
    }

    public void SetTenantId(Guid? tenantId)
    {
        _tenantId = tenantId;
    }

    public void SetTenantName(string tenantName)
    {
        _tenantName = tenantName;
        IsHost = string.IsNullOrEmpty(_httpContextAccessor.GetTenantName());
    }

    public void SetConnectionString(DbContextOptionsBuilder contextOptionsBuilder)
    {
        Validate();

        // Attempts to retrieve the tenant name.
        var tenantName = _httpContextAccessor.GetTenantName();

        // Get the data isolation strategy for the tenant.
        var dataIsolationStrategy = GetDataIsolationStrategy(tenantName);

        // Check the data isolation strategy for the tenant.
        switch (dataIsolationStrategy)
        {
            // If the data isolation strategy is set to use a separate database for each tenant.
            case DataIsolationStrategy.SeparateDatabasePerTenant:
                {
                    // Construct the connection string using the tenant name and set it on the DbContextOptionsBuilder.
                    var connectionString = string.Format(_configuration.GetConnectionString("TenantConnection")
                                                         // Check if the "TenantConnection"
                                                         // connection string is missing from the
                                                         // appsettings.json file.
                                                         ?? throw new InvalidOperationException("TenantConnection is missing from the appsettings."),
                                                         // Replace the placeholder in the
                                                         // connection string format string with the
                                                         // actual tenant name.
                                                         tenantName);

                    contextOptionsBuilder.UseSqlServer(connectionString);
                    break;
                }
            // If the data isolation strategy is set to use a shared database for all tenants.
            case DataIsolationStrategy.SharedDatabaseForAllTenants:
                // Use the default connection string for the DbContextOptionsBuilder.
                contextOptionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
                break;
        }
    }

    #endregion Public Methods

    #region Private Methods

    private DataIsolationStrategy GetDataIsolationStrategy(string tenantName)
    {
        // Check if the tenant name is not null and the tenant mode is set to multi-tenant and the
        // data isolation strategy is set to separate database per tenant.
        var separateDbPerTenantEnabled = tenantName is not null
                                         && TenantMode == TenantMode.MultiTenant
                                         && DataIsolationStrategy == DataIsolationStrategy.SeparateDatabasePerTenant;

        // Return database isolation strategy.
        return separateDbPerTenantEnabled
            ? DataIsolationStrategy.SeparateDatabasePerTenant
            : DataIsolationStrategy.SharedDatabaseForAllTenants;
    }

    #endregion Private Methods
}