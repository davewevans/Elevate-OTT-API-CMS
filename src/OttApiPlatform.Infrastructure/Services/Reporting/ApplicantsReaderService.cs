namespace OttApiPlatform.Infrastructure.Services.Reporting;

public class ApplicantsReaderService : IApplicantsReaderService
{
    #region Private Fields

    private readonly IApplicationDbContext _applicationDbContext;

    #endregion Private Fields

    #region Public Constructors

    public ApplicantsReaderService(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    #endregion Public Constructors

    #region Public Methods

    public async Task<Envelope<ApplicantsResponse>> GetApplicants(GetApplicantsQuery request)
    {
        // Start with a query that retrieves all applicants and their references from the database.
        var query = _applicationDbContext.Applicants.Include(a => a.References).AsQueryable();

        // If a search text is provided, filter the applicants by their first name or last name.
        if (!string.IsNullOrWhiteSpace(request.SearchText))
            query = query.Where(a => a.FirstName.Contains(request.SearchText) || a.LastName.Contains(request.SearchText) || a.Ssn.ToString().Contains(request.SearchText));

        // If a sort by field is provided, sort the query by that field; otherwise, sort by first
        // name and then last name.
        query = !string.IsNullOrWhiteSpace(request.SortBy)
            ? query.SortBy(request.SortBy)
            : query.OrderBy(a => a.FirstName).ThenBy(a => a.LastName);

        // Convert the query to a paged list of applicant item DTOs.
        var applicantItems = await query.Select(q => ApplicantItem.MapFromEntity(q, true))
                                        .AsNoTracking()
                                        .ToPagedListAsync(request.PageNumber, request.RowsPerPage);

        // Create an applicants response DTO with the list of applicant item DTOs.
        var applicantsResponse = new ApplicantsResponse
        {
            Applicants = applicantItems
        };

        // Return a success response with the applicants response DTO as the payload.
        return Envelope<ApplicantsResponse>.Result.Ok(applicantsResponse);
    }

    #endregion Public Methods
}