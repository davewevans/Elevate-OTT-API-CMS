using OttApiPlatform.Application.Features.ContentManagement.Videos.Queries.GetVideos;
using OttApiPlatform.Domain.Entities.Content;

namespace OttApiPlatform.Application.Common.Contracts.Repository
{
    public interface IVideoRepository
    {
        Task<VideoModel?> GetVideoAsync(Guid tenantId, Guid videoId, bool trackChanges);
        VideoModel? GetVideoByPassthrough(string passthrough);
        IQueryable<VideoModel>? GetVideos(Guid tenantId, GetVideosQuery request, bool trackChanges);
        Task<VideoModel?> FindVideoByConditionAsync(Expression<Func<VideoModel, bool>> expression, bool trackChanges);
        void CreateVideoForTenant(Guid tenantId, VideoModel video);
        Task<IEnumerable<VideoModel>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        void DeleteVideo(VideoModel video);
        Task<bool> VideoExistsAsync(Expression<Func<VideoModel, bool>> expression);
    }
}
