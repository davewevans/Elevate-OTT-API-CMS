namespace OttApiPlatform.CMS.Features.Reports.GetReportForEdit;

public class ReportForEdit : AuditableDto
{
    #region Public Properties

    public string ContentType { get; set; }
    public string FileName { get; set; }
    public string FileUri { get; set; }
    public string Id { get; set; }
    public string QueryString { get; set; }
    public ReportStatus Status { get; set; }
    public string Title { get; set; }

    #endregion Public Properties
}