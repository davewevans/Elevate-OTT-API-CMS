namespace OttApiPlatform.Domain.Entities.Content;

[Table("PodcastsCategories")]
public class PodcastCategoryModel : IMustHaveTenant
{
    public Guid TenantId { get; set; }

    public Guid PodcastId { get; set; }

    public Guid CategoryId { get; set; }

    public PodcastModel? Podcast { get; set; }

    public CategoryModel? Category { get; set; }
}
