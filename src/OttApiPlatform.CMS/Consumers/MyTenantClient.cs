namespace OttApiPlatform.CMS.Consumers;

public class MyTenantClient : IMyTenantClient
{
    #region Private Fields

    private readonly IHttpService _httpService;

    #endregion Private Fields

    #region Public Constructors

    public MyTenantClient(IHttpService httpService)
    {
        _httpService = httpService;
    }

    #endregion Public Constructors

    #region Public Methods

    public async Task<ApiResponseWrapper<MyTenantForEdit>> GetTenant(GetMyTenantForEditQuery request)
    {
        return await _httpService.Post<GetMyTenantForEditQuery, MyTenantForEdit>("MyTenant/GetTenant", request);
    }

    public async Task<ApiResponseWrapper<string>> UpdateTenant(UpdateMyTenantCommand request)
    {
        return await _httpService.Put<UpdateMyTenantCommand, string>("MyTenant/UpdateTenant", request);
    }

    #endregion Public Methods
}