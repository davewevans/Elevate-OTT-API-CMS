namespace OttApiPlatform.Application.Features.ContentManagement.Videos.Queries.GetVideos;

public class VideosResponse
{
    #region Public Properties
    public PagedList<VideoItem>? Videos { get; set; }
    #endregion Public Properties
}
