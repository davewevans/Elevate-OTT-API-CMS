namespace OttApiPlatform.CMS.Features.ContentManagement.Authors.Queries.GetAuthorsForAutoComplete;

public class AuthorItemForAutoComplete
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
}
