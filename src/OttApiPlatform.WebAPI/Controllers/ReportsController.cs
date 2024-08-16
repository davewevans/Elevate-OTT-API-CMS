namespace OttApiPlatform.WebAPI.Controllers;

[Route("api/[controller]")]
[BpAuthorize]
public class ReportsController : ApiController
{
    #region Public Methods

    [ProducesResponseType(typeof(ApiSuccessResponse<ReportForEditResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("GetReport")]
    public async Task<IActionResult> GetReport(GetReportForEditQuery request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<ReportsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("GetReports")]
    public async Task<IActionResult> GetReports(GetReportsQuery request)
    {
        var response = await Mediator.Send(request);
        return TryGetResult(response);
    }

    #endregion Public Methods
}