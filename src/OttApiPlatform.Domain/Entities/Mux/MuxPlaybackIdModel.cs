namespace OttApiPlatform.Domain.Entities.Mux;

[Table("MuxPlaybackIds")]
public class MuxPlaybackIdModel : BaseEntity, IMustHaveTenant
{
    public Guid TenantId { get; set; }

    public string Policy { get; set; } = string.Empty;

    public string PlaybackId { get; set; } = string.Empty;

    #region Foreign Key Properties

    public Guid MuxAssetId { get; set; } 

    #endregion

    #region Navigational Properties 

    public MuxAssetModel MuxAsset { get; set; } 

    #endregion
}
