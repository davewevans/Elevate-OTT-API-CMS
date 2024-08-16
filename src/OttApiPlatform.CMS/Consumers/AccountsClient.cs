namespace OttApiPlatform.CMS.Consumers;

public class AccountsClient : IAccountsClient
{
    #region Private Fields

    private readonly IHttpService _httpService;

    #endregion Private Fields

    #region Public Constructors

    public AccountsClient(IHttpService httpService)
    {
        _httpService = httpService;
    }

    #endregion Public Constructors

    #region Public Methods

    public async Task<ApiResponseWrapper<RegisterResponse>> Register(RegisterCommand request)
    {
        return await _httpService.Post<RegisterCommand, RegisterResponse>("account/Register", request);
    }

    public async Task<ApiResponseWrapper<LoginResponse>> Login(LoginCommand request)
    {
        return await _httpService.Post<LoginCommand, LoginResponse>("account/Login", request);
    }

    public async Task<ApiResponseWrapper<LoginWith2FaResponse>> LoginWith2Fa(LoginWith2FaCommand request)
    {
        return await _httpService.Post<LoginWith2FaCommand, LoginWith2FaResponse>("account/LoginWith2Fa", request);
    }

    public async Task<ApiResponseWrapper<LoginWithRecoveryCodeResponse>> LoginWithRecoveryCode(LoginWithRecoveryCodeCommand request)
    {
        return await _httpService.Post<LoginWithRecoveryCodeCommand, LoginWithRecoveryCodeResponse>("account/LoginWithRecoveryCode", request);
    }

    public async Task<ApiResponseWrapper<ForgetPasswordResponse>> ForgetPassword(ForgetPasswordCommand request)
    {
        return await _httpService.Post<ForgetPasswordCommand, ForgetPasswordResponse>("account/ForgotPassword", request);
    }

    public async Task<ApiResponseWrapper<string>> ResetPassword(ResetPasswordCommand request)
    {
        return await _httpService.Post<ResetPasswordCommand, string>("account/ResetPassword", request);
    }

    public async Task<ApiResponseWrapper<string>> ConfirmEmail(ConfirmEmailCommand request)
    {
        return await _httpService.Post<ConfirmEmailCommand, string>("account/ConfirmEmail", request);
    }

    public async Task<ApiResponseWrapper<ResendEmailConfirmationResponse>> ResendEmailConfirmation(ResendEmailConfirmationCommand request)
    {
        return await _httpService.Post<ResendEmailConfirmationCommand, ResendEmailConfirmationResponse>("account/ResendEmailConfirmation", request);
    }

    public async Task<ApiResponseWrapper<AuthResponse>> RefreshToken(RefreshTokenCommand request)
    {
        return await _httpService.Post<RefreshTokenCommand, AuthResponse>("account/RefreshToken", request);
    }

    #endregion Public Methods
}