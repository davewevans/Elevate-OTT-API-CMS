using OttApiPlatform.CMS.Features.ContentManagement.Categories.Commands.CreateCategory;
using OttApiPlatform.CMS.Features.ContentManagement.Categories.Queries.GetCategories;
using OttApiPlatform.CMS.Features.ContentManagement.Categories.Queries.GetCategoriesForAutoComplete;
using OttApiPlatform.CMS.Features.ContentManagement.Categories.Queries.GetCategoryForEdit;

namespace OttApiPlatform.CMS.Consumers;

public class CategoriesClient : ICategoriesClient
{
    #region Private Fields

    private readonly IHttpService _httpService;
    private const string ControllerName = "categories";

    #endregion Private Fields

    #region Public Constructors

    public CategoriesClient(IHttpService httpService)
    {
        _httpService = httpService;
    }

    #endregion Public Constructors

    public async Task<ApiResponseWrapper<CategoryForEdit>> GetCategory(Guid id)
    {
        return await _httpService.Get<CategoryForEdit>($"{ControllerName}/{id}");
    }

    public async Task<ApiResponseWrapper<CategoriesResponse>> GetCategories(GetCategoriesQuery request)
    {
        return await _httpService.Post<GetCategoriesQuery, CategoriesResponse>($"{ControllerName}", request);
    }

    public async Task<ApiResponseWrapper<CategoriesForAutoCompleteResponse>> GetCategoriesForAutoComplete(GetCategoriesForAutoCompleteQuery request)
    {
        return await _httpService.Post<GetCategoriesForAutoCompleteQuery, CategoriesForAutoCompleteResponse>($"{ControllerName}/auto-complete", request);
    }

    public async Task<ApiResponseWrapper<CreateCategoryResponse>> CreateCategory(MultipartFormDataContent request)
    {
        return await _httpService.PostFormData<MultipartFormDataContent, CreateCategoryResponse>($"{ControllerName}/add", request);
    }

    public async Task<ApiResponseWrapper<string>> UpdateCategory(MultipartFormDataContent request)
    {
        Console.WriteLine("UpdateCategory invoked");
        Console.WriteLine("request: " + request);
        return await _httpService.PutFormData<MultipartFormDataContent, string>($"{ControllerName}", request);
    }

    public async Task<ApiResponseWrapper<string>> DeleteCategory(Guid id)
    {
        return await _httpService.Delete<string>($"{ControllerName}/{id}");
    }
}
