namespace OttApiPlatform.Application.Features.POC.Applicants.Queries.ExportApplicants;

public class ExportApplicantsResponse
{
    #region Public Properties

    public string Id { get; set; }
    public string SuccessMessage { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public string FileUrl { get; set; }

    #endregion Public Properties
}