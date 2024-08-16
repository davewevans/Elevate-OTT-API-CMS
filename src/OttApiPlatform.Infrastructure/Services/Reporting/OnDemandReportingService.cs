namespace OttApiPlatform.Infrastructure.Services.Reporting;

public class OnDemandReportingService : IOnDemandReportingService
{
    #region Private Fields

    private readonly IHtmlReportBuilderService _htmlReportBuilderService;

    #endregion Private Fields

    #region Public Constructors

    public OnDemandReportingService(IHtmlReportBuilderService htmlReportBuilderService)
    {
        _htmlReportBuilderService = htmlReportBuilderService;
    }

    #endregion Public Constructors

    #region Public Methods

    public async Task<ExportApplicantsResponse> ExportApplicantToPdfOnDemand(ApplicantsResponse applicantsResponse, string issuerName, string baseUrl = null)
    {
        // Generate the PDF file from the provided HTML template and the applicant data.
        var fileMetaData = await _htmlReportBuilderService.GenerateApplicantsPdfFromHtml(applicantsResponse.Applicants.Items, baseUrl);

        // Create and return the ExportApplicantsResponse object containing the file metadata.
        var response = new ExportApplicantsResponse
        {
            FileName = fileMetaData.FileName,
            FileUrl = fileMetaData.FileUri,
            ContentType = fileMetaData.ContentType
        };

        return response;
    }

    #endregion Public Methods
}