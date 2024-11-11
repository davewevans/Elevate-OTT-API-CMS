using AutoMapper;
using OttApiPlatform.Application.Common.Contracts.Mux;
using OttApiPlatform.Application.Common.Contracts.Reports;
using OttApiPlatform.Application.Common.Contracts.Repository;
using OttApiPlatform.Application.Common.Contracts.UseCases.Content;
using OttApiPlatform.Application.Features.ContentManagement.Videos.Commands.CreateVideo;
using OttApiPlatform.Application.Features.ContentManagement.Videos.Commands.DeleteVideo;
using OttApiPlatform.Application.Features.ContentManagement.Videos.Commands.UpdateVideo;
using OttApiPlatform.Application.Features.ContentManagement.Videos.Queries.ExportVideos;
using OttApiPlatform.Application.Features.ContentManagement.Videos.Queries.GetSasToken;
using OttApiPlatform.Application.Features.ContentManagement.Videos.Queries.GetVideoForEdit;
using OttApiPlatform.Application.Features.ContentManagement.Videos.Queries.GetVideos;
using OttApiPlatform.Application.Features.ContentManagement.Videos.Queries.GetVideosForAutoComplete;
using OttApiPlatform.Domain.Entities.Content;
using OttApiPlatform.Domain.Enums.Content;

namespace OttApiPlatform.Application.UseCases.Content;
public class VideoUseCase : IVideoUseCase
{
    #region Private Fields

    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    public readonly IRepositoryManager _repositoryManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IReportingService _reportingService;
    private readonly ITenantResolver _tenantResolver;
    private readonly IStorageProvider _storageProvider;
    private readonly IConfigReaderService _configReaderService;
    private readonly IMuxAssetService _muxAssetService;

    #endregion Private Fields

    #region Public Constructors

    public VideoUseCase(IApplicationDbContext dbContext,
                            IHttpContextAccessor httpContextAccessor,
                            IReportingService reportingService,
                            IMapper mapper,
                            IRepositoryManager repositoryManager,
                            ITenantResolver tenantResolver,
                            IStorageProvider storageProvider,
                            IConfigReaderService configReaderService,
                            IMuxAssetService muxAssetService)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
        _reportingService = reportingService;
        _mapper = mapper;
        _repositoryManager = repositoryManager;
        _tenantResolver = tenantResolver;
        _storageProvider = storageProvider;
        _configReaderService = configReaderService;
        _muxAssetService = muxAssetService;
    }

    #endregion Public Constructors

    #region Public Methods

    public async Task<Envelope<VideoForEdit>> GetVideo(GetVideoForEditQuery request)
    {
        if (request.Id is null)
        {
            return Envelope<VideoForEdit>.Result.BadRequest(Resource.Invalid_video_Id);
        }

        var tenantId = _tenantResolver.GetTenantId();

        if (tenantId is null)
        {
            return Envelope<VideoForEdit>.Result.BadRequest(Resource.Invalid_tenant_Id);
        }

        var video = await _repositoryManager.Video.GetVideoAsync(tenantId.Value, request.Id.Value, false);
   
        if (video == null)
            return Envelope<VideoForEdit>.Result.NotFound(Resource.Unable_to_load_video);

        var videoForEdit = _mapper.Map<VideoForEdit>(video);

        if (video.VideosCategories is not null)
        {
            videoForEdit.CategoryIds = new List<Guid>();
            foreach (var vc in video.VideosCategories)
            {
                videoForEdit?.CategoryIds.Add(vc.CategoryId);
            }
        }        

        // videos
        if (video?.TrailerVideoId is not null)
        {
            var trailerVideo = await _repositoryManager.Video.GetVideoAsync(tenantId.Value, video.TrailerVideoId.Value, false);
            videoForEdit.TrailerVideo = trailerVideo is not null ? new VideoItemForAutoComplete
            {
                Id = trailerVideo.Id,
                Title = trailerVideo.Title,
                ThumbnailUrl = trailerVideo.ThumbnailUrl,
                Duration = trailerVideo.Duration
            } : null;
        }

        if (video?.FeaturedCategoryVideoId is not null)
        {
            var featuredCategoryVideo = await _repositoryManager.Video.GetVideoAsync(tenantId.Value, video.FeaturedCategoryVideoId.Value, false);
            videoForEdit.TrailerVideo = featuredCategoryVideo is not null ? new VideoItemForAutoComplete
            {
                Id = featuredCategoryVideo.Id,
                Title = featuredCategoryVideo.Title,
                ThumbnailUrl = featuredCategoryVideo.ThumbnailUrl,
                Duration = featuredCategoryVideo.Duration
            } : null;
        }

        return Envelope<VideoForEdit>.Result.Ok(videoForEdit);
    }

    public async Task<Envelope<VideosResponse>> GetVideos(GetVideosQuery request)
    {
        var tenantId = _tenantResolver.GetTenantId();

        if (tenantId is null)
        {
            return Envelope<VideosResponse>.Result.BadRequest(Resource.Invalid_tenant_Id);
        }

        var query = _repositoryManager.Video.GetVideos(tenantId.Value, request, false);

        // TODO should query be nullable ?????

        var videoItems = query is not null
            ? await query.Select(video => _mapper.Map<VideoItem>(video))
                .ToPagedListAsync(request.PageNumber, request.RowsPerPage)
            : null;

        var videosResponse = new VideosResponse
        {
            Videos = videoItems
        };

        return Envelope<VideosResponse>.Result.Ok(videosResponse);
    }

    public async Task<Envelope<CreateVideoResponse>> AddVideo(CreateVideoCommand request)
    {
        var tenantId = _tenantResolver.GetTenantId();

        if (tenantId is null)
        {
            return Envelope<CreateVideoResponse>.Result.BadRequest(Resource.Invalid_tenant_Id);
        }

        var video = _mapper.Map<VideoModel>(request);

        var blobOptions = _configReaderService.GetBlobOptions();
        string blobBaseUrl = blobOptions.BlobBaseUrl;
        string containerName = blobOptions.VideoBlobContainerName;

        string videoUrl = $"{blobBaseUrl}/{containerName}/{video.BlobName}";
        video.Passthrough = Guid.NewGuid().ToString();
        video.StreamCreationStatus = AssetCreationStatus.Preparing;
        video.DownloadUrl = videoUrl;
        video.TenantId = tenantId.Value;

        _repositoryManager.Video.CreateVideoForTenant(tenantId.Value, video);
        await _repositoryManager.SaveAsync();

        var createVideoResponse = new CreateVideoResponse
        {
            TenantId = video.TenantId,
            Id = video.Id,
            SuccessMessage = Resource.Video_has_been_created_successfully,
            BlobUrl = videoUrl,
            DownloadUrl = videoUrl,
            Passthrough = video.Passthrough,
            LanguageCode = video.LanguageCode ?? string.Empty,
            ClosedCaptions = video.ClosedCaptions,
            Mp4Support = video.Mp4Support,
            IsTestAsset = video.IsTestAsset
        };

        return Envelope<CreateVideoResponse>.Result.Ok(createVideoResponse);
    }
   

    public async Task<Envelope<string>> EditVideo(UpdateVideoCommand request)
    {
        var tenantId = _tenantResolver.GetTenantId();

        if (tenantId is null)
        {
            return Envelope<string>.Result.BadRequest(Resource.Invalid_tenant_Id);
        }      

        string? fileNamePrefix = string.Empty;

        if (_dbContext.Tenants != null)
        {
            var tenant = await _dbContext.Tenants.FindAsync(tenantId);
            fileNamePrefix = tenantId.Value.GetStorageFileNamePrefix();
        }

        var videoEntity = await _repositoryManager.Video.GetVideoAsync(tenantId.Value, request.Id, true);

        if (videoEntity == null)
            return Envelope<string>.Result.NotFound(Resource.Unable_to_load_video);

        if (request.AuthorId.Equals(Guid.Empty)) request.AuthorId = null;

        // store catories 
        var categoryIdsAsStrings = request.CategoryIdsAsStrings?.Split(",");
        if (categoryIdsAsStrings != null && categoryIdsAsStrings.Count() > 0)
        {
            videoEntity.VideosCategories = new List<VideoCategoryModel>();

            foreach (var categoryId in categoryIdsAsStrings)
            {
                videoEntity.VideosCategories.Add(new VideoCategoryModel
                {
                    VideoId = videoEntity.Id,
                    CategoryId = Guid.Parse(categoryId)
                });
            }
        }      

        MapRequestToEntity(request, videoEntity);
 
        var storageService = _storageProvider.InvokeInstanceForAzureStorage();

        videoEntity.PlayerImageUrl = await UpdateImageAsync(request.PlayerImage, fileNamePrefix, videoEntity?.PlayerImageUrl ?? string.Empty, storageService, request.PlayerImageState);
        videoEntity.CatalogImageUrl = await UpdateImageAsync(request.CatalogImage, fileNamePrefix, videoEntity?.CatalogImageUrl ?? string.Empty, storageService, request.CatalogImageState);
        videoEntity.FeaturedCatalogImageUrl = await UpdateImageAsync(request.FeaturedCatalogImage, fileNamePrefix, videoEntity?.FeaturedCatalogImageUrl ?? string.Empty, storageService, request.FeaturedCatalogImageState);
        videoEntity.AnimatedGifUrl = await UpdateImageAsync(request.AnimatedGif, fileNamePrefix, videoEntity?.AnimatedGifUrl ?? string.Empty, storageService, request.AnimatedGifState);

        await _repositoryManager.SaveAsync();

        return Envelope<string>.Result.Ok(Resource.Video_has_been_updated_successfully);
    }

    private void MapRequestToEntity(UpdateVideoCommand request, VideoModel videoEntity)
    {
        videoEntity.Title = request.Title;
        videoEntity.FullDescription = request.FullDescription;
        videoEntity.ShortDescription = request.ShortDescription;
        videoEntity.AllowDownload = request.AllowDownload;
        videoEntity.SeoTitle = request.SeoTitle;
        videoEntity.SeoDescription = request.SeoDescription;
        videoEntity.Slug = request.Slug;
        videoEntity.AuthorId = request.AuthorId;
        videoEntity.TrailerVideoId = request.TrailerVideoId;
        videoEntity.FeaturedCategoryVideoId = request.FeaturedCategoryVideoId;
        videoEntity.PublicationStatus = request.PublicationStatus;
        videoEntity.ContentAccess = request.ContentAccess;
        videoEntity.ReleasedDate = request.ReleasedDate;
        videoEntity.ExpirationDate = request.ExpirationDate; 
        videoEntity.HasOneTimePurchasePrice = request.HasOneTimePurchasePrice; 
        videoEntity.OneTimePurchasePrice = request.OneTimePurchasePrice; 
        videoEntity.HasRentalPrice = request.HasRentalPrice; 
        videoEntity.RentalPrice = request.RentalPrice; 
        videoEntity.RentalDuration = request.RentalDuration; 
    }

    public async Task<Envelope<string>> DeleteVideo(DeleteVideoCommand request)
    {
        var tenantId = _tenantResolver.GetTenantId();

        if (tenantId is null)
        {
            return Envelope<string>.Result.BadRequest(Resource.Invalid_tenant_Id);
        }

        var videoEntity = await _repositoryManager.Video.GetVideoAsync(tenantId.Value, request.Id, true);
        await _repositoryManager.SaveAsync();

        if (videoEntity == null)
            return Envelope<string>.Result.NotFound(Resource.The_video_was_not_found);

        _repositoryManager.Video.DeleteVideo(videoEntity);
        await _repositoryManager.SaveAsync();

        return Envelope<string>.Result.Ok(Resource.Video_has_been_deleted_successfully);
    }

    public Task<Envelope<ExportVideosResponse>> ExportAsPdf(ExportVideosQuery request)
    {
        throw new NotImplementedException();
    }

    public Envelope<SasTokenResponse> GetSasTokenFromAzure()
    {
        var storageService = _storageProvider.InvokeInstanceForAzureStorage();
        var response = storageService.GetSasTokenForVideoContainer();

        return response is null ? Envelope<SasTokenResponse>.Result.NotFound(Resource.Unable_to_obtain_sas_token) 
            : Envelope<SasTokenResponse>.Result.Ok(response);
    }

    #endregion Public Methods

    #region Private Methods
    //private async Task<string?> SaveAssetImageAsync(string currentImageUri, IFormFile? image, string fileNamePrefix)
    //{
    //    var storageService = _storageProvider.InvokeInstanceForAzureStorage();

    //    string? newImageUri = null;
    //    switch (storageService.GetFileState(image, currentImageUri))
    //    {
    //        case FileStatus.Unchanged:
    //            break;

    //        case FileStatus.Modified:
    //            newImageUri = await UpdateImageAsync(image, fileNamePrefix, currentImageUri ?? string.Empty, storageService);
    //            break;

    //        case FileStatus.Deleted:
    //            var blobOptions = _configReaderService.GetBlobOptions();
    //            await storageService.DeleteFileIfExists(currentImageUri ?? string.Empty, blobOptions.ImageBlobContainerName);
    //            newImageUri = null;
    //            break;
    //        default:
    //            throw new ArgumentOutOfRangeException();
    //    }

    //    return newImageUri ?? currentImageUri;
    //}

    private async Task<string?> UpdateImageAsync(IFormFile? image, string fileNamePrefix, string currentFileUri, IFileStorageService storageService, ImageState imageState)
    {
        var blobOptions = _configReaderService.GetBlobOptions();
        switch (imageState)
        {
            case ImageState.Unchanged:
                return currentFileUri;
            case ImageState.Added:                
                if (image == null) return string.Empty;
                var newImageUri = await storageService.EditFile(image, blobOptions.ImageBlobContainerName, fileNamePrefix, currentFileUri);
                return newImageUri.FileUri;
            case ImageState.Removed:
                await storageService.DeleteFileIfExists(currentFileUri, blobOptions.ImageBlobContainerName);
                return null;
            default:
                return currentFileUri;
        }
    }
    #endregion Private Methods
}
