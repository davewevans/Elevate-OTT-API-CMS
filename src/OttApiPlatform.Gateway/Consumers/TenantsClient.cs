namespace OttApiPlatform.Gateway.Consumers;

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

    public async Task<ApiResponseWrapper<CreateTenantResponse>> CreateTenant(CreateTenantCommand request)
    {
        _httpService.SetTenantHeader(value: request.Name);

        return await _httpService.Post<CreateTenantCommand, CreateTenantResponse>("api/gateway/createTenant", request);
    }

    #endregion Public Methods
}