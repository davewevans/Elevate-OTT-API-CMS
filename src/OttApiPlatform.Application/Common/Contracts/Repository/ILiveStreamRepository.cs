using OttApiPlatform.Domain.Entities.Content;

namespace OttApiPlatform.Application.Common.Contracts.Repository
{
    public interface ILiveStreamRepository
    {
        //Task<PagedList<LiveStreamModel>> GetLiveStreamsAsync(Guid tenantId, LiveStreamParameters liveStreamParameters, bool trackChanges);
        Task<LiveStreamModel?> GetLiveStreamAsync(Guid liveStreamId, bool trackChanges);
        Task<LiveStreamModel?> FindLiveStreamByConditionAsync(Expression<Func<LiveStreamModel, bool>> expression, bool trackChanges);
        Task<LiveStreamModel?> GetLiveStreamByPassthroughAsync(string passthrough, bool trackChanges); 
        Task<LiveStreamModel?> GetLiveStreamByMuxLiveStreamIdAsync(string muxLiveStreamId, bool trackChanges);
        //void CreateLiveStreamForTenant(Guid tenantId, LiveStreamModel liveStream);
        Task<IEnumerable<LiveStreamModel>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        void DeleteLiveStream(LiveStreamModel liveStream);
        Task<bool> LiveStreamExistsAsync(Expression<Func<LiveStreamModel, bool>> expression);
    }
}
