using OttApiPlatform.Domain.Entities.Content;

namespace OttApiPlatform.Application.Common.Contracts.Repository
{
    public interface ICollectionRepository
    {
        //Task<PagedList<CollectionModel>> GetCollectionsAsync(Guid tenantId, CollectionParameters collectionParameters, bool trackChanges);
        Task<CollectionModel?> GetCollectionAsync(Guid collectionId, bool trackChanges);
        Task<CollectionModel?> FindCollectionByConditionAsync(Expression<Func<CollectionModel, bool>> expression, bool trackChanges);
        //void CreateCollectionForTenant(Guid tenantId, CollectionModel collection);
        Task<IEnumerable<CollectionModel>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        void DeleteCollection(CollectionModel collection);
        Task<bool> CollectionExistsAsync(Expression<Func<CollectionModel, bool>> expression);
    }
}
