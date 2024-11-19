using OttApiPlatform.Domain.Enums.Content;

namespace OttApiPlatform.Domain.Entities.Content;

[Table("Videos")]
public class VideoModel : BaseAsset, IMustHaveTenant
{
    // TODO filters
    // TODO Geo-blocking

    // TODO subscription

    public VideoModel()
    {
        VideosCategories = new List<VideoCategoryModel>();
    }

    public Guid TenantId { get; set; }

    public bool Mp4Support { get; set; }

    public Guid? TrailerVideoId { get; set; }
    public Guid? FeaturedCategoryVideoId { get; set; }


    public bool HasOneTimePurchasePrice { get; set; }

    
    public double OneTimePurchasePrice { get; set; }
    public bool HasRentalPrice { get; set; }
    public RentalDuration RentalDuration { get; set; }

    
    public double RentalPrice { get; set; }


    #region foreign keys

    [ForeignKey(nameof(PersonModel))]
    public Guid? AuthorId { get; set; }

    #endregion

    #region Navigational Properties
    public ICollection<VideoCategoryModel> VideosCategories { get; set; } 

    //public ICollection<VideoCollectionModel>? VideosCollections { get; set; }
    //public ICollection<VideoTagModel>? VideosTags { get; set; }

    public PersonModel? Author { get; set; }

    //public ICollection<ExtraModel>? Extras { get; set; }
    //public ICollection<CommentModel>? Comments { get; set; }
    //public ICollection<SubtitleModel>? Subtitles { get; set; }
    #endregion
}
