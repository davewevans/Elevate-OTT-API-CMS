namespace OttApiPlatform.Domain.Entities.Content;

[Table("CategoryVideoPositions")]
public class CategoryVideoPositionModel : BaseEntity, IMustHaveTenant
{
    public Guid TenantId { get; set; }

    public Guid? CategoryId { get; set; }
    public Guid? VideoId { get; set; }
    public int Position { get; set; }

    #region Navigational Properties
    public CategoryModel? Category { get; set; }
    public VideoModel? Video { get; set; }
    #endregion

}
