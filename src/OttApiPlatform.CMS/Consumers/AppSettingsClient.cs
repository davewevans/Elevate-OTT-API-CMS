namespace OttApiPlatform.CMS.Consumers;

public class AppSettingsClient : IAppSettingsClient
{
    #region Private Fields

    private readonly IHttpService _httpService;

    #endregion Private Fields

    #region Public Constructors

    public AppSettingsClient(IHttpService httpService)
    {
        _httpService = httpService;
    }

    #endregion Public Constructors

    #region Public Methods

    public async Task<ApiResponseWrapper<IdentitySettingsForEdit>> GetIdentitySettings()
    {
        return await _httpService.Get<IdentitySettingsForEdit>("AppSettings/GetIdentitySettings");
    }

    public async Task<ApiResponseWrapper<IdentitySettingsResponse>> UpdateIdentitySettings(UpdateIdentitySettingsCommand request)
    {
        return await _httpService.Put<UpdateIdentitySettingsCommand, IdentitySettingsResponse>("AppSettings/UpdateIdentitySettings", request);
    }

    public async Task<ApiResponseWrapper<FileStorageSettingsForEdit>> GetFileStorageSettings()
    {
        return await _httpService.Get<FileStorageSettingsForEdit>("AppSettings/GetFileStorageSettings");
    }

    public async Task<ApiResponseWrapper<TokenSettingsResponse>> UpdateFileStorageSettings(UpdateFileStorageSettingsCommand request)
    {
        return await _httpService.Put<UpdateFileStorageSettingsCommand, TokenSettingsResponse>("AppSettings/UpdateFileStorageSettings", request);
    }

    public async Task<ApiResponseWrapper<TokenSettingsForEdit>> GetTokenSettings()
    {
        return await _httpService.Get<TokenSettingsForEdit>("AppSettings/GetTokenSettings");
    }

    public async Task<ApiResponseWrapper<TokenSettingsResponse>> UpdateTokenSettings(UpdateTokenSettingsCommand request)
    {
        return await _httpService.Put<UpdateTokenSettingsCommand, TokenSettingsResponse>("AppSettings/UpdateTokenSettings", request);
    }

    #endregion Public Methods
}