namespace OttApiPlatform.Domain.Entities.ContentManagement;

[Table("StorageLocations")]
public class StorageLocation : BaseEntity, IMustHaveTenant
{
    #region Public Properties

    public Guid TenantId { get; set; }
    public string Name { get; set; }
    public string Details { get; set; } 

    #endregion

    #region Navigational Properties 

    public ICollection<AssetStorageModel> AssetStorages { get; set; } 

    #endregion
}
