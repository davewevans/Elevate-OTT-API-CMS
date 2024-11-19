using OttApiPlatform.Domain.Common.DTOs;

namespace OttApiPlatform.Domain.DTOs;

public class PersonDto : AuditableDto
{
    public Guid Id { get; set; }

    public Guid TenantId { get; set; }

    public string Name { get; set; }

    public string Bio { get; set; }

    public string ImageUrl { get; set; }

    public string SeoTitle { get; set; }

    public string SeoDescription { get; set; }

    public string Slug { get; set; }
}
