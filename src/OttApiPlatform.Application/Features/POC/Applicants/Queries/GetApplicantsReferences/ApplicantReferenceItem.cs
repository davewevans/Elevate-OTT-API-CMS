namespace OttApiPlatform.Application.Features.POC.Applicants.Queries.GetApplicantsReferences;

public class ApplicantReferenceItem : AuditableDto
{
    #region Public Properties

    public string Id { get; set; }
    public string Name { get; set; }
    public string JobTitle { get; set; }
    public string Phone { get; set; }

    #endregion Public Properties

    #region Public Methods

    public static ApplicantReferenceItem MapFromEntity(Reference reference)
    {
        return new()
        {
            Id = reference.Id.ToString(),
            Name = reference.Name,
            JobTitle = reference.JobTitle,
            Phone = reference.Phone,
            CreatedOn = reference.CreatedOn,
            CreatedBy = reference.CreatedBy,
            ModifiedOn = reference.ModifiedOn,
            ModifiedBy = reference.ModifiedBy
        };
    }

    #endregion Public Methods
}