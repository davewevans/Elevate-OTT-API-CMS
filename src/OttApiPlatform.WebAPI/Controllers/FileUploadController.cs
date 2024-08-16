namespace OttApiPlatform.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class FileUploadController : ApiController
    {
        #region Public Methods

        [ProducesResponseType(typeof(ApiSuccessResponse<FileUploadResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
        [HttpPost("UploadFile")]
        [RequestFormLimits(MultipartBodyLengthLimit = 999999999)]
        public async Task<IActionResult> UploadFile([FromForm] FileUploadCommand request, CancellationToken cancellationToken)
        {
            var response = await Mediator.Send(request, cancellationToken);
            return TryGetResult(response);
        }

        #endregion Public Methods
    }
}