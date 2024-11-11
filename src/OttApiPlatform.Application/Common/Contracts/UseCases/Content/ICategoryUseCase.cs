using OttApiPlatform.Application.Features.ContentManagement.Categories.Commands.CreateCategory;
using OttApiPlatform.Application.Features.ContentManagement.Categories.Commands.DeleteCategory;
using OttApiPlatform.Application.Features.ContentManagement.Categories.Commands.UpdateCategory;
using OttApiPlatform.Application.Features.ContentManagement.Categories.Queries.GetCategories;
using OttApiPlatform.Application.Features.ContentManagement.Categories.Queries.GetCategoryForEdit;

namespace OttApiPlatform.Application.Common.Contracts.UseCases.Content;

public interface ICategoryUseCase
{
    #region Public Methods

    Task<Envelope<CategoryForEdit>> GetCategory(GetCategoryForEditQuery request);
    Task<Envelope<CategoriesResponse>> GetCategories(GetCategoriesQuery request);
    Task<Envelope<CreateCategoryResponse>> AddCategory(CreateCategoryCommand request);
    Task<Envelope<string>> EditCategory(UpdateCategoryCommand request);
    Task<Envelope<string>> DeleteCategory(DeleteCategoryCommand request);

    #endregion Public Methods
}
