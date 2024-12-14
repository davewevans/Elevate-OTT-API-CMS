using System.ComponentModel.DataAnnotations;

namespace OttApiPlatform.Domain.Entities.ContentManagement;

[Table("Collections")]
public class CollectionModel : BaseEntity, IMustHaveTenant
{
    #region Public Properties

    public Guid TenantId { get; set; }

    [Required]
    [MaxLength(300, ErrorMessage = "Maximum length for the Title is {0} characters.")]
    public string Title { get; set; }

    [MaxLength(500, ErrorMessage = "Maximum length for the Description is {0} characters.")]
    public string Description { get; set; }

    [MaxLength(50, ErrorMessage = "Maximum length for the Seo Title is {0} characters.")]
    public string SeoTitle { get; set; }

    [MaxLength(500, ErrorMessage = "Maximum length for the Seo Description is {0} characters.")]
    public string SeoDescription { get; set; }

    [MaxLength(50, ErrorMessage = "Maximum length for the Slug is {0} characters.")]
    public string Slug { get; set; }

    public ContentType Type { get; set; }

    public ContentStatus Status { get; set; }

    public string Permalink { get; set; }

    public string Language { get; set; }

    public string Rating { get; set; } 

    #endregion

    #region Foreign Key Properties
    public Guid? ImageAssetId { get; set; }
    public Guid? BannerWebsiteId { get; set; }
    public Guid? BannerMobileId { get; set; }
    public Guid? BannerTvAppsId { get; set; }
    public Guid? PosterWebId { get; set; }
    public Guid? PosterMobileId { get; set; }
    public Guid? PosterTvAppsId { get; set; }

    #endregion

    #region Navigational Properties

    public AssetModel Image { get; set; }

    public AssetModel BannerWebsite { get; set; } // 1600X560

    public AssetModel BannerMobile { get; set; } // 1600X900

    public AssetModel BannerTvApps { get; set; } // 1920X1080

    public AssetModel PosterWeb { get; set; } // 288X424

    public AssetModel PosterMobile { get; set; } // 200X300

    public AssetModel PosterTvApps { get; set; } // 450X600

    public ICollection<CollectionsAssetModel> CollectionAssets { get; set; }

    #endregion

}
