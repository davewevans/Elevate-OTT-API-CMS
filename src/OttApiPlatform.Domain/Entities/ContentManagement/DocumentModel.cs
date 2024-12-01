namespace OttApiPlatform.Domain.Entities.ContentManagement;

[Table("Documents")]
public class DocumentModel : BaseEntity, IMustHaveTenant
{
    #region Public Properties

    public Guid TenantId { get; set; }

    public string FileName { get; set; }

    public string ContentType { get; set; }

    public double FileSize { get; set; }

    #endregion

    #region Foreign Key Properties

    public Guid AssetId { get; set; } 

    #endregion

    #region Navigational Properties

    public AssetModel Asset { get; set; }

    #endregion
}
