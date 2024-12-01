namespace OttApiPlatform.Domain.Entities.ContentManagement;

[Table("Videos")]
public class VideoModel : BaseEntity, IMustHaveTenant
{
    #region Public Properties

    public Guid TenantId { get; set; }

    public string FileName { get; set; }

    public string Resolution { get; set; }

    public string VideoCodec { get; set; }

    public string VideoBitrate { get; set; }

    public string AudioBitrate { get; set; }

    public string Bitrate { get; set; }

    public string FrameRate { get; set; }

    public TimeSpan Duration { get; set; }

    public double FileSize { get; set; }

    public bool Encoded { get; set; }
    
    public Guid AssetId { get; set; }

    #endregion

    #region Navigational Properties

    public AssetModel Asset { get; set; }

    #endregion
}
