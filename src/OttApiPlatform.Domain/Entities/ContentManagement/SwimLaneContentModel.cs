namespace OttApiPlatform.Domain.Entities.ContentManagement;

[Table("SwimLaneContent")]
public class SwimLaneContentModel : BaseEntity, IMustHaveTenant
{
    #region Public Properties

    public Guid TenantId { get; set; }

    public Guid ContentId { get; set; }

    public Guid SwimLaneId { get; set; }

    public int Order { get; set; } 

    #endregion

    #region Navigational Properties 

    public ContentModel Content { get; set; }

    public CategoryModel SwimLane { get; set; } 

    #endregion
}