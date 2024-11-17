namespace OttApiPlatform.CMS.Features.ContentManagement.Authors.Commands.UpdateAuthor;

public class UpdateAuthorCommand
{
    #region Public Properties

    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string SeoTitle { get; set; } = string.Empty;
    public string SeoDescription { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public bool IsImageAdded { get; set; }
    // public IFormFile? ImageFile { get; set; }

    #endregion Public Properties
}
