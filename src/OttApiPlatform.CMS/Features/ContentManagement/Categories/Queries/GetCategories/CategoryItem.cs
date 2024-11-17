namespace OttApiPlatform.CMS.Features.ContentManagement.Categories.Queries.GetCategories;

public class CategoryItem : AuditableDto
{
    #region Public Properties

    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public int Position { get; set; }

    public string? ImageUrl { get; set; }

    public string? SeoTitle { get; set; }

    public string? SeoDescription { get; set; }

    public string? Slug { get; set; }

    #endregion Public Properties
}
