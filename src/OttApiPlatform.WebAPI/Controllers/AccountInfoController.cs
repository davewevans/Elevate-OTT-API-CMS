using OttApiPlatform.Application.Features.AccountInfo.Commands.CreateAccountInfo;
using OttApiPlatform.Application.Features.AccountInfo.Commands.UpdateAccountInfo;
using OttApiPlatform.Application.Features.AccountInfo.Queries.GetAccountInfo;

namespace OttApiPlatform.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [BpAuthorize]
    public class AccountInfoController : ApiController
    {
        #region Public Methods

        [ProducesResponseType(typeof(ApiSuccessResponse<AccountInfoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
        [HttpPost("GetAccountInfo")]
        public async Task<IActionResult> GetAccountInfo(GetAccountInfoQuery request)
        {
            var response = await Mediator.Send(request);
            return TryGetResult(response);
        }

        [ProducesResponseType(typeof(ApiSuccessResponse<CreateAccountInfoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
        [HttpPost("CreateAccountInfo")]
        public async Task<IActionResult> CreateAccountInfo(CreateAccountInfoCommand request)
        {
            var response = await Mediator.Send(request);
            return TryGetResult(response);
        }

        [ProducesResponseType(typeof(ApiSuccessResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
        [HttpPut("UpdateAccountInfo")]
        public async Task<IActionResult> UpdateAccountInfo(UpdateAccountInfoCommand request)
        {
            var response = await Mediator.Send(request);
            return TryGetResult(response);
        }

        #endregion Public Methods
    }
}
