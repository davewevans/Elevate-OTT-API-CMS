namespace OttApiPlatform.WebAPI.Controllers;

[Route("api/[controller]")]
public class AccountController : ApiController
{
    #region Public Methods

    [ProducesResponseType(typeof(ApiSuccessResponse<LoginResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginCommand request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<LoginWith2FaResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("LoginWith2Fa")]
    public async Task<IActionResult> LoginWith2Fa(LoginWith2FaCommand request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<LoginWithRecoveryCodeResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("LoginWithRecoveryCode")]
    public async Task<IActionResult> LoginWithRecoveryCode(LoginWithRecoveryCodeCommand request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<RegisterResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("Register")]
    public async Task<IActionResult> Register(RegisterCommand request)
    {
        await Mediator.Send(new CreateTenantCommand());

        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("ConfirmEmail")]
    public async Task<IActionResult> ConfirmEmail(ConfirmEmailCommand request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<ResendEmailConfirmationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("ResendEmailConfirmation")]
    public async Task<IActionResult> ResendEmailConfirmation(ResendEmailConfirmationCommand request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<ForgetPasswordResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("ForgotPassword")]
    public async Task<IActionResult> ForgotPassword(ForgetPasswordCommand request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("ResetPassword")]
    public async Task<IActionResult> ResetPassword(ResetPasswordCommand request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<AuthResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("RefreshToken")]
    public async Task<IActionResult> RefreshToken(RefreshTokenCommand request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }

    #endregion Public Methods
}