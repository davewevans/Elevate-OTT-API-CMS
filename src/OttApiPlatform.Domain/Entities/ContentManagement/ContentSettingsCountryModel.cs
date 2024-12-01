namespace OttApiPlatform.Domain.Entities.ContentManagement;

[Table("ContentSettingsCountries")]
public class ContentSettingsCountryModel : BaseEntity, IMustHaveTenant
{
    public Guid TenantId { get; set; }

    public Guid ContentSettingsId { get; set; }

    public Guid CountryId { get; set; }


    #region Navigational Properties

    public ContentSettingsModel ContentSettings { get; set; }
    public CountryModel Country { get; set; } 

    #endregion
}
