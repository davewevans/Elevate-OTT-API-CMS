namespace OttApiPlatform.Domain.Entities.ContentManagement;

[Table("PlaylistAssets")]
public class PlaylistAssetModel : BaseEntity, IMustHaveTenant
{
    #region Public Properties

    public Guid TenantId { get; set; }

    public Guid AssetId { get; set; }

    public Guid PlaylistId { get; set; }

    public int Order { get; set; } 

    #endregion

    #region Navigational Properties 

    public AssetModel Asset { get; set; }

    public PlaylistModel Playlist { get; set; } 

    #endregion
}
