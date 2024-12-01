namespace OttApiPlatform.Domain.Entities.ContentManagement;

[Table("Images")]
public class ImageModel : BaseEntity, IMustHaveTenant
{
    #region Public Properties

    public Guid TenantId { get; set; }

    // The original file name
    public string FileName { get; set; } 

    public AssetImageType Type { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }

    public double ImageSize { get; set; }

    #endregion

    #region Foreign Key Properties

    public Guid AssetId { get; set; } 

    #endregion

    #region Navigational Properties

    public AssetModel Asset { get; set; }

    #endregion
}
