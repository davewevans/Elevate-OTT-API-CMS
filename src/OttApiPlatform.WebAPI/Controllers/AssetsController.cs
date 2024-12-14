using Microsoft.AspNetCore.Mvc;
using Mux.Csharp.Sdk.Model;
using OttApiPlatform.Application.Features.ContentManagement.Assets.Commands.CreateAsset;
using OttApiPlatform.Application.Features.ContentManagement.Assets.Commands.DeleteAsset;
using OttApiPlatform.Application.Features.ContentManagement.Assets.Commands.UpdateAsset;
using OttApiPlatform.Application.Features.ContentManagement.Assets.Queries.GetAssetForEdit;
using OttApiPlatform.Application.Features.ContentManagement.Assets.Queries.GetAssets;

namespace OttApiPlatform.WebAPI.Controllers;

//[BpAuthorize]
[Route("api/[controller]")]
public class AssetsController : ApiController
{
    #region Public Methods

    //[ProducesResponseType(typeof(ApiSuccessResponse<AssetForEditResponse>), StatusCodes.Status200OK)]
    //[ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    //[HttpGet("Asset")]
    //public async Task<IActionResult> GetAsset([FromQuery] GetAssetForEditQuery request)
    //{
    //    var response = await Mediator.Send(request);
    //    return TryGetResult(response);
    //}


    //[ProducesResponseType(typeof(ApiSuccessResponse<AssetResponse>), StatusCodes.Status200OK)]
    //[ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    //[HttpGet]
    //public async Task<IActionResult> GetAssets([FromQuery] GetAssetsQuery request)
    //{
    //    var response = await Mediator.Send(request);
    //    return TryGetResult(response);
    //}

    [ProducesResponseType(typeof(ApiSuccessResponse<CreateAssetResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public async Task<IActionResult> CreateAsset(CreateAssetCommand request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }

    //[ProducesResponseType(typeof(ApiSuccessResponse<string>), StatusCodes.Status200OK)]
    //[ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    //[HttpPut]
    //public async Task<IActionResult> UpdateAsset(UpdateAssetCommand request)
    //{
    //    var response = await Mediator.Send(request);
    //    return TryGetResult(response);
    //}

    //[ProducesResponseType(typeof(ApiSuccessResponse<string>), StatusCodes.Status200OK)]
    //[ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    //[HttpDelete("{id:guid}")]
    //public async Task<IActionResult> DeleteAsset(Guid id)
    //{
    //    var response = await Mediator.Send(new DeleteAssetCommand { Id = id });
    //    return TryGetResult(response);
    //}

    #endregion Public Methods
}
