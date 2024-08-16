namespace OttApiPlatform.CMS.Contracts.Consumers;

/// <summary>
/// Provides methods for managing permissions.
/// </summary>
public interface IPermissionsClient
{
    #region Public Methods

    /// <summary>
    /// Gets the list of permissions.
    /// </summary>
    /// <param name="request">The query to filter the list of permissions.</param>
    /// <returns>A <see cref="PermissionsResponse"/>.</returns>
    Task<ApiResponseWrapper<PermissionsResponse>> GetPermissions(GetPermissionsQuery request);

    #endregion Public Methods
}