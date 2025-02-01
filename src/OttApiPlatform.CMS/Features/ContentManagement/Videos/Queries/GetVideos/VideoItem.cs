namespace OttApiPlatform.CMS.Features.ContentManagement.Videos.Queries.GetVideos;

public class VideoItem : AuditableDto
{
    #region Public Properties

    public Guid Id { get; set; }
    public string PublicUrl { get; set; }
    public string SignedUrl { get; set; }
    public string FileName { get; set; }
    public string Language { get; set; }
    public string LanguageCode { get; set; }
    public string LanguageRegionCode { get; set; }
    public string DownloadUrl { get; set; }
    public string ThumbnailUrl { get; set; }
    public string TimeLineHoverUrl { get; set; }
    public string AnimatedGifUrl { get; set; }
    public bool AllowDownload { get; set; }
    public bool ClosedCaptions { get; set; }
    public TimeSpan? Duration { get; set; }
    public AssetCreationStatus StreamCreationStatus { get; set; }
    public StaticRenditionStatus StaticRenditionStatus { get; set; }

    #endregion Public Properties
}
