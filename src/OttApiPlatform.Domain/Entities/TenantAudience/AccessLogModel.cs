using OttApiPlatform.Domain.Entities.ContentManagement;
using OttApiPlatform.Domain.Entities.Identity;

namespace OttApiPlatform.Domain.Entities.TenantAudience;

public class AccessLogModel : BaseEntity, IMustHaveTenant
{
    public Guid TenantId { get; set; }

    public Guid? PlaylistId { get; set; }

    public Guid? SeriesId { get; set; }

    public Guid? SwimLaneId { get; set; }

    public DateTime AccessTime { get; set; }

    public Guid UserId { get; set; }

    public Guid? ContentId { get; set; }

    public ApplicationUser User { get; set; }

    public ContentModel Content { get; set; }
}
