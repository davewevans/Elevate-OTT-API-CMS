namespace OttApiPlatform.Application.Features.POC.Applicants.Queries.GetApplicants;

public class ApplicantsResponse
{
    #region Public Properties

    public PagedList<ApplicantItem> Applicants { get; set; }

    #endregion Public Properties
}