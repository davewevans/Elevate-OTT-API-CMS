namespace OttApiPlatform.Domain.Entities.ContentManagement;

[Table("Languages")]
public class LanguageModel : BaseEntity
{
    #region Public Properties

    public string Code { get; set; }
    public string RegionCode { get; set; }
    public string Name { get; set; } 

    #endregion

    #region Navigational Properties 

    public ICollection<AssetModel> Assets { get; set; } 

    #endregion
}
