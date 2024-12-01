namespace OttApiPlatform.Domain.Entities.ContentManagement;
public class SeasonModel : BaseEntity, IMustHaveTenant
{
    #region Public Properties

    public Guid TenantId { get; set; }

    public int SeasonNumber { get; set; }

    public string Name { get; set; }

    public string Description { get; set; } 

    #endregion

    #region Foreign Key Properties

    public Guid SeriesId { get; set; }
    public Guid? ImageAssetId { get; set; } 

    #endregion


    #region Navigational Properties

    public SeriesModel Series { get; set; }
    public AssetModel Image { get; set; }

    public ICollection<SeasonAssetModel> SeasonAssets { get; set; }

    #endregion
}
