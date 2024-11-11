using Ardalis.GuardClauses;
using OttApiPlatform.Application.Common.Contracts.UseCases.Reports;
using OttApiPlatform.Application.Features.Reports.GetReportForEdit;
using OttApiPlatform.Application.Features.Reports.GetReports;

namespace OttApiPlatform.Application.UseCases.Reports;

public class ReportUseCase : IReportUseCase
{
    #region Private Fields

    private readonly IApplicationDbContext _dbContext;

    #endregion Private Fields

    #region Public Constructors

    public ReportUseCase(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    #endregion Public Constructors

    #region Public Methods

    public async Task<Envelope<GetReportForEditQuery>> GetReport(GetReportForEditQuery request)
    {
        if (!Guid.TryParse(request.Id, out var reportId))
            return Envelope<GetReportForEditQuery>.Result.BadRequest(Resource.Invalid_report_Id);

        var report = await _dbContext.Reports.Where(a => a.Id == reportId).FirstOrDefaultAsync();

        if (report == null)
            return Envelope<GetReportForEditQuery>.Result.NotFound(Resource.Unable_to_load_report);

        var reportForEdit = GetReportForEditQuery.MapFromEntity(report);

        return Envelope<GetReportForEditQuery>.Result.Ok(reportForEdit);
    }

    public async Task<Envelope<ReportsResponse>> GetReports(GetReportsQuery? request)
    {
        Guard.Against.Null(request, nameof(request));

        var query = _dbContext.Reports.Where(a => (a.Title.Contains(request.SearchText)
                                                  || a.FileName.Contains(request.SearchText)
                                                  || a.FileUri.Contains(request.SearchText)
                                                  || a.QueryString.Contains(request.SearchText)
                                                  || request.SearchText == null)
                                                  && (a.Status == (int)request.SelectedReportStatus || request.SelectedReportStatus == null || request.SelectedReportStatus == 0));

        query = !string.IsNullOrWhiteSpace(request.SortBy)
            ? query.SortBy(request.SortBy)
            : query.OrderByDescending(a => a.CreatedOn);

        var reportItems = await query.Select(q => ReportItem.MapFromEntity(q))
            .ToPagedListAsync(request.PageNumber, request.RowsPerPage);

        var reportsResponse = new ReportsResponse
        {
            Reports = reportItems
        };

        return Envelope<ReportsResponse>.Result.Ok(reportsResponse);
    }

    #endregion Public Methods
}
