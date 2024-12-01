using System.ComponentModel.DataAnnotations;

namespace OttApiPlatform.Domain.Entities.ContentManagement;

[Table("Categories")]
public class CategoryModel : BaseEntity, IMustHaveTenant
{
    #region Public Properties

    public Guid TenantId { get; set; }

    [Required]
    [MaxLength(300, ErrorMessage = "Maximum length for the Title is {0} characters.")]
    public string Title { get; set; }

    [MaxLength(500, ErrorMessage = "Maximum length for the Description is {0} characters.")]
    public string Description { get; set; }

    public int Order { get; set; }

    [MaxLength(50, ErrorMessage = "Maximum length for the Seo Title is {0} characters.")]
    public string SeoTitle { get; set; }

    [MaxLength(500, ErrorMessage = "Maximum length for the Seo Description is {0} characters.")]
    public string SeoDescription { get; set; }

    [MaxLength(50, ErrorMessage = "Maximum length for the Slug is {0} characters.")]
    public string Slug { get; set; }

    #endregion

    #region Foreign Key Properties

    public Guid? ImageAssetId { get; set; } 

    #endregion


    #region Navigational Properties

    public AssetModel Image { get; set; }

    public ICollection<ContentCategoryModel> ContentCategories { get; set; } = new List<ContentCategoryModel>();

    public ICollection<SwimLaneContentModel> SwimLaneContent { get; set; }

    #endregion
}
