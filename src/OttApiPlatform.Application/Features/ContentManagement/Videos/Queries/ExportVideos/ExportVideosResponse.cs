namespace OttApiPlatform.Application.Features.ContentManagement.Videos.Queries.ExportVideos;

public class ExportVideosResponse
{
    #region Public Properties

    public string Id { get; set; }
    public string SuccessMessage { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public string FileUrl { get; set; }

    #endregion Public Properties
}
