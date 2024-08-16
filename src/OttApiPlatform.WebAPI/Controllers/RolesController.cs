namespace OttApiPlatform.WebAPI.Controllers;

[Route("api/identity/[controller]")]
[BpAuthorize]
public class RolesController : ApiController
{
    #region Public Methods

    [ProducesResponseType(typeof(ApiSuccessResponse<RoleForEditResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("GetRole")]
    public async Task<IActionResult> GetRole(GetRoleForEditQuery request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<RolePermissionsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("GetRolePermissions")]
    public async Task<IActionResult> GetRolePermissions(GetRolePermissionsForEditQuery request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<RolesResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("GetRoles")]
    public async Task<IActionResult> GetRoles(GetRolesQuery request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<CreateRoleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("CreateRole")]
    public async Task<IActionResult> CreateRole(CreateRoleCommand request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPut("UpdateRole")]
    public async Task<IActionResult> UpdateRole(UpdateRoleCommand request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpDelete("DeleteRole")]
    public async Task<IActionResult> DeleteRole(string id)
    {
        var response = await Mediator.Send(new DeleteRoleCommand { Id = id });
        return TryGetResult(response);
    }

    #endregion Public Methods
}