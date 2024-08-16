namespace OttApiPlatform.WebAPI.Controllers;

[Route("api/[controller]")]
[BpTenantAuthorize]
public class MyTenantController : ApiController
{
    #region Public Methods

    [ProducesResponseType(typeof(ApiSuccessResponse<MyTenantForEditResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("GetTenant")]
    public async Task<IActionResult> GetTenant(GetMyTenantForEditQuery request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPut("UpdateTenant")]
    public async Task<IActionResult> UpdateTenant(UpdateMyTenantCommand request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpDelete("DeleteTenant")]
    public async Task<IActionResult> DeleteTenant(string id)
    {
        var response = await Mediator.Send(new DeleteMyTenantCommand { Id = id });
        return TryGetResult(response);
    }

    #endregion Public Methods
}