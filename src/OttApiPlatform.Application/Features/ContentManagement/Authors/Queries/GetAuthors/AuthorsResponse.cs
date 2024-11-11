namespace OttApiPlatform.Application.Features.ContentManagement.Authors.Queries.GetAuthors;

public class AuthorsResponse
{
    #region Public Properties

    public PagedList<AuthorItem>? Authors { get; set; }

    #endregion Public Properties
}
