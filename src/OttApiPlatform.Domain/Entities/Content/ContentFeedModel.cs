using System.ComponentModel.DataAnnotations;

namespace OttApiPlatform.Domain.Entities.Content;

[Table("ContentFeeds")]
public class ContentFeedModel : BaseEntity, IMustHaveTenant
{
    public Guid TenantId { get; set; }

    [Url]
    public string? Url { get; set; }

    public int Version { get; set; }

    public DistributionType DistributionType { get; set; }

    public string? ProviderName { get; set; } 

    public string? Language { get; set; }

    public string? LastUpdated { get; set; }
}
