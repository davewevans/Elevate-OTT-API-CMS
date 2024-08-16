namespace OttApiPlatform.WebAPI.Controllers;

[Route("api/identity/[controller]")]
[BpAuthorize]
public class UsersController : ApiController
{
    #region Public Methods

    [ProducesResponseType(typeof(ApiSuccessResponse<UserForEditResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("GetUser")]
    public async Task<IActionResult> GetUser(GetUserForEditQuery request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<UsersResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("GetUsers")]
    public async Task<IActionResult> GetUsers(GetUsersQuery request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<CreateUserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("CreateUser")]
    public async Task<IActionResult> CreateUser(CreateUserCommand request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPut("UpdateUser")]
    public async Task<IActionResult> UpdateUser(UpdateUserCommand request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpDelete("DeleteUser")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var response = await Mediator.Send(new DeleteUserCommand { Id = id });
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<UserPermissionsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("GetUserPermissions")]
    public async Task<IActionResult> GetUserPermissions(GetUserPermissionsQuery request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("GrantOrRevokeUserPermissions")]
    public async Task<IActionResult> GrantOrRevokeUserPermissions(GrantOrRevokeUserPermissionsCommand request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }

    #endregion Public Methods
}