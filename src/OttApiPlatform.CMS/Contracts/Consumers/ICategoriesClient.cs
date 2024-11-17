using OttApiPlatform.CMS.Features.ContentManagement.Categories.Commands.CreateCategory;
using OttApiPlatform.CMS.Features.ContentManagement.Categories.Queries.GetCategories;
using OttApiPlatform.CMS.Features.ContentManagement.Categories.Queries.GetCategoriesForAutoComplete;
using OttApiPlatform.CMS.Features.ContentManagement.Categories.Queries.GetCategoryForEdit;

namespace OttApiPlatform.CMS.Contracts.Consumers;

public interface ICategoriesClient
{
    #region Public Methods

    Task<ApiResponseWrapper<CategoryForEdit>> GetCategory(Guid id);
    Task<ApiResponseWrapper<CategoriesResponse>> GetCategories(GetCategoriesQuery request);
    Task<ApiResponseWrapper<CategoriesForAutoCompleteResponse>> GetCategoriesForAutoComplete(GetCategoriesForAutoCompleteQuery request);
    Task<ApiResponseWrapper<CreateCategoryResponse>> CreateCategory(MultipartFormDataContent request);
    Task<ApiResponseWrapper<string>> UpdateCategory(MultipartFormDataContent request);
    Task<ApiResponseWrapper<string>> DeleteCategory(Guid id);

    #endregion Public Methods
}
