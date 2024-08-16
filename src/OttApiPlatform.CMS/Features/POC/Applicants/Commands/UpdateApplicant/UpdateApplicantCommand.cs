namespace OttApiPlatform.CMS.Features.POC.Applicants.Commands.UpdateApplicant;

public class UpdateApplicantCommand
{
    #region Public Constructors

    public UpdateApplicantCommand()
    {
        NewApplicantReferences = new List<ReferenceItemForAdd>();
        ModifiedApplicantReferences = new List<ReferenceItemForEdit>();
        RemovedApplicantReferences = new List<string>();
    }

    #endregion Public Constructors

    #region Public Properties

    public string Id { get; set; }
    public int Ssn { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public decimal? Height { get; set; }
    public decimal? Weight { get; set; }

    public decimal? Bmi
    {
        get => Height != 0 ? Weight / (Height / 100 * 2) : 0;
        set { if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value)); }
    }

    public List<ReferenceItemForAdd> NewApplicantReferences { get; set; }
    public List<ReferenceItemForEdit> ModifiedApplicantReferences { get; set; }
    public List<string> RemovedApplicantReferences { get; set; }

    #endregion Public Properties
}