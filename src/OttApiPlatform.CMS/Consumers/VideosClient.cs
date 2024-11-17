﻿using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using OttApiPlatform.CMS.Features.ContentManagement.Videos.Commands.CreateVideo;
using OttApiPlatform.CMS.Features.ContentManagement.Videos.Queries.GetNewStorageName;
using OttApiPlatform.CMS.Features.ContentManagement.Videos.Queries.GetSasToken;
using OttApiPlatform.CMS.Features.ContentManagement.Videos.Queries.GetVideoForEdit;
using OttApiPlatform.CMS.Features.ContentManagement.Videos.Queries.GetVideos;
using OttApiPlatform.CMS.Features.ContentManagement.Videos.Queries.GetVideosForAutoComplete;
using OttApiPlatform.CMS.Models.Videos;

namespace OttApiPlatform.CMS.Consumers;

public class VideosClient : IVideosClient
{
    #region Private Fields

    private readonly IHttpService _httpService;
    private const string ControllerName = "videos";

    #endregion Private Fields

    #region Public Constructors

    public VideosClient(IHttpService httpService)
    {
        _httpService = httpService;
    }

    #endregion Public Constructors

    public async Task<ApiResponseWrapper<VideoForEdit>> GetVideo(Guid id)
    {
        return await _httpService.Get<VideoForEdit>($"{ControllerName}/{id}");
    }

    public async Task<ApiResponseWrapper<SasTokenResponse>> GetAzureBlobSasToken()
    {
        return await _httpService.Get<SasTokenResponse>($"{ControllerName}/azure-blob-sas-token");
    }

    public async Task<ApiResponseWrapper<NewStorageNameResponse>> GetNewStorageName()
    {
        return await _httpService.Get<NewStorageNameResponse>($"{ControllerName}/new-storage-name");
    }

    public async Task<ApiResponseWrapper<VideosResponse>> GetVideos(GetVideosQuery request)
    {
        return await _httpService.Post<GetVideosQuery, VideosResponse>($"{ControllerName}", request);
    }

    public async Task<ApiResponseWrapper<VideosForAutoCompleteResponse>> GetVideosForAutoComplete(GetVideosForAutoCompleteQuery request)
    {
        return await _httpService.Post<GetVideosForAutoCompleteQuery, VideosForAutoCompleteResponse>($"{ControllerName}/auto-complete", request);
    }

    //public async Task<ApiResponseWrapper<object>> CreateVideo(MultipartFormDataContent request)
    //{
    //    return await _httpService.PostFormData<MultipartFormDataContent, CreateVideoResponse>($"{ControllerName}", request);
    //}

    //public async Task StoreVideosForStreaming()
    //{
    //    await _httpService.Get($"{ControllerName}/new-storage-name");
    //}

    public async Task<ApiResponseWrapper<CreateVideoResponse>> CreateVideo(CreateVideoCommand request)
    {
        return await _httpService.Post<CreateVideoCommand, CreateVideoResponse>($"{ControllerName}/add", request);
    }

    public async Task DirectUploadToAzureStorageAsync(Uri uriSasToken, UploadFileModel file, CancellationToken cancellationToken = default)
    {
        BlockBlobClient blockBlobClient = new BlockBlobClient(uriSasToken);

        Console.WriteLine($"name: {blockBlobClient.Name}");
        Console.WriteLine($"container name: {blockBlobClient.BlobContainerName}");
        Console.WriteLine($"file size: {file.FileSize}");

        await using var stream = file.BrowserFile?.OpenReadStream(file.MaxSizeAllowed, cancellationToken);

        var uploadOptions = new BlobUploadOptions
        {
            HttpHeaders = new BlobHttpHeaders
            {
                ContentType = file.ContentType
            },
            ProgressHandler = new Progress<long>(p =>
            {
                if (file.UploadProgress is null) return;
                file.UploadProgress.Value = Convert.ToDouble(p * 100 / file.FileSize);
                Console.WriteLine($"progress: {file.UploadProgress.Value}");
            })
        };

        await blockBlobClient.UploadAsync(stream, uploadOptions, cancellationToken);
    }
    

    public async Task<ApiResponseWrapper<string>> UpdateVideo(MultipartFormDataContent request)
    {
        Console.WriteLine("UpdateVideo invoked");
        Console.WriteLine("request: " + request);
        return await _httpService.PutFormData<MultipartFormDataContent, string>($"{ControllerName}", request);
    }

    //public async Task<ApiResponseWrapper<object>> UpdateVideo(UpdateVideoCommand request)
    //{
    //    return await _httpService.Put<UpdateVideoCommand, string>("videos", request);
    //}

    public async Task<ApiResponseWrapper<string>> DeleteVideo(Guid id)
    {
        return await _httpService.Delete<string>($"{ControllerName}/{id}");
    }
}
