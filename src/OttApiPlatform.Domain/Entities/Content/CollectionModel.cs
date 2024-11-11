namespace OttApiPlatform.Domain.Entities.Content;

[Table("Collections")]
public class CollectionModel : BaseEntity, IMustHaveTenant
{
    public Guid TenantId { get; set; }

    public string? Title { get; set; } 

    public string? Description { get; set; }

    public string? SeoTitle { get; set; }

    public string? SeoDescription { get; set; }

    public string? Slug { get; set; }

    public int Position { get; set; }

    public string? ImageUrl { get; set; } 

    #region foreign keys

    #endregion

    #region Navigational Properties
    //public ICollection<VideoCollectionModel>? VideosCollections { get; set; }
    //public ICollection<CategoryCollectionModel>? CategoriesCollections { get; set; }
    //public ICollection<PodcastCollectionModel>? PodcastsCollections { get; set; }

    #endregion

}
