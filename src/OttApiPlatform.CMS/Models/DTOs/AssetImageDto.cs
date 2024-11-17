namespace OttApiPlatform.CMS.Models.DTOs;

public class AssetImageDto : AuditableDto
{
    public Guid TenantId { get; set; }

    public string? Name { get; set; } = string.Empty;

    public AssetImageType AssetImageType { get; set; }

    public string? Url { get; set; } = string.Empty;

    public int Width { get; set; }

    public int Height { get; set; }
}
