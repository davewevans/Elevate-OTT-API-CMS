namespace OttApiPlatform.Domain.Entities.ContentManagement;

[Table("ContentTags")]
public class ContentTagModel : BaseEntity, IMustHaveTenant
{
    #region Public Properties

    public Guid TenantId { get; set; }

    public Guid ContentId { get; set; }

    public Guid TagId { get; set; }

    public int Order { get; set; } 

    #endregion

    #region Public Properties

    public ContentModel Content { get; set; }

    public TagModel Tag { get; set; } 

    #endregion
}
