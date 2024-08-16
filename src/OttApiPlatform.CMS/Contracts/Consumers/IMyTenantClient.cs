namespace OttApiPlatform.CMS.Contracts.Consumers;

/// <summary>
/// Provides methods for my tenant.
/// </summary>
public interface IMyTenantClient
{
    #region Public Methods

    /// <summary>
    /// Retrieves the tenant for the currently authenticated user.
    /// </summary>
    /// <param name="request">The query parameter for retrieving a tenant.</param>
    /// <returns>A <see cref="MyTenantForEdit"/>.</returns>
    Task<ApiResponseWrapper<MyTenantForEdit>> GetTenant(GetMyTenantForEditQuery request);

    /// <summary>
    /// Updates the tenant for the currently authenticated user.
    /// </summary>
    /// <param name="request">The command parameter for updating a tenant.</param>
    /// <returns>A success message.</returns>
    Task<ApiResponseWrapper<string>> UpdateTenant(UpdateMyTenantCommand request);

    #endregion Public Methods
}