namespace OttApiPlatform.WebAPI.Controllers;

[Route("api/[controller]")]
[BpAuthorize]
public class AppSettingsController : ApiController
{
    #region Public Methods

    [ProducesResponseType(typeof(ApiSuccessResponse<IdentitySettingsForEditResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpGet("GetIdentitySettings")]
    public async Task<IActionResult> GetIdentitySettings()
    {
        var response = await Mediator.Send(new GetIdentitySettingsQuery());
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<IdentitySettingsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPut("UpdateIdentitySettings")]
    public async Task<IActionResult> UpdateIdentitySettings(UpdateIdentitySettingsCommand request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<FileStorageSettingsForEditResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpGet("GetFileStorageSettings")]
    public async Task<IActionResult> GetFileStorageSettings()
    {
        var response = await Mediator.Send(new GetFileStorageSettingsQuery());
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<FileStorageSettingsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPut("UpdateFileStorageSettings")]
    public async Task<IActionResult> UpdateFileStorageSettings(UpdateFileStorageSettingsCommand request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<TokenSettingsForEditResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpGet("GetTokenSettings")]
    public async Task<IActionResult> GetTokenSettings()
    {
        var response = await Mediator.Send(new GetTokenSettingsQuery());
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<TokenSettingsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPut("UpdateTokenSettings")]
    public async Task<IActionResult> UpdateTokenSettings(UpdateTokenSettingsCommand request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }

    #endregion Public Methods
}