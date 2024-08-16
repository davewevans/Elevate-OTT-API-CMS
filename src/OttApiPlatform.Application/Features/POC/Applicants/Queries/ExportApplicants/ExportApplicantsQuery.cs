namespace OttApiPlatform.Application.Features.POC.Applicants.Queries.ExportApplicants
{
    public class ExportApplicantsQuery : IRequest<Envelope<ExportApplicantsResponse>>
    {
        #region Public Properties

        public string SearchText { get; set; }
        public string SortBy { get; set; }

        #endregion Public Properties

        #region Public Classes

        public class ExportApplicantsHandler : IRequestHandler<ExportApplicantsQuery, Envelope<ExportApplicantsResponse>>
        {
            #region Private Fields

            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IApplicantsReaderService _applicantsReaderService;
            private readonly IOnDemandReportingService _onDemandReportingService;

            #endregion Private Fields

            #region Public Constructors

            public ExportApplicantsHandler(IApplicantsReaderService applicantsReaderService,
                                           IOnDemandReportingService onDemandReportingService,
                                           IHttpContextAccessor httpContextAccessor)
            {
                _httpContextAccessor = httpContextAccessor;
                _applicantsReaderService = applicantsReaderService;
                _onDemandReportingService = onDemandReportingService;
            }

            #endregion Public Constructors

            #region Public Methods

            public async Task<Envelope<ExportApplicantsResponse>> Handle(ExportApplicantsQuery request, CancellationToken cancellationToken)
            {
                // Get the list of applicants based on the search criteria and sorting order provided in the query.
                var applicantResponse = await _applicantsReaderService.GetApplicants(new GetApplicantsQuery
                {
                    SearchText = request.SearchText,
                    SortBy = request.SortBy
                });

                // Get the name of the current user who requested the export.
                var currentUserName = _httpContextAccessor.GetUserName();

                // Get the issuer name by splitting the current user name by '@' and taking the
                // first part.
                var issuerName = currentUserName.Split("@")[0];

                // Get the base URL of the application from the HTTP context accessor.
                var baseUrl = $"{_httpContextAccessor.HttpContext?.Request.Scheme}://{_httpContextAccessor.HttpContext?.Request.Host}";

                // Export the list of applicants as a PDF file using the reporting service.
                var payload = await _onDemandReportingService.ExportApplicantToPdfOnDemand(applicantResponse.Payload, issuerName, baseUrl);

                // Return the PDF file as a response.
                return Envelope<ExportApplicantsResponse>.Result.Ok(payload);
            }

            #endregion Public Methods
        }

        #endregion Public Classes
    }
}