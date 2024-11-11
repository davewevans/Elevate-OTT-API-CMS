namespace OttApiPlatform.Application.Features.ContentManagement.Authors.Queries.GetAuthorForEdit;

public class AuthorForEdit : AuditableDto
{
    #region Public Properties

    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Bio { get; set; } 
    public string? ImageUrl { get; set; }
    public string? SeoTitle { get; set; } 
    public string? SeoDescription { get; set; } 
    public string? Slug { get; set; } 

    #endregion Public Properties
}
