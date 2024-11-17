using OttApiPlatform.CMS.Features.ContentManagement.Authors.Commands.CreateAuthor;
using OttApiPlatform.CMS.Features.ContentManagement.Authors.Queries.GetAuthorForEdit;
using OttApiPlatform.CMS.Features.ContentManagement.Authors.Queries.GetAuthors;
using OttApiPlatform.CMS.Features.ContentManagement.Authors.Queries.GetAuthorsForAutoComplete;

namespace OttApiPlatform.CMS.Contracts.Consumers;

public interface IAuthorsClient
{
    #region Public Methods

    Task<ApiResponseWrapper<AuthorForEdit>> GetAuthor(Guid id);
    Task<ApiResponseWrapper<AuthorsResponse>> GetAuthors(GetAuthorsQuery request);
    Task<ApiResponseWrapper<AuthorsForAutoCompleteResponse>> GetAuthorsForAutoComplete(GetAuthorsForAutoCompleteQuery request);
    Task<ApiResponseWrapper<CreateAuthorResponse>> CreateAuthor(MultipartFormDataContent request);
    Task<ApiResponseWrapper<string>> UpdateAuthor(MultipartFormDataContent request);
    Task<ApiResponseWrapper<string>> DeleteAuthor(Guid id);

    #endregion Public Methods
}
