using OttApiPlatform.Domain.Entities.Products;

namespace OttApiPlatform.Application.Common.Contracts.Repository
{
    public interface IItemPriceRepository
    {
        //Task<PagedList<ItemPriceModel>> GetItemPricesAsync(Guid productItemId, ItemPriceParameters itemPriceParameters, bool trackChanges);
        Task<ItemPriceModel?> GetItemPriceAsync(Guid itemPriceId, bool trackChanges);
        Task<ItemPriceModel?> FindItemPriceByConditionAsync(Expression<Func<ItemPriceModel, bool>> expression, bool trackChanges);
        Task CreateItemPriceForProductItem(Guid productItemId, ItemPriceModel itemPrice);
        Task<IEnumerable<ItemPriceModel>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        void DeleteItemPrice(ItemPriceModel itemPrice);
        Task<bool> ItemPriceExistsAsync(Expression<Func<ItemPriceModel, bool>> expression);
    }
}
