namespace OttApiPlatform.Application.Features.Reports.GetReportForEdit;

public class GetReportForEditQuery : IRequest<Envelope<ReportForEditResponse>>
{
    #region Public Properties

    public string Id { get; set; }

    #endregion Public Properties

    public class GetReportForEditQueryHandler : IRequestHandler<GetReportForEditQuery, Envelope<ReportForEditResponse>>
    {
        #region Private Fields

        private readonly IApplicationDbContext _dbContext;

        #endregion Private Fields

        #region Public Constructors

        public GetReportForEditQueryHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion Public Constructors

        #region Public Methods

        #region Public Methods

        public async Task<Envelope<ReportForEditResponse>> Handle(GetReportForEditQuery request, CancellationToken cancellationToken)
        {
            // Check if the request id is valid.
            if (!Guid.TryParse(request.Id, out var reportId))
                return Envelope<ReportForEditResponse>.Result.BadRequest(Resource.Invalid_report_Id);

            // Find the report by its id.
            var report = await _dbContext.Reports.Where(a => a.Id == reportId).FirstOrDefaultAsync(cancellationToken: cancellationToken);

            // If the report is not found, return a not found error response.
            if (report == null)
                return Envelope<ReportForEditResponse>.Result.NotFound(Resource.Unable_to_load_report);

            // Map the report entity to response DTO.
            var reportForEditResponse = ReportForEditResponse.MapFromEntity(report);

            return Envelope<ReportForEditResponse>.Result.Ok(reportForEditResponse);
        }

        #endregion Public Methods
    }

    #endregion Public Methods
}