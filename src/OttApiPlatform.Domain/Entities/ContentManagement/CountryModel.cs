namespace OttApiPlatform.Domain.Entities.ContentManagement;

[Table("Countries")]
public class CountryModel : BaseEntity
{
    #region Public Properties

    public string Name { get; set; }
    public string ISO2 { get; set; }
    public string ISO3 { get; set; }
    public int NumericCode { get; set; }
    public string DialCode { get; set; }
    public string Region { get; set; }
    public string SubRegion { get; set; }
    public bool IsActive { get; set; }

    #endregion

    #region Foreign Key Properties

    public Guid? FlagAssetId { get; set; } 

    #endregion

    #region Navigational Properties

    public AssetModel FlagAsset { get; set; }

    public ICollection<ContentSettingsCountryModel> RestrictedCountries { get; set; }

    #endregion
}
