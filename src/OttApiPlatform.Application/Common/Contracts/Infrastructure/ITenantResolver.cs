namespace OttApiPlatform.Application.Common.Contracts.Infrastructure;

/// <summary>
/// Resolves tenant ID and name.
/// </summary>
public interface ITenantResolver
{
    #region Public Properties

    /// <summary>
    /// Gets the tenant mode for the current request.
    /// </summary>
    TenantMode TenantMode { get; }

    /// <summary>
    /// Gets the data isolation strategy.
    /// </summary>
    DataIsolationStrategy DataIsolationStrategy { get; }

    /// <summary>
    /// Gets or sets a value indicating whether the current request is for the host (non-tenant) context.
    /// </summary>
    bool IsHost { get; }

    #endregion Public Properties

    #region Public Methods

    /// <summary>
    /// Gets the ID of the current tenant.
    /// </summary>
    /// <returns>The ID of the current tenant, or null if no tenant ID has been set.</returns>
    Guid? GetTenantId();

    /// <summary>
    /// Sets the ID of the current tenant.
    /// </summary>
    /// <param name="tenantId">The ID of the tenant to set.</param>
    void SetTenantId(Guid? tenantId);

    /// <summary>
    /// Sets the connection string for the DbContextOptionsBuilder.
    /// </summary>
    /// <param name="contextOptionsBuilder">
    /// The DbContextOptionsBuilder to set the connection string for.
    /// </param>
    void SetConnectionString(DbContextOptionsBuilder contextOptionsBuilder);

    /// <summary>
    /// Validates the current tenant configuration.
    /// </summary>
    /// <remarks>Throws an <see cref="InvalidOperationException"/> if the configuration is invalid.</remarks>
    void Validate();

    void SetTenantName(string tenantName);

    string GetTenantName();

    #endregion Public Methods
}