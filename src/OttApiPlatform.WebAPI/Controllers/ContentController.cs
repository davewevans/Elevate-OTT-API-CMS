using OttApiPlatform.Application.Features.ContentManagement.Content.Commands.CreateContent;
using OttApiPlatform.Application.Features.ContentManagement.Content.Commands.DeleteContent;
using OttApiPlatform.Application.Features.ContentManagement.Content.Commands.UpdateContent;
using OttApiPlatform.Application.Features.ContentManagement.Content.Queries.GetContent;
using OttApiPlatform.Application.Features.ContentManagement.Content.Queries.GetContentForEdit;

namespace OttApiPlatform.WebAPI.Controllers;

[Route("api/[controller]")]
[BpAuthorize]
public class ContentController : ApiController
{
    #region Public Methods

    [ProducesResponseType(typeof(ApiSuccessResponse<ContentForEditResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpGet("Content")]
    public async Task<IActionResult> GetContent([FromQuery] GetContentForEditQuery request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }
 

    [ProducesResponseType(typeof(ApiSuccessResponse<ContentResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpGet("Contents")]
    public async Task<IActionResult> GetContents([FromQuery] GetContentQuery request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<CreateContentResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("Content")]
    public async Task<IActionResult> CreateContent(CreateContentCommand request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPut("Content")]
    public async Task<IActionResult> UpdateContent(UpdateContentCommand request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpDelete("Content/{id:guid}")]
    public async Task<IActionResult> DeleteContent(Guid id)
    {
        var response = await Mediator.Send(new DeleteContentCommand { Id = id });
        return TryGetResult(response);
    }

    #endregion Public Methods
}
