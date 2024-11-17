using OttApiPlatform.CMS.Features.ContentManagement.Authors.Commands.CreateAuthor;
using OttApiPlatform.CMS.Features.ContentManagement.Authors.Queries.GetAuthorForEdit;
using OttApiPlatform.CMS.Features.ContentManagement.Authors.Queries.GetAuthors;
using OttApiPlatform.CMS.Features.ContentManagement.Authors.Queries.GetAuthorsForAutoComplete;

namespace OttApiPlatform.CMS.Consumers;

public class AuthorsClient : IAuthorsClient
{
    #region Private Fields

    private readonly IHttpService _httpService;
    private const string ControllerName = "authors";

    #endregion Private Fields

    #region Public Constructors

    public AuthorsClient(IHttpService httpService)
    {
        _httpService = httpService;
    }

    #endregion Public Constructors

    public async Task<ApiResponseWrapper<AuthorForEdit>> GetAuthor(Guid id)
    {
        return await _httpService.Get<AuthorForEdit>($"{ControllerName}/{id}");
    }

    public async Task<ApiResponseWrapper<AuthorsResponse>> GetAuthors(GetAuthorsQuery request)
    {
        return await _httpService.Post<GetAuthorsQuery, AuthorsResponse>($"{ControllerName}", request);
    }

    public async Task<ApiResponseWrapper<AuthorsForAutoCompleteResponse>> GetAuthorsForAutoComplete(GetAuthorsForAutoCompleteQuery request)
    {
        return await _httpService.Post<GetAuthorsForAutoCompleteQuery, AuthorsForAutoCompleteResponse>($"{ControllerName}/auto-complete", request);
    }

    public async Task<ApiResponseWrapper<CreateAuthorResponse>> CreateAuthor(MultipartFormDataContent request)
    {
        return await _httpService.PostFormData<MultipartFormDataContent, CreateAuthorResponse>($"{ControllerName}/add", request);
    }

    public async Task<ApiResponseWrapper<string>> UpdateAuthor(MultipartFormDataContent request)
    {
        Console.WriteLine("UpdateAuthor invoked");
        Console.WriteLine("request: " + request);
        return await _httpService.PutFormData<MultipartFormDataContent, string>($"{ControllerName}", request);
    }

    public async Task<ApiResponseWrapper<string>> DeleteAuthor(Guid id)
    {
        return await _httpService.Delete<string>($"{ControllerName}/{id}");
    }
}
