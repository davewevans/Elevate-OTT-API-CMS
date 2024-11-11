namespace OttApiPlatform.Domain.Entities.Content;

[Table("VideosCollections")]
public class VideoCollectionModel : IMustHaveTenant
{
    public Guid TenantId { get; set; }

    public Guid VideoId { get; set; }

    public Guid CollectionId { get; set; }

    public VideoModel? Video { get; set; }

    public CollectionModel? Collection { get; set; }
}
