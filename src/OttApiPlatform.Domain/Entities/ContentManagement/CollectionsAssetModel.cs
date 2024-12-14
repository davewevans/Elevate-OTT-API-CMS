namespace OttApiPlatform.Domain.Entities.ContentManagement;

[Table("CollectionsAssets")]
public class CollectionsAssetModel : BaseEntity, IMustHaveTenant
{
    #region Public Properties

    public Guid TenantId { get; set; }

    public Guid AssetId { get; set; }

    public Guid CollectionId { get; set; }

    public int Order { get; set; } 

    #endregion

    #region Navigational Properties 

    public AssetModel Asset { get; set; }

    public CollectionModel Collection { get; set; } 

    #endregion
}
