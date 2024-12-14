namespace OttApiPlatform.Domain.Entities.ContentManagement;

[Table("AssetStorage")]
public class AssetStorageModel : BaseEntity, IMustHaveTenant
{
    public Guid TenantId { get; set; }
    public Guid AssetId { get; set; }
    public Guid StorageLocationId { get; set; }
    public int Order { get; set; } 

    #region Navigational Properties 

    public AssetModel Asset { get; set; }
    public StorageLocationModel StorageLocation { get; set; } 

    #endregion
}

