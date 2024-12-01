namespace OttApiPlatform.Domain.Entities.ContentManagement;

[Table("SeriesAssets")]
public class SeriesAssetModel : BaseEntity, IMustHaveTenant
{
    #region Public Properties

    public Guid TenantId { get; set; }

    public Guid AssetId { get; set; }

    public Guid SeriesId { get; set; }

    public int Order { get; set; } 

    #endregion

    #region Navigational Properties 

    public AssetModel Asset { get; set; }

    public SeriesModel Series { get; set; } 

    #endregion
}
