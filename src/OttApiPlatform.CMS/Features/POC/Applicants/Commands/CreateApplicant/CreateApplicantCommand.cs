namespace OttApiPlatform.CMS.Features.POC.Applicants.Commands.CreateApplicant;

public class CreateApplicantCommand
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

    public List<ReferenceItemForAdd> ReferenceItems { get; set; }

    #endregion Public Properties
}