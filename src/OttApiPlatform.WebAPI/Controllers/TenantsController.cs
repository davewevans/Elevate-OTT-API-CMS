namespace OttApiPlatform.WebAPI.Controllers;

[Route("api/[controller]")]
[BpHostAuthorize]
public class TenantsController : ApiController
{
    #region Public Methods

    [ProducesResponseType(typeof(ApiSuccessResponse<TenantForEditResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("GetTenant")]
    public async Task<IActionResult> GetTenant(GetTenantForEditQuery request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<TenantsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("GetTenants")]
    public async Task<IActionResult> GetTenants(GetTenantsQuery request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<CreateTenantResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("CreateTenant")]
    public async Task<IActionResult> CreateTenant(CreateTenantCommand request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPut("UpdateTenant")]
    public async Task<IActionResult> UpdateTenant(UpdateTenantCommand request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpDelete("DeleteTenant")]
    public async Task<IActionResult> DeleteTenant(string id)
    {
        var response = await Mediator.Send(new DeleteTenantCommand { Id = id });
        return TryGetResult(response);
    }

    #endregion Public Methods
}