namespace OttApiPlatform.Application.Features.ContentManagement.Categories.Queries.GetCategoriesForAutoComplete;

public class CategoryItemForAutoComplete
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string ImageUrl { get; set; } = string.Empty;
}
