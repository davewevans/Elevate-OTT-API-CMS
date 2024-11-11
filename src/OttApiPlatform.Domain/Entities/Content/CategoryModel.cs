namespace OttApiPlatform.Domain.Entities.Content;

[Table("Categories")]
public class CategoryModel : BaseEntity, IMustHaveTenant
{

    public CategoryModel()
    {
        VideosCategories = new List<VideoCategoryModel>();
    }
    public Guid TenantId { get; set; }

    public string? Title { get; set; } 

    public string? Description { get; set; } 

    public int Position { get; set; } 

    public string? ImageUrl { get; set; }

    public string? SeoTitle { get; set; }

    public string? SeoDescription { get; set; }

    public string? Slug { get; set; }


    #region Navigational Properties

    public ICollection<VideoCategoryModel> VideosCategories { get; set; } 
    //public ICollection<CategoryCollectionModel>? CategoriesCollections { get; set; }
    //public ICollection<LiveStreamCategoryModel>? LiveStreamsCategories { get; set; }
    //public ICollection<PodcastCategoryModel>? PodcastsCategoriess { get; set; }

    #endregion
}
