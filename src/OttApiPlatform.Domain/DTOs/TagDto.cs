using OttApiPlatform.Domain.Common.DTOs;

namespace OttApiPlatform.Domain.DTOs;

public class TagDto : AuditableDto
{
    public Guid TenantId { get; set; }

    public string Name { get; set; }
}
