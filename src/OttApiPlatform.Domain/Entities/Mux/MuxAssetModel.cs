namespace OttApiPlatform.Domain.Entities.Mux;

[Table("MuxAssets")]
public class MuxAssetModel : BaseEntity, IMustHaveTenant
{
    #region Public Properties

    public Guid TenantId { get; set; }
    public string MuxAssetId { get; set; } // Asset ID provided by the service
    public string Status { get; set; }
    public string VideoQuality { get; set; }
    public string MP4Support { get; set; }
    public string MasterAccess { get; set; }
    public string EncodingTier { get; set; }
    public string ResolutionTier { get; set; }
    public string MaxResolutionTier { get; set; }
    public double MaxStoredFrameRate { get; set; }
    public double Duration { get; set; }
    public string IngestType { get; set; }
    public bool IsTestAsset { get; set; }
    public string CreatedAt { get; set; }

    #endregion


    #region Navigational Properties 

    public ICollection<MuxPlaybackIdModel> PlaybackIds { get; set; } 
    public ICollection<MuxAssetTrackModel> AssetTracks { get; set; }

    #endregion
}