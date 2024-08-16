namespace OttApiPlatform.CMS.Contracts.Consumers;

/// <summary>
/// Provides methods for managing roles.
/// </summary>
public interface IRolesClient
{
    #region Public Methods

    /// <summary>
    /// Get a role by ID for editing.
    /// </summary>
    /// <param name="request">The ID of the role to get.</param>
    /// <returns>A <see cref="RoleForEdit"/>.</returns>
    Task<ApiResponseWrapper<RoleForEdit>> GetRole(GetRoleForEditQuery request);

    /// <summary>
    /// Get a list of permissions for a role for editing.
    /// </summary>
    /// <param name="request">The ID of the role to get permissions for.</param>
    /// <returns>A <see cref="RolePermissionsResponse"/>.</returns>
    Task<ApiResponseWrapper<RolePermissionsResponse>> GetRolePermissions(GetRolePermissionsForEditQuery request);

    /// <summary>
    /// Get a list of roles.
    /// </summary>
    /// <param name="request">The query parameters for filtering, sorting and pagination.</param>
    /// <returns>A <see cref="RolesResponse"/>.</returns>
    Task<ApiResponseWrapper<RolesResponse>> GetRoles(GetRolesQuery request);

    /// <summary>
    /// Create a new role.
    /// </summary>
    /// <param name="request">The details of the role to create.</param>
    /// <returns>A <see cref="CreateRoleResponse"/>.</returns>
    Task<ApiResponseWrapper<CreateRoleResponse>> CreateRole(CreateRoleCommand request);

    /// <summary>
    /// Update a role by ID.
    /// </summary>
    /// <param name="request">The details of the role to update.</param>
    /// <returns>A success message.</returns>
    Task<ApiResponseWrapper<string>> UpdateRole(UpdateRoleCommand request);

    /// <summary>
    /// Delete a role by ID.
    /// </summary>
    /// <param name="id">The ID of the role to delete.</param>
    /// <returns>A success message.</returns>
    Task<ApiResponseWrapper<string>> DeleteRole(string id);

    #endregion Public Methods
}