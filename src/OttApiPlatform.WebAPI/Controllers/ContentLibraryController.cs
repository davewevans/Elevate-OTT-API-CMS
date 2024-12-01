using Microsoft.AspNetCore.Mvc;

namespace OttApiPlatform.WebAPI.Controllers;

[Route("api/[controller]")]
[BpAuthorize]
public class ContentLibraryController : ApiController
{
    #region Public Methods

    //[ProducesResponseType(typeof(ApiSuccessResponse<ContentResponse>), StatusCodes.Status200OK)]
    //[ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    //[HttpPost("GetContent")]
    //public async Task<IActionResult> GetContent(GetContentQuery request)
    //{
    //    var response = await Mediator.Send(request);
    //    return TryGetResult(response);
    //}

    //[ProducesResponseType(typeof(ApiSuccessResponse<ContentReferencesResponse>), StatusCodes.Status200OK)]
    //[ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    //[HttpPost("GetContentReferences")]
    //public async Task<IActionResult> GetContentReferences(GetContentReferencesQuery request)
    //{
    //    var response = await Mediator.Send(request);
    //    return TryGetResult(response);
    //}

    //[ProducesResponseType(typeof(ApiSuccessResponse<ContentsResponse>), StatusCodes.Status200OK)]
    //[ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    //[HttpPost("GetContents")]
    //public async Task<IActionResult> GetContents(GetContentsQuery request)
    //{
    //    var response = await Mediator.Send(request);
    //    return TryGetResult(response);
    //}

    //[ProducesResponseType(typeof(ApiSuccessResponse<CreateContentResponse>), StatusCodes.Status200OK)]
    //[ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    //[HttpPost("CreateContent")]
    //public async Task<IActionResult> CreateContent(CreateContentCommand request)
    //{
    //    var response = await Mediator.Send(request);
    //    return TryGetResult(response);
    //}

    //[ProducesResponseType(typeof(ApiSuccessResponse<string>), StatusCodes.Status200OK)]
    //[ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    //[HttpPut("UpdateContent")]
    //public async Task<IActionResult> UpdateContent(UpdateContentCommand request)
    //{
    //    var response = await Mediator.Send(request);
    //    return TryGetResult(response);
    //}

    //[ProducesResponseType(typeof(ApiSuccessResponse<string>), StatusCodes.Status200OK)]
    //[ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    //[HttpDelete("DeleteContent")]
    //public async Task<IActionResult> DeleteContent(string id)
    //{
    //    var response = await Mediator.Send(new DeleteContentCommand { Id = id });
    //    return TryGetResult(response);
    //}

    #endregion Public Methods
}
