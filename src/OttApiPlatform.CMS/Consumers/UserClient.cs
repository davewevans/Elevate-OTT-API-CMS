namespace OttApiPlatform.CMS.Consumers;

public class UsersClient : IUsersClient
{
    #region Private Fields

    private readonly IHttpService _httpService;

    #endregion Private Fields

    #region Public Constructors

    public UsersClient(IHttpService httpService)
    {
        _httpService = httpService;
    }

    #endregion Public Constructors

    #region Public Methods

    public async Task<ApiResponseWrapper<UserForEdit>> GetUser(GetUserForEditQuery request)
    {
        return await _httpService.Post<GetUserForEditQuery, UserForEdit>("identity/users/GetUser", request);
    }

    public async Task<ApiResponseWrapper<UsersResponse>> GetUsers(GetUsersQuery request)
    {
        return await _httpService.Post<GetUsersQuery, UsersResponse>("identity/users/GetUsers", request);
    }

    public async Task<ApiResponseWrapper<CreateUserResponse>> CreateUser(CreateUserCommand request)
    {
        return await _httpService.Post<CreateUserCommand, CreateUserResponse>("identity/users/CreateUser", request);
    }

    public async Task<ApiResponseWrapper<string>> UpdateUser(UpdateUserCommand request)
    {
        return await _httpService.Put<UpdateUserCommand, string>("identity/users/UpdateUser", request);
    }

    public async Task<ApiResponseWrapper<string>> DeleteUser(string id)
    {
        return await _httpService.Delete<string>($"identity/users/DeleteUser?id={id}");
    }

    public async Task<ApiResponseWrapper<UserPermissionsResponse>> GetUserPermissions(GetUserPermissionsQuery request)
    {
        return await _httpService.Post<GetUserPermissionsQuery, UserPermissionsResponse>("identity/users/GetUserPermissions", request);
    }

    public async Task<ApiResponseWrapper<string>> GrantOrRevokeUserPermissions(GrantOrRevokeUserPermissionsCommand request)
    {
        return await _httpService.Post<GrantOrRevokeUserPermissionsCommand, string>("identity/users/GrantOrRevokeUserPermissions", request);
    }

    #endregion Public Methods
}