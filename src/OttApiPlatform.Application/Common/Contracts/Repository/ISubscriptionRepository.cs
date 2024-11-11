using OttApiPlatform.Domain.Entities.Subscriptions;

namespace OttApiPlatform.Application.Common.Contracts.Repository
{
    public interface ISubscriptionRepository
    {
        //Task<PagedList<SubscriptionModel>> GetSubscriptionsAsync(Guid tenantId, SubscriptionParameters subscriptionParameters, bool trackChanges);
        Task<SubscriptionModel?> GetSubscriptionAsync(Guid subscriptionId, bool trackChanges);
        Task<SubscriptionModel?> FindSubscriptionByConditionAsync(Expression<Func<SubscriptionModel, bool>> expression, bool trackChanges);
        //void CreateSubscriptionForTenant(Guid tenantId, SubscriptionModel subscription);
        Task<IEnumerable<SubscriptionModel>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        void DeleteSubscription(SubscriptionModel subscription);
        Task<bool> SubscriptionExistsAsync(Expression<Func<SubscriptionModel, bool>> expression);
    }
}
