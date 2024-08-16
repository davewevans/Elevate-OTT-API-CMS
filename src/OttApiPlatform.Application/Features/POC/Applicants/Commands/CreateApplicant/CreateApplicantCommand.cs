namespace OttApiPlatform.Application.Features.POC.Applicants.Commands.CreateApplicant;

public class CreateApplicantCommand : IRequest<Envelope<CreateApplicantResponse>>
{
    #region Public Constructors

    public CreateApplicantCommand()
    {
        ReferenceItems = new List<ReferenceItemForAdd>();
    }

    #endregion Public Constructors

    #region Public Properties

    public int? Ssn { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public decimal? Height { get; set; }
    public decimal? Weight { get; set; }

    public decimal? Bmi
    {
        get => Height != 0 ? Weight / (Height / 100 * 2) : 0;
        set { }
    }

    public IReadOnlyList<ReferenceItemForAdd> ReferenceItems { get; set; }

    #endregion Public Properties

    #region Public Methods

    public Applicant MapToEntity()
    {
        return new()
        {
            Ssn = Ssn ?? throw new ArgumentNullException(nameof(Ssn)),
            FirstName = FirstName,
            LastName = LastName,
            DateOfBirth = DateOfBirth,
            Height = Height ?? throw new ArgumentNullException(nameof(Height)),
            Weight = Weight ?? throw new ArgumentNullException(nameof(Weight)),
            References = ReferenceItems.Select(ri => new Reference
            {
                Name = ri.Name,
                JobTitle = ri.JobTitle,
                Phone = ri.Phone,
            }).ToList()
        };
    }

    #endregion Public Methods

    #region Public Classes

    public class CreateApplicantCommandHandler : IRequestHandler<CreateApplicantCommand, Envelope<CreateApplicantResponse>>
    {
        #region Private Fields

        private readonly IApplicationDbContext _dbContext;

        #endregion Private Fields

        #region Public Constructors

        public CreateApplicantCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<CreateApplicantResponse>> Handle(CreateApplicantCommand request, CancellationToken cancellationToken)
        {
            // Map the request to an entity.
            var applicant = request.MapToEntity();

            // Add the applicant to the database context.
            await _dbContext.Applicants.AddAsync(applicant, cancellationToken);

            // Save changes to the database.
            await _dbContext.SaveChangesAsync(cancellationToken);

            // Create a response with the applicant ID and a success message.
            var createApplicantResponse = new CreateApplicantResponse
            {
                Id = applicant.Id.ToString(),
                SuccessMessage = Resource.Applicant_has_been_created_successfully
            };

            // Return a result envelope with the response as the payload.
            return Envelope<CreateApplicantResponse>.Result.Ok(createApplicantResponse);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}