namespace OttApiPlatform.CMS.Contracts.Consumers;

/// <summary>
/// Provides methods for managing users.
/// </summary>
public interface IUsersClient
{
    #region Public Methods

    /// <summary>
    /// Gets a user by their ID.
    /// </summary>
    /// <param name="request">The query parameters.</param>
    /// <returns>A <see cref="UserForEdit"/>.</returns>
    Task<ApiResponseWrapper<UserForEdit>> GetUser(GetUserForEditQuery request);

    /// <summary>
    /// Gets a list of users.
    /// </summary>
    /// <param name="request">The query parameters.</param>
    /// <returns>A <see cref="UsersResponse"/>.</returns>
    Task<ApiResponseWrapper<UsersResponse>> GetUsers(GetUsersQuery request);

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="request">The user information to create.</param>
    /// <returns>A <see cref="CreateUserResponse"/>.</returns>
    Task<ApiResponseWrapper<CreateUserResponse>> CreateUser(CreateUserCommand request);

    /// <summary>
    /// Updates an existing user.
    /// </summary>
    /// <param name="request">The updated user information.</param>
    /// <returns>A success message.</returns>
    Task<ApiResponseWrapper<string>> UpdateUser(UpdateUserCommand request);

    /// <summary>
    /// Deletes a user by their ID.
    /// </summary>
    /// <param name="id">The user ID.</param>
    /// <returns>A success message.</returns>
    Task<ApiResponseWrapper<string>> DeleteUser(string id);

    /// <summary>
    /// Gets the permissions for a user.
    /// </summary>
    /// <param name="request">The query parameters.</param>
    /// <returns>A <see cref="UserPermissionsResponse"/>.</returns>
    Task<ApiResponseWrapper<UserPermissionsResponse>> GetUserPermissions(GetUserPermissionsQuery request);

    /// <summary>
    /// Grants or revokes permissions for a user.
    /// </summary>
    /// <param name="request">The permission change request.</param>
    /// <returns>A success message.</returns>
    Task<ApiResponseWrapper<string>> GrantOrRevokeUserPermissions(GrantOrRevokeUserPermissionsCommand request);

    #endregion Public Methods
}