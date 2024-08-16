namespace OttApiPlatform.CMS.Consumers;

public class TenantsClient : ITenantsClient
{
    #region Private Fields

    private readonly IHttpService _httpService;

    #endregion Private Fields

    #region Public Constructors

    public TenantsClient(IHttpService httpService)
    {
        _httpService = httpService;
    }

    #endregion Public Constructors

    #region Public Methods

    public async Task<ApiResponseWrapper<TenantForEdit>> GetTenant(GetTenantForEditQuery request)
    {
        return await _httpService.Post<GetTenantForEditQuery, TenantForEdit>("tenants/GetTenant", request);
    }

    public async Task<ApiResponseWrapper<TenantsResponse>> GetTenants(GetTenantsQuery request)
    {
        return await _httpService.Post<GetTenantsQuery, TenantsResponse>("tenants/GetTenants", request);
    }

    public async Task<ApiResponseWrapper<CreateTenantResponse>> CreateTenant(CreateTenantCommand request)
    {
        return await _httpService.Post<CreateTenantCommand, CreateTenantResponse>("tenants/CreateTenant", request);
    }

    public async Task<ApiResponseWrapper<string>> UpdateTenant(UpdateTenantCommand request)
    {
        return await _httpService.Put<UpdateTenantCommand, string>("tenants/UpdateTenant", request);
    }

    public async Task<ApiResponseWrapper<string>> DeleteTenant(string id)
    {
        return await _httpService.Delete<string>($"tenants/DeleteTenant?id={id}");
    }

    #endregion Public Methods
}