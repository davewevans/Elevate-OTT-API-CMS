namespace OttApiPlatform.WebAPI.Controllers;

[Route("api/[controller]")]
[BpAuthorize]
public class ApplicantsController : ApiController
{
    #region Public Methods

    [ProducesResponseType(typeof(ApiSuccessResponse<ApplicantForEditResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("GetApplicant")]
    public async Task<IActionResult> GetApplicant(GetApplicantForEditQuery request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<ApplicantReferencesResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("GetApplicantReferences")]
    public async Task<IActionResult> GetApplicantReferences(GetApplicantReferencesQuery request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<ApplicantsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("GetApplicants")]
    public async Task<IActionResult> GetApplicants(GetApplicantsQuery request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<CreateApplicantResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("CreateApplicant")]
    public async Task<IActionResult> CreateApplicant(CreateApplicantCommand request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPut("UpdateApplicant")]
    public async Task<IActionResult> UpdateApplicant(UpdateApplicantCommand request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpDelete("DeleteApplicant")]
    public async Task<IActionResult> DeleteApplicant(string id)
    {
        var response = await Mediator.Send(new DeleteApplicantCommand { Id = id });
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<ExportApplicantsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("ExportAsPdf")]
    public async Task<IActionResult> ExportAsPdf(ExportApplicantsQuery request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }

    #endregion Public Methods
}