namespace OttApiPlatform.WebAPI.Controllers;

[Route("api/[controller]")]
[BpAuthorize]
public class PermissionsController : ApiController
{
    #region Public Methods

    [ProducesResponseType(typeof(ApiSuccessResponse<PermissionsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("GetPermissions")]
    public async Task<IActionResult> GetPermissions(GetPermissionsQuery request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }

    #endregion Public Methods
}