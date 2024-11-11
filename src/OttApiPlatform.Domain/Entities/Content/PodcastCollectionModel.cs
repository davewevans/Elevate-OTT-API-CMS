namespace OttApiPlatform.Domain.Entities.Content;

[Table("PodcastsCollections")]
public class PodcastCollectionModel : IMustHaveTenant
{
    public Guid TenantId { get; set; }

    public Guid PodcastId { get; set; }

    public Guid CollectionId { get; set; }

    public PodcastModel? Podcast { get; set; }

    public CollectionModel? Collection { get; set; }
}
