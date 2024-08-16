namespace OttApiPlatform.CMS.Consumers;

public class RolesClient : IRolesClient
{
    #region Private Fields

    private readonly IHttpService _httpService;

    #endregion Private Fields

    #region Public Constructors

    public RolesClient(IHttpService httpService)
    {
        _httpService = httpService;
    }

    #endregion Public Constructors

    #region Public Methods

    public async Task<ApiResponseWrapper<RoleForEdit>> GetRole(GetRoleForEditQuery request)
    {
        return await _httpService.Post<GetRoleForEditQuery, RoleForEdit>("identity/roles/GetRole", request);
    }

    public async Task<ApiResponseWrapper<RolePermissionsResponse>> GetRolePermissions(GetRolePermissionsForEditQuery request)
    {
        return await _httpService.Post<GetRolePermissionsForEditQuery, RolePermissionsResponse>("identity/roles/GetRolePermissions", request);
    }

    public async Task<ApiResponseWrapper<RolesResponse>> GetRoles(GetRolesQuery request)
    {
        return await _httpService.Post<GetRolesQuery, RolesResponse>("identity/roles/GetRoles", request);
    }

    public async Task<ApiResponseWrapper<CreateRoleResponse>> CreateRole(CreateRoleCommand request)
    {
        return await _httpService.Post<CreateRoleCommand, CreateRoleResponse>("identity/roles/CreateRole", request);
    }

    public async Task<ApiResponseWrapper<string>> UpdateRole(UpdateRoleCommand request)
    {
        return await _httpService.Put<UpdateRoleCommand, string>("identity/roles/UpdateRole", request);
    }

    public async Task<ApiResponseWrapper<string>> DeleteRole(string id)
    {
        return await _httpService.Delete<string>($"identity/roles/DeleteRole?id={id}");
    }

    #endregion Public Methods
}