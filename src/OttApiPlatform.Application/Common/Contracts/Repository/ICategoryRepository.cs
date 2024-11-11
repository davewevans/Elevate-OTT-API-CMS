using OttApiPlatform.Application.Features.ContentManagement.Categories.Queries.GetCategories;
using OttApiPlatform.Domain.Entities.Content;

namespace OttApiPlatform.Application.Common.Contracts.Repository
{
    public interface ICategoryRepository
    {
        Task<CategoryModel?> GetCategoryAsync(Guid tenantId, Guid categoryId, bool trackChanges);
        IQueryable<CategoryModel>? GetCategories(Guid tenantId, GetCategoriesQuery request, bool trackChanges);
        Task<CategoryModel?> FindCategoryByConditionAsync(Expression<Func<CategoryModel, bool>> expression, bool trackChanges);
        void CreateCategory(CategoryModel category);
        Task<IEnumerable<CategoryModel>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        void DeleteCategory(CategoryModel category);
        Task<bool> CategoryExistsAsync(Expression<Func<CategoryModel, bool>> expression);
    }
}
