using OttApiPlatform.Domain.Entities.Products;

namespace OttApiPlatform.Application.Common.Contracts.Repository
{
    public interface IProductItemRepository
    {
        //Task<PagedList<ProductItemModel>> GetItemsAsync(Guid productFamilyId, ItemParameters itemParameters, bool trackChanges);
        Task<ProductItemModel?> GetItemAsync(Guid itemId, bool trackChanges);
        Task<ProductItemModel?> FindItemByConditionAsync(Expression<Func<ProductItemModel, bool>> expression, bool trackChanges);
        Task CreateProductItemForProductFamily(Guid productFamilyId, ProductItemModel item);
        Task<IEnumerable<ProductItemModel>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        void DeleteItem(ProductItemModel item);
        Task<bool> ProductItemExistsAsync(Expression<Func<ProductItemModel, bool>> expression);
    }
}
