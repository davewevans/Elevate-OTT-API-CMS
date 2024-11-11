namespace OttApiPlatform.Domain.Entities.Content;

[Table("LiveStreamsCategories")]
public class LiveStreamCategoryModel : IMustHaveTenant
{
    public Guid TenantId { get; set; }

    public Guid LiveStreamId { get; set; }

    public Guid CategoryId { get; set; }

    public LiveStreamModel? LiveStream { get; set; }

    public CategoryModel? Category { get; set; }
}
