namespace OttApiPlatform.Domain.Entities.ContentManagement;

[Table("ContentCategories")]
public class ContentCategoryModel : BaseEntity, IMustHaveTenant
{
    public Guid TenantId { get; set; }

    public Guid ContentId { get; set; }

    public Guid CategoryId { get; set; }

    public int Order { get; set; }

    #region Navigational Properties 

    public ContentModel Content { get; set; }

    public CategoryModel Category { get; set; } 

    #endregion
}
