namespace OttApiPlatform.Application.Features.POC.Applicants.Queries.GetApplicantForEdit;

public class ApplicantForEditResponse : AuditableDto
{
    #region Public Properties

    public string Id { get; set; }
    public int Ssn { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public decimal Height { get; set; }
    public decimal Weight { get; set; }

    public decimal Bmi
    {
        get => Height != 0 ? Weight / (Height / 100 * 2) : 0;
        set { if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value)); }
    }

    #endregion Public Properties

    #region Public Methods

    public static ApplicantForEditResponse MapFromEntity(Applicant applicant)
    {
        return new()
        {
            Id = applicant.Id.ToString(),
            Ssn = applicant.Ssn,
            FirstName = applicant.FirstName,
            LastName = applicant.LastName,
            DateOfBirth = applicant.DateOfBirth,
            Height = applicant.Height,
            Weight = applicant.Weight,
        };
    }

    #endregion Public Methods
}