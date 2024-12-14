using Microsoft.AspNetCore.Mvc;
using OttApiPlatform.Application.Features.ContentManagement.Assets.Queries.GetAssetForEdit;
using OttApiPlatform.Application.Features.ContentManagement.Assets.Queries.GetSasToken;

namespace OttApiPlatform.WebAPI.Controllers;

//[BpAuthorize]
[Route("api/[controller]")]
public class UploadController : ApiController
{
    [ProducesResponseType(typeof(ApiSuccessResponse<AssetForEditResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpGet("sas-token")]
    public async Task<IActionResult> GetSasToken([FromQuery] GetSasTokenQuery request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }
}
