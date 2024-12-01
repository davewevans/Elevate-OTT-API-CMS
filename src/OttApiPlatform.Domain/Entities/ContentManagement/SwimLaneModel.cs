using System.ComponentModel.DataAnnotations;

namespace OttApiPlatform.Domain.Entities.ContentManagement;

[Table("SwimLanes")]
public class SwimLaneModel : BaseEntity, IMustHaveTenant
{
    public Guid TenantId { get; set; }

    [Required]
    [MaxLength(300, ErrorMessage = "Maximum length for the Title is {0} characters.")]
    public string Title { get; set; }

    [MaxLength(500, ErrorMessage = "Maximum length for the Description is {0} characters.")]
    public string Description { get; set; }

    public ContentType ContentType { get; set; }

    public SwimLaneType SwimLaneType { get; set; }

    public SwimLaneCriteria SwimLaneCriteria { get; set; }

    public int ContentLimit { get; set; }

    #region Navigational Properties 

    public ICollection<ContentCategoryModel> ContentCategories { get; set; }

    public ICollection<SwimLaneContentModel> SwimLaneContent { get; set; } 

    #endregion
}
