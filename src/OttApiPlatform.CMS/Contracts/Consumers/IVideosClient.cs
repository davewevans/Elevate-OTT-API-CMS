using OttApiPlatform.CMS.Features.ContentManagement.Videos.Commands.CreateVideo;
using OttApiPlatform.CMS.Features.ContentManagement.Videos.Queries.GetNewStorageName;
using OttApiPlatform.CMS.Features.ContentManagement.Videos.Queries.GetSasToken;
using OttApiPlatform.CMS.Features.ContentManagement.Videos.Queries.GetVideoForEdit;
using OttApiPlatform.CMS.Features.ContentManagement.Videos.Queries.GetVideos;
using OttApiPlatform.CMS.Features.ContentManagement.Videos.Queries.GetVideosForAutoComplete;
using OttApiPlatform.CMS.Models.Videos;

namespace OttApiPlatform.CMS.Contracts.Consumers;

public interface IVideosClient
{
    #region Public Methods

    Task<ApiResponseWrapper<VideoForEdit>> GetVideo(Guid id);
    Task<ApiResponseWrapper<VideosResponse>> GetVideos(GetVideosQuery request);
    Task<ApiResponseWrapper<SasTokenResponse>> GetAzureBlobSasToken();
    Task<ApiResponseWrapper<NewStorageNameResponse>> GetNewStorageName();
    Task<ApiResponseWrapper<VideosForAutoCompleteResponse>> GetVideosForAutoComplete(GetVideosForAutoCompleteQuery request);
    Task DirectUploadToAzureStorageAsync(Uri uriSasToken, UploadFileModel file,
        CancellationToken cancellationToken = default);
    //Task StoreVideosForStreaming();

    //Task<ApiResponseWrapper<object>> CreateVideo(MultipartFormDataContent request);
    Task<ApiResponseWrapper<CreateVideoResponse>> CreateVideo(CreateVideoCommand request);
    Task<ApiResponseWrapper<string>> UpdateVideo(MultipartFormDataContent request);
    //Task<ApiResponseWrapper<object>> UpdateVideo(UpdateVideoCommand request);
    Task<ApiResponseWrapper<string>> DeleteVideo(Guid id);

    #endregion Public Methods
}
