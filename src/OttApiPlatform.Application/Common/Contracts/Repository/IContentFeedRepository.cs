using OttApiPlatform.Domain.Entities.Content;

namespace OttApiPlatform.Application.Common.Contracts.Repository
{
    public interface IContentFeedRepository
    {
        //Task<PagedList<ContentFeedModel>> GetContentFeedsAsync(Guid tenantId, ContentFeedParameters contentFeedParameters, bool trackChanges);
        Task<ContentFeedModel?> GetContentFeedAsync(Guid contentFeedId, bool trackChanges);
        Task<ContentFeedModel?> FindContentFeedByConditionAsync(Expression<Func<ContentFeedModel, bool>> expression, bool trackChanges);
        //void CreateContentFeedForTenant(Guid tenantId, ContentFeedModel contentFeed);
        Task<IEnumerable<ContentFeedModel>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        void DeleteContentFeed(ContentFeedModel contentFeed);
        Task<bool> ContentFeedExistsAsync(Expression<Func<ContentFeedModel, bool>> expression);
    }
}
