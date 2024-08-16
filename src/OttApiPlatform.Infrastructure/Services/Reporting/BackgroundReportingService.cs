namespace OttApiPlatform.Infrastructure.Services.Reporting;

public class BackgroundReportingService : IBackgroundReportingService
{
    #region Private Fields

    private readonly ITenantResolver _tenantResolver;
    private readonly IUtcTimeService _utcTimeService;
    private readonly IApplicationDbContext _dbContext;
    private readonly IHubNotificationService _hubNotificationService;
    private readonly IApplicantsReaderService _applicantsReaderService;
    private readonly IHtmlReportBuilderService _htmlReportBuilderService;

    #endregion Private Fields

    #region Public Constructors

    public BackgroundReportingService(ITenantResolver tenantResolver,
                                      IUtcTimeService utcTimeService,
                                      IApplicationDbContext dbContext,
                                      IHubNotificationService hubNotificationService,
                                      IApplicantsReaderService applicantsReaderService,
                                      IHtmlReportBuilderService htmlReportBuilderService)
    {
        _tenantResolver = tenantResolver;
        _utcTimeService = utcTimeService;
        _dbContext = dbContext;
        _hubNotificationService = hubNotificationService;
        _applicantsReaderService = applicantsReaderService;
        _htmlReportBuilderService = htmlReportBuilderService;
    }

    #endregion Public Constructors

    #region Public Methods

    public async Task<FileMetaData> ExportApplicantsAsPdfInBackground(ExportApplicantsQuery request,
                                                                      Guid reportId,
                                                                      string baseUrl,
                                                                      string userNameIdentifier,
                                                                      Guid? tenantId)
    {
        // Set the tenant ID if it's not null.
        SetTenantIdIfNotNull(tenantId);

        // Create a new FileMetaData object.
        FileMetaData fileMetaData = new();

        try
        {
            // Update the report status to in progress and notify the user.
            await UpdateStatusAndNotify(reportId, ReportStatus.InProgress, userNameIdentifier, fileMetaData);

            // Get the applicants data.
            var applicantsResponse = await _applicantsReaderService.GetApplicants(new GetApplicantsQuery
            {
                SearchText = request.SearchText,
                SortBy = request.SortBy
            });

            // Generate the PDF file from the provided HTML template and the applicant data.
            fileMetaData = await _htmlReportBuilderService.GenerateApplicantsPdfFromHtml(applicantsResponse.Payload.Applicants.Items, baseUrl);

            // Update the report status to completed and notify the user.
            await UpdateStatusAndNotify(reportId, ReportStatus.Completed, userNameIdentifier, fileMetaData);
        }
        catch
        {
            // If an exception occurs, update the report status to failed and notify the user.
            await UpdateStatusAndNotify(reportId, ReportStatus.Failed, userNameIdentifier, fileMetaData);
        }

        // Return the FileMetaData object.
        return fileMetaData;
    }

    public async Task InitiateApplicantsReport(ExportApplicantsQuery request, Guid reportId, string userNameIdentifier, Guid? tenantId)
    {
        // Update the report status to pending and notify the user.
        await UpdateStatusAndNotify(reportId, ReportStatus.Pending, userNameIdentifier);

        // Set the tenant ID if it's not null.
        SetTenantIdIfNotNull(tenantId);

        // Set the initial status for the report.
        await SetInitialStatus(request, reportId);
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Set an initial pending status for a report based on the provided query and report.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="reportId"></param>
    /// <returns></returns>
    private async Task SetInitialStatus(ExportApplicantsQuery request, Guid reportId)
    {
        _dbContext.Reports.Add(new Report
        {
            Id = reportId,
            Title = $"N/A",
            QueryString = $"SearchText:{request.SearchText ?? "All"}, SortBy:{request.SortBy}",
            Status = (int)ReportStatus.Pending
        });

        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Set the tenant ID and tenant mode if a tenant ID is provided.
    /// </summary>
    /// <param name="tenantId"></param>
    private void SetTenantIdIfNotNull(Guid? tenantId)
    {
        if (tenantId is not null)
            _tenantResolver.SetTenantId(tenantId);
    }

    /// <summary>
    /// Update the status of a report and notify the issuer and viewers.
    /// </summary>
    /// <param name="reportId"></param>
    /// <param name="status"></param>
    /// <param name="userNameIdentifier"></param>
    /// <param name="fileMetaData"></param>
    /// <returns></returns>
    private async Task UpdateStatusAndNotify(Guid reportId,
                                             ReportStatus status,
                                             string userNameIdentifier,
                                             FileMetaData fileMetaData = null)
    {
        // Wait briefly to simulate processing time.
        //await Task.Delay(1000);

        // Retrieve the report from the database.
        var report = await _dbContext.Reports.Where(r => r.Id == reportId).FirstOrDefaultAsync();

        // If the report exists, update its status and associated file metadata.
        if (report != null)
        {
            report.Title = $"{_utcTimeService.GetUtcNow().ToLongDateString()} {fileMetaData?.FileName}";
            report.Status = (int)status;
            report.ContentType = fileMetaData?.ContentType;
            report.FileName = fileMetaData?.FileName;
            report.FileUri = fileMetaData?.FileUri;
            await _dbContext.SaveChangesAsync();
            // Notify the issuer of the updated status.
            await _hubNotificationService.NotifyReportIssuer(userNameIdentifier, fileMetaData, status);
        }

        await _hubNotificationService.RefreshReportsViewer(userNameIdentifier);
    }

    #endregion Private Methods
}