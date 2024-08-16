namespace OttApiPlatform.WebAPI.Controllers;

[Route("api/account/[controller]")]
[Authorize]
public class ManageController : ApiController
{
    #region Public Methods

    [ProducesResponseType(typeof(ApiSuccessResponse<CurrentUserForEditResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpGet("GetCurrentUser")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var response = await Mediator.Send(new GetCurrentUserForEditQuery());
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<UserAvatarForEditResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpGet("GetUserAvatar")]
    public async Task<IActionResult> GetUserAvatar()
    {
        var response = await Mediator.Send(new GetUserAvatarForEditQuery());
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPut("UpdateUserProfile")]
    public async Task<IActionResult> UpdateUserProfile(UpdateUserProfileCommand request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPut("UpdateUserAvatar")]
    public async Task<IActionResult> UpdateUserAvatar(UpdateUserAvatarCommand request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<ChangeEmailResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("ChangeEmail")]
    public async Task<IActionResult> ChangeEmail(ChangeEmailCommand request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<ChangeEmailResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [AllowAnonymous]
    [HttpPut("ConfirmEmailChange")]
    public async Task<IActionResult> ConfirmEmailChange(ConfirmEmailChangeCommand request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<ChangePasswordResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("ChangePassword")]
    public async Task<IActionResult> ChangePassword(ChangePasswordCommand request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("Disable2Fa")]
    public async Task<IActionResult> Disable2Fa()
    {
        var response = await Mediator.Send(new Disable2FaCommand());
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<LoadSharedKeyAndQrCodeUriResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpGet("LoadSharedKeyAndQrCodeUri")]
    public async Task<IActionResult> LoadSharedKeyAndQrCodeUri()
    {
        var response = await Mediator.Send(new LoadSharedKeyAndQrCodeUriQuery());
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<EnableAuthenticatorResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("EnableAuthenticator")]
    public async Task<IActionResult> EnableAuthenticator(EnableAuthenticatorCommand request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<ResetAuthenticatorResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("ResetAuthenticator")]
    public async Task<IActionResult> ResetAuthenticator()
    {
        var response = await Mediator.Send(new ResetAuthenticatorCommand());
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<SetPasswordResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("SetPassword")]
    public async Task<IActionResult> SetPassword(SetPasswordCommand request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<DownloadPersonalDataResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpGet("DownloadPersonalData")]
    public async Task<IActionResult> DownloadPersonalData()
    {
        var response = await Mediator.Send(new DownloadPersonalDataQuery());
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<User2FaStateResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpGet("CheckUser2FaState")]
    public async Task<IActionResult> CheckUser2FaState()
    {
        var response = await Mediator.Send(new CheckUser2FaStateQuery());
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<GenerateRecoveryCodesResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpGet("GenerateRecoveryCodes")]
    public async Task<IActionResult> GenerateRecoveryCodes()
    {
        var response = await Mediator.Send(new GenerateRecoveryCodesQuery());
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<TwoFactorAuthenticationStateResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpGet("Get2FaState")]
    public async Task<IActionResult> Get2FaState()
    {
        var response = await Mediator.Send(new Get2FaStateQuery());
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("DeletePersonalData")]
    public async Task<IActionResult> DeletePersonalData(DeletePersonalDataCommand request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpGet("RequirePassword")]
    public async Task<IActionResult> RequirePassword()
    {
        var response = await Mediator.Send(new RequirePasswordQuery());
        return TryGetResult(response);
    }

    #endregion Public Methods
}