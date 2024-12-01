using System.ComponentModel.DataAnnotations;
using OttApiPlatform.Domain.Entities.ContentAccessManagement;
using OttApiPlatform.Domain.Entities.TransactionManagement;

namespace OttApiPlatform.Domain.Entities.ContentManagement;

[Table("Asset")]
public class ContentModel : BaseEntity, IMustHaveTenant
{
    #region Public Properties

    public Guid TenantId { get; set; }

    [Required]
    [MaxLength(300, ErrorMessage = "Maximum length for the Title is {0} characters.")]
    public string Title { get; set; }

    [MaxLength(50, ErrorMessage = "Maximum length for the Short Description is {0} characters.")]
    public string ShortDescription { get; set; }

    [MaxLength(2000, ErrorMessage = "Maximum length for the Full Description is {0} characters.")]
    public string FullDescription { get; set; }

    [MaxLength(50, ErrorMessage = "Maximum length for the Seo Title is {0} characters.")]
    public string SeoTitle { get; set; }

    [MaxLength(500, ErrorMessage = "Maximum length for the Seo Description is {0} characters.")]
    public string SeoDescription { get; set; }

    [MaxLength(50, ErrorMessage = "Maximum length for the Slug is {0} characters.")]
    public string Slug { get; set; }

    public ContentType Type { get; set; }

    public ContentStatus Status { get; set; }

    public bool AllowDownload { get; set; }

    public string Language { get; set; }

    #endregion

    #region Foreign Key Properties

    public Guid? PrimaryMediaId { get; set; }
    public Guid? PreviewMediaId { get; set; }
    public Guid? BannerWebsiteId { get; set; }
    public Guid? BannerMobileId { get; set; }
    public Guid? BannerTvAppsId { get; set; }
    public Guid? PosterWebId { get; set; }
    public Guid? PosterMobileId { get; set; }
    public Guid? PosterTvAppsId { get; set; } 

    #endregion


    #region Navigational Properties

    public AssetModel PrimaryMedia { get; set; }

    public AssetModel PreviewMedia { get; set; }

    public AssetModel BannerWebsite { get; set; } // 1600X560

    public AssetModel BannerMobile { get; set; } // 1600X900

    public AssetModel BannerTvApps { get; set; } // 1920X1080

    public AssetModel PosterWeb { get; set; } // 288X424

    public AssetModel PosterMobile { get; set; } // 200X300

    public AssetModel PosterTvApps { get; set; } // 450X600

    public SeriesModel Series { get; set; }

    public ICollection<ContentCategoryModel> ContentCategories { get; set; }

    public ICollection<ContentTagModel> ContentTags { get; set; }

    public ICollection<ContentPersonModel> ContentPeople { get; set; }

    public ICollection<SubtitleModel> Subtitles { get; set; }

    //public ICollection<TransactionModel> Transactions { get; set; }

    //public ICollection<RentalContentModel> RentalContents { get; set; }

    public ICollection<SwimLaneContentModel> SwimLaneContent { get; set; }

    #endregion
}
