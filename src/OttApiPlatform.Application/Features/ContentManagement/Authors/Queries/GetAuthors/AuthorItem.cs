using OttApiPlatform.Domain.Entities.Content;

namespace OttApiPlatform.Application.Features.ContentManagement.Authors.Queries.GetAuthors;

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

    #region Public Methods

    public static AuthorItem MapFromEntity(AuthorModel author)
    {
        return new()
        {
            Id = author.Id,
            Name = author.Name,
            Bio = author.Bio,
            ImageUrl = author.ImageUrl,
            SeoTitle = author.SeoTitle,
            SeoDescription = author.SeoDescription,
            Slug = author.Slug,
            CreatedOn = author.CreatedOn,
            CreatedBy = author.CreatedBy,
            ModifiedOn = author.ModifiedOn,
            ModifiedBy = author.ModifiedBy,
        };
    }

    #endregion Public Methods
}
