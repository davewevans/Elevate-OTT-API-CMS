namespace OttApiPlatform.CMS.Consumers;

public class ManageClient : IManageClient
{
    #region Private Fields

    private readonly IHttpService _httpService;

    #endregion Private Fields

    #region Public Constructors

    public ManageClient(IHttpService httpService)
    {
        _httpService = httpService;
    }

    #endregion Public Constructors

    #region Public Methods

    public async Task<ApiResponseWrapper<CurrentUserForEdit>> GetUser()
    {
        return await _httpService.Get<CurrentUserForEdit>("account/manage/GetCurrentUser");
    }

    public async Task<ApiResponseWrapper<UserAvatarForEdit>> GetUserAvatar()
    {
        return await _httpService.Get<UserAvatarForEdit>("account/manage/GetUserAvatar");
    }

    public async Task<ApiResponseWrapper<string>> UpdateUserProfile(UpdateUserProfileCommand request)
    {
        return await _httpService.Put<UpdateUserProfileCommand, string>("account/manage/UpdateUserProfile", request);
    }

    public async Task<ApiResponseWrapper<string>> UpdateUserAvatar(UpdateUserAvatarCommand request)
    {
        return await _httpService.Put<UpdateUserAvatarCommand, string>("account/manage/UpdateUserAvatar", request);
    }

    public async Task<ApiResponseWrapper<ChangeEmailResponse>> ChangeEmail(ChangeEmailCommand request)
    {
        return await _httpService.Post<ChangeEmailCommand, ChangeEmailResponse>("account/manage/ChangeEmail", request);
    }

    public async Task<ApiResponseWrapper<ChangeEmailResponse>> ConfirmEmailChange(ConfirmEmailChangeCommand request)
    {
        return await _httpService.Put<ConfirmEmailChangeCommand, ChangeEmailResponse>("account/manage/ConfirmEmailChange", request);
    }

    public async Task<ApiResponseWrapper<ChangePasswordResponse>> ChangePassword(ChangePasswordCommand request)
    {
        return await _httpService.Post<ChangePasswordCommand, ChangePasswordResponse>("account/manage/ChangePassword", request);
    }

    public async Task<ApiResponseWrapper<TwoFactorAuthenticationStateResponse>> Get2FaState()
    {
        return await _httpService.Get<TwoFactorAuthenticationStateResponse>("account/manage/Get2FaState");
    }

    public async Task<ApiResponseWrapper<LoadSharedKeyAndQrCodeUriResponse>> LoadSharedKeyAndQrCodeUri()
    {
        return await _httpService.Get<LoadSharedKeyAndQrCodeUriResponse>("account/manage/LoadSharedKeyAndQrCodeUri");
    }

    public async Task<ApiResponseWrapper<EnableAuthenticatorResponse>> EnableAuthenticator(EnableAuthenticatorCommand request)
    {
        return await _httpService.Post<EnableAuthenticatorCommand, EnableAuthenticatorResponse>("account/manage/EnableAuthenticator", request);
    }

    public async Task<ApiResponseWrapper<string>> Disable2Fa()
    {
        return await _httpService.Post<string>("account/manage/Disable2Fa");
    }

    public async Task<ApiResponseWrapper<GenerateRecoveryCodesResponse>> GenerateRecoveryCodes()
    {
        return await _httpService.Get<GenerateRecoveryCodesResponse>("account/manage/GenerateRecoveryCodes");
    }

    public async Task<ApiResponseWrapper<User2FaStateResponse>> CheckUser2FaState()
    {
        return await _httpService.Get<User2FaStateResponse>("account/manage/CheckUser2FaState");
    }

    public async Task<ApiResponseWrapper<ResetAuthenticatorResponse>> ResetAuthenticator()
    {
        return await _httpService.Post<ResetAuthenticatorResponse>("account/manage/ResetAuthenticator");
    }

    public async Task<ApiResponseWrapper<DownloadPersonalDataResponse>> DownloadPersonalData()
    {
        return await _httpService.Get<DownloadPersonalDataResponse>("account/manage/DownloadPersonalData");
    }

    public async Task<ApiResponseWrapper<string>> DeletePersonalData(DeletePersonalDataCommand request)
    {
        return await _httpService.Post<DeletePersonalDataCommand, string>("account/manage/DeletePersonalData", request);
    }

    public async Task<ApiResponseWrapper<bool>> RequirePassword()
    {
        return await _httpService.Get<bool>("account/manage/RequirePassword");
    }

    #endregion Public Methods
}