namespace OttApiPlatform.CMS.Features.POC.Applicants.Queries.GetApplicantsReferences;

public class ApplicantReferenceItem : AuditableDto
{
    #region Public Properties

    public string Id { get; set; }
    public string Name { get; set; }
    public string JobTitle { get; set; }
    public string Phone { get; set; }
    public new DateTime? CreatedOn { get; set; }
    public bool IsAddedOrModified { get; set; }

    #endregion Public Properties
}