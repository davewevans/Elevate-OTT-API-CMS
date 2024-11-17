namespace OttApiPlatform.CMS.Features.ContentManagement.Authors.Queries.GetAuthors;

public class AuthorItem : AuditableDto
{
    #region Public Properties

    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string SeoTitle { get; set; } = string.Empty;
    public string SeoDescription { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;

    #endregion Public Properties
}
