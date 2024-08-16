namespace OttApiPlatform.Application.Features.Reports.GetReportForEdit;

public class ReportForEditResponse : AuditableDto
{
    #region Public Properties

    public string Id { get; set; }
    public string Title { get; set; }
    public string QueryString { get; set; }
    public string FileName { get; set; }
    public string FileUri { get; set; }
    public string ContentType { get; set; }
    public int Status { get; set; }

    #endregion Public Properties

    #region Public Methods

    public static ReportForEditResponse MapFromEntity(Report report)
    {
        return new()
        {
            Id = report.Id.ToString(),
            Title = report.Title,
            QueryString = report.QueryString,
            FileName = report.FileName,
            FileUri = report.FileUri,
            ContentType = report.ContentType,
            Status = report.Status,
        };
    }

    #endregion Public Methods
}