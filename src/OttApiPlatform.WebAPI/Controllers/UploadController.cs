using Microsoft.AspNetCore.Mvc;
using OttApiPlatform.Application.Features.ContentManagement.Assets.Queries.GetAssetById;
using OttApiPlatform.Application.Features.ContentManagement.Assets.Queries.GetSasToken;

namespace OttApiPlatform.WebAPI.Controllers;

//[BpAuthorize]
[Route("api/[controller]")]
public class UploadController : ApiController
{
    private readonly IWebHostEnvironment _environment;

    public UploadController(IWebHostEnvironment environment)
    {
        _environment = environment;
    }


    [ProducesResponseType(typeof(ApiSuccessResponse<GetAssetByIdResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("sas-token")]
    public async Task<IActionResult> GetSasToken(GetSasTokenQuery request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }

    [HttpPost("upload-chunk")]
    public async Task<IActionResult> UploadChunk(IFormFile fileChunk, int chunkIndex, int totalChunks)
    {
        if (fileChunk == null || fileChunk.Length == 0)
            return BadRequest("Invalid chunk.");

        var uploadPath = Path.Combine(_environment.ContentRootPath, "uploads");
        Directory.CreateDirectory(uploadPath);

        var tempFilePath = Path.Combine(uploadPath, fileChunk.FileName);

        // Append chunk to the temp file
        using (var stream = new FileStream(tempFilePath, chunkIndex == 0 ? FileMode.Create : FileMode.Append))
        {
            await fileChunk.CopyToAsync(stream);
        }

        if (chunkIndex + 1 == totalChunks)
        {
            // All chunks uploaded; finalize the file
            // Optionally process or move the file here
        }

        return Ok("Chunk uploaded.");
    }

}
