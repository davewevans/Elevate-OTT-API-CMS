namespace OttApiPlatform.WebAPI.Controllers;

[Route("api/[controller]")]
[AllowAnonymous]
public class GatewayController : ApiController
{
    #region Public Methods

    [ProducesResponseType(typeof(ApiSuccessResponse<CreateTenantResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("CreateTenant")]
    public async Task<IActionResult> CreateTenant(CreateTenantCommand request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }

    #endregion Public Methods
}