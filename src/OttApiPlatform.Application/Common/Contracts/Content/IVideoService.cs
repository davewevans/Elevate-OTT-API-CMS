using OttApiPlatform.Domain.Entities.ContentManagement;

namespace OttApiPlatform.Application.Common.Contracts.Content;

public interface IVideoService
{
    public Task<VideoModel> GetVideo(Guid id);
    public Task<Guid> CreateVideo(VideoModel video, CancellationToken cancellationToken);
    public void UpdateVideo();
    public void DeleteVideo();
}
