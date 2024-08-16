namespace OttApiPlatform.CMS.Contracts.Consumers;

/// <summary>
/// Provides methods for managing tenants.
/// </summary>
public interface ITenantsClient
{
    #region Public Methods

    /// <summary>
    /// Gets the details of a tenant.
    /// </summary>
    /// <param name="request">The query parameters for retrieving the tenant.</param>
    /// <returns>A <see cref="TenantForEdit"/>.</returns>
    Task<ApiResponseWrapper<TenantForEdit>> GetTenant(GetTenantForEditQuery request);

    /// <summary>
    /// Gets a list of tenants.
    /// </summary>
    /// <param name="request">The query parameters for retrieving the list of tenants.</param>
    /// <returns>A <see cref="TenantsResponse"/>.</returns>
    Task<ApiResponseWrapper<TenantsResponse>> GetTenants(GetTenantsQuery request);

    /// <summary>
    /// Creates a new tenant.
    /// </summary>
    /// <param name="request">The information about the new tenant.</param>
    /// <returns>A <see cref="CreateTenantResponse"/>.</returns>
    Task<ApiResponseWrapper<CreateTenantResponse>> CreateTenant(CreateTenantCommand request);

    /// <summary>
    /// Updates the details of a tenant.
    /// </summary>
    /// <param name="request">The updated information about the tenant.</param>
    /// <returns>A success message.</returns>
    Task<ApiResponseWrapper<string>> UpdateTenant(UpdateTenantCommand request);

    /// <summary>
    /// Deletes a tenant.
    /// </summary>
    /// <param name="id">The ID of the tenant to delete.</param>
    /// <returns>A success message.</returns>
    Task<ApiResponseWrapper<string>> DeleteTenant(string id);

    #endregion Public Methods
}