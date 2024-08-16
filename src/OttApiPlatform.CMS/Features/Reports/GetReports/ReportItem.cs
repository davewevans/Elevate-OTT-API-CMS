namespace OttApiPlatform.CMS.Features.Reports.GetReports;

public class ReportItem : AuditableDto
{
    #region Public Properties

    public string Id { get; set; }
    public string Title { get; set; }
    public string ContentType { get; set; }
    public string FileName { get; set; }
    public string FileUri { get; set; }
    public string QueryString { get; set; }
    public ReportStatus Status { get; set; }

    #endregion Public Properties
}