namespace OttApiPlatform.CMS.Features.ContentManagement.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommand
{
    public Guid Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public int Position { get; set; }

    public string? ImageUrl { get; set; }

    public string? SeoTitle { get; set; }

    public string? SeoDescription { get; set; }

    public string? Slug { get; set; }

    public bool IsImageAdded { get; set; }

    // public IFormFile? ImageFile { get; set; }
}
