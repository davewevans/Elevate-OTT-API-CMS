namespace OttApiPlatform.Application.Features.Reports.GetReports;

public class GetReportsQuery : FilterableQuery, IRequest<Envelope<ReportsResponse>>
{
    #region Public Properties

    public ReportStatus? SelectedReportStatus { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class GetReportsQueryHandler : IRequestHandler<GetReportsQuery, Envelope<ReportsResponse>>
    {
        #region Private Fields

        private readonly IApplicationDbContext _dbContext;

        #endregion Private Fields

        #region Public Constructors

        public GetReportsQueryHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<ReportsResponse>> Handle(GetReportsQuery request, CancellationToken cancellationToken)
        {
            // Get the reports from the database.
            var query = _dbContext.Reports.AsQueryable();

            // Filter reports by search text if there is any.
            if (!string.IsNullOrWhiteSpace(request.SearchText))
                query = query.Where(q => q.Title.Contains(request.SearchText) ||
                                         q.FileName.Contains(request.SearchText) ||
                                         q.FileUri.Contains(request.SearchText) ||
                                         q.QueryString.Contains(request.SearchText));

            // Filter reports by selected status if there is any.
            query = query.Where(q => q.Status == (int)request.SelectedReportStatus ||
                                     request.SelectedReportStatus == null ||
                                     request.SelectedReportStatus == 0);

            // Sort reports by the specified field.
            query = !string.IsNullOrWhiteSpace(request.SortBy)
                ? query.SortBy(request.SortBy)
                : query.OrderByDescending(a => a.CreatedOn);

            // Get the report items as paged list.
            var reportItems = await query.Select(q => ReportItem.MapFromEntity(q))
                                         .AsNoTracking()
                                         .ToPagedListAsync(request.PageNumber, request.RowsPerPage);

            // Map the report items to response DTO.
            var reportsResponse = new ReportsResponse
            {
                Reports = reportItems
            };

            return Envelope<ReportsResponse>.Result.Ok(reportsResponse);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}