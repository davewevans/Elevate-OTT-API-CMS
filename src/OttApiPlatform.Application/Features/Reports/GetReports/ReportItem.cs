namespace OttApiPlatform.Application.Features.Reports.GetReports;

public class ReportItem : AuditableDto
{
    #region Public Properties

    public string Id { get; set; }
    public string ContentType { get; set; }
    public string FileName { get; set; }
    public string FileUri { get; set; }
    public string QueryString { get; set; }
    public int Status { get; set; }
    public string Title { get; set; }

    #endregion Public Properties

    #region Public Methods

    public static ReportItem MapFromEntity(Report report)
    {
        return new()
        {
            Id = report.Id.ToString(),
            Title = report.Title ?? Resource.N_A,
            QueryString = report.QueryString,
            FileName = report.FileName?.Replace(".html", "") ?? Resource.N_A,
            FileUri = report.FileUri,
            ContentType = report.ContentType ?? Resource.N_A,
            Status = report.Status,
            CreatedOn = report.CreatedOn,
            CreatedBy = report.CreatedBy,
            ModifiedOn = report.ModifiedOn,
            ModifiedBy = report.ModifiedBy
        };
    }

    #endregion Public Methods
}