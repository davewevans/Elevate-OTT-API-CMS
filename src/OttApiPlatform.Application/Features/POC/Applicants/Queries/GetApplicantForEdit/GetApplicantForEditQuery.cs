namespace OttApiPlatform.Application.Features.POC.Applicants.Queries.GetApplicantForEdit;

public class GetApplicantForEditQuery : IRequest<Envelope<ApplicantForEditResponse>>
{
    #region Public Properties

    public string Id { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class GetApplicantForEditQueryHandler : IRequestHandler<GetApplicantForEditQuery, Envelope<ApplicantForEditResponse>>
    {
        #region Private Fields

        private readonly IApplicationDbContext _dbContext;

        #endregion Private Fields

        #region Public Constructors

        public GetApplicantForEditQueryHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<ApplicantForEditResponse>> Handle(GetApplicantForEditQuery request, CancellationToken cancellationToken)
        {
            // Check if the ID provided in the request is a valid GUID.
            if (!Guid.TryParse(request.Id, out var applicantId))
                return Envelope<ApplicantForEditResponse>.Result.BadRequest(Resource.Invalid_applicant_Id);

            // Retrieve the applicant from the database using the ID.
            var applicant = await _dbContext.Applicants.Where(a => a.Id == applicantId).FirstOrDefaultAsync(cancellationToken: cancellationToken);

            // If the applicant is not found, return a not found response.
            if (applicant == null)
                return Envelope<ApplicantForEditResponse>.Result.NotFound(Resource.Unable_to_load_applicant);

            // Map the applicant entity to an applicant response DTO.
            var applicantForEditResponse = ApplicantForEditResponse.MapFromEntity(applicant);

            // Return a success response with the applicant response DTO as the payload.
            return Envelope<ApplicantForEditResponse>.Result.Ok(applicantForEditResponse);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}