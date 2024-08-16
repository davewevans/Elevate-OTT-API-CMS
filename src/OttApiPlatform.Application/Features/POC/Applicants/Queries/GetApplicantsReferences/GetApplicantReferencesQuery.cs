namespace OttApiPlatform.Application.Features.POC.Applicants.Queries.GetApplicantsReferences;

public class GetApplicantReferencesQuery : FilterableQuery, IRequest<Envelope<ApplicantReferencesResponse>>
{
    #region Public Properties

    public Guid ApplicantId { get; set; }

    #endregion Public Properties

    public class GetApplicantReferencesQueryHandler : IRequestHandler<GetApplicantReferencesQuery, Envelope<ApplicantReferencesResponse>>
    {
        #region Private Fields

        private readonly IApplicationDbContext _dbContext;

        #endregion Private Fields

        #region Public Constructors

        public GetApplicantReferencesQueryHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion Public Constructors

        #region Public Methods

        #region Public Methods

        public async Task<Envelope<ApplicantReferencesResponse>> Handle(GetApplicantReferencesQuery request, CancellationToken cancellationToken)
        {
            // Start with a query that retrieves all references for the specified applicant from the database.
            var query = _dbContext.References.Where(a => a.ApplicantId == request.ApplicantId
                                                         && (a.Name.Contains(request.SearchText)
                                                             || a.JobTitle.Contains(request.SearchText)
                                                             || a.Phone.Contains(request.SearchText)
                                                             || string.IsNullOrEmpty(request.SearchText)));

            // If a sort by field is provided, sort the query by that field; otherwise, sort by name.
            query = !string.IsNullOrWhiteSpace(request.SortBy)
                ? query.SortBy(request.SortBy)
                : query.OrderBy(a => a.Name);

            // Convert the query to a paged list of applicant reference item DTOs.
            var applicantReferenceItems = await query.Select(q => ApplicantReferenceItem.MapFromEntity(q)).
                                                      ToPagedListAsync(request.PageNumber, request.RowsPerPage);

            // Create an applicant references response DTO with the list of applicant reference item DTOs.
            var applicantReferencesResponse = new ApplicantReferencesResponse
            {
                ApplicantReferences = applicantReferenceItems
            };

            // Return a success response with the applicant references response DTO as the payload.
            return Envelope<ApplicantReferencesResponse>.Result.Ok(applicantReferencesResponse);
        }

        #endregion Public Methods
    }

    #endregion Public Methods
}