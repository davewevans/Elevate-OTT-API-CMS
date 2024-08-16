namespace OttApiPlatform.CMS.Consumers;

public class PermissionsClient : IPermissionsClient
{
    #region Private Fields

    private readonly IHttpService _httpService;

    #endregion Private Fields

    #region Public Constructors

    public PermissionsClient(IHttpService httpService)
    {
        _httpService = httpService;
    }

    #endregion Public Constructors

    #region Public Methods

    public async Task<ApiResponseWrapper<PermissionsResponse>> GetPermissions(GetPermissionsQuery request)
    {
        return await _httpService.Post<GetPermissionsQuery, PermissionsResponse>("permissions/GetPermissions", request);
    }

    #endregion Public Methods
}