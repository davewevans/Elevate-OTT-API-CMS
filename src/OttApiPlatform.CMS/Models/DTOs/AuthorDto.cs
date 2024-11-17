namespace OttApiPlatform.CMS.Models.DTOs;

public class AuthorDto : AuditableDto
{    
    public Guid Id { get; set; }

    public Guid TenantId { get; set; }

    public string? Name { get; set; }

    public string? Bio { get; set; }

    public string? ImageUrl { get; set; }

    public string? SeoTitle { get; set; }

    public string? SeoDescription { get; set; }

    public string? Slug { get; set; }
}
