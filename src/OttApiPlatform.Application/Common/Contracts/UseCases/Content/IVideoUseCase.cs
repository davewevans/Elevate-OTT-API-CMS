using OttApiPlatform.Application.Features.ContentManagement.Videos.Commands.CreateVideo;
using OttApiPlatform.Application.Features.ContentManagement.Videos.Commands.DeleteVideo;
using OttApiPlatform.Application.Features.ContentManagement.Videos.Commands.UpdateVideo;
using OttApiPlatform.Application.Features.ContentManagement.Videos.Queries.ExportVideos;
using OttApiPlatform.Application.Features.ContentManagement.Videos.Queries.GetSasToken;
using OttApiPlatform.Application.Features.ContentManagement.Videos.Queries.GetVideoForEdit;
using OttApiPlatform.Application.Features.ContentManagement.Videos.Queries.GetVideos;

namespace OttApiPlatform.Application.Common.Contracts.UseCases.Content;
public interface IVideoUseCase
{
    #region Public Methods

    Task<Envelope<VideoForEdit>> GetVideo(GetVideoForEditQuery request);
    Task<Envelope<VideosResponse>> GetVideos(GetVideosQuery request);
    Task<Envelope<CreateVideoResponse>> AddVideo(CreateVideoCommand request);
    Task<Envelope<string>> EditVideo(UpdateVideoCommand request);
    Task<Envelope<string>> DeleteVideo(DeleteVideoCommand request);
    Task<Envelope<ExportVideosResponse>> ExportAsPdf(ExportVideosQuery request);
    Envelope<SasTokenResponse> GetSasTokenFromAzure();

    #endregion Public Methods
}
