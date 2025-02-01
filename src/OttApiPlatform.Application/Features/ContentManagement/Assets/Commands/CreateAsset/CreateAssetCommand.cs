using Microsoft.AspNetCore.Components.Forms;
using Mux.Csharp.Sdk.Model;
using OttApiPlatform.Application.Common.Contracts;
using OttApiPlatform.Application.Common.Contracts.Infrastructure.Storage;
using OttApiPlatform.Application.Common.Contracts.Mux;
using OttApiPlatform.Domain.Entities.ContentManagement;
using OttApiPlatform.Domain.Entities.Mux;
using OttApiPlatform.Domain.Entities.POC;

namespace OttApiPlatform.Application.Features.ContentManagement.Assets.Commands.CreateAsset;

public class CreateAssetCommand : IRequest<Envelope<CreateAssetResponse>>
{
    public IFormFile File { get; set; }

    public AssetType Type { get; set; }

    public bool IsAlreadyUploaded { get; set; }

    public string FileName { get; set; }

    public string SasTokenUrl { get; set; }
}

public class Handler : IRequestHandler<CreateAssetCommand, Envelope<CreateAssetResponse>>
{
    private readonly IStorageProvider _storageProvider;
    private readonly IApplicationDbContext _context;
    private readonly IMuxConfigurationFactory _muxConfigurationFactory;
    private readonly IMuxApiService _muxService;
    private readonly ITenantResolver _tenantResolver;

    public Handler(IStorageProvider storageProvider, IApplicationDbContext context, 
        IMuxConfigurationFactory muxConfigurationFactory, IMuxApiService muxService, ITenantResolver tenantResolver)
    {
        _storageProvider = storageProvider;
        _context = context;
        _muxConfigurationFactory = muxConfigurationFactory;
        _muxService = muxService;
        _tenantResolver = tenantResolver;
    }

    public async Task<Envelope<CreateAssetResponse>> Handle(CreateAssetCommand request,
        CancellationToken cancellationToken)
    {
        var fileMetaData = request.IsAlreadyUploaded
            ? await HandleExistingAssetAsync(request, cancellationToken)
            : await UploadNewAssetAsync(request, cancellationToken);

        var asset = new AssetModel
        {
            Url = fileMetaData.FileUri ?? string.Empty,
            Type = request.Type,
            CreationStatus = GetCreationStatus(request.Type),
        };

        await _context.Assets.AddAsync(asset, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        // TODO: SignalR notification for asset creation status

        // TODO: Send asset to streaming service for processing
        var tenantId = _tenantResolver.GetTenantId().GetValueOrDefault();
        var accountInfo = await _context.AccountInfo.FirstOrDefaultAsync(x => x.TenantId == tenantId, cancellationToken);

        if (accountInfo == null)
        {
            throw new InvalidOperationException("Account information not found for the tenant.");
        }

        switch (accountInfo.VodStreamingService)
        {
            case VodStreamingService.Managed: // Company Mux account
            case VodStreamingService.Mux:
                await CreateMuxAssetAsync(request, tenantId, accountInfo, asset, cancellationToken);
                break;
            case VodStreamingService.UserProvided:
                break;
            case VodStreamingService.DaCast:
                break;
            case VodStreamingService.Wowza:
                break;
            case VodStreamingService.Brightcove:
                break;
            case VodStreamingService.Azure:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        

        // TODO: save streaming asset id to asset

        //var streamingServiceAsset = new StreamingServiceAssetModel
        //{
        //    AssetId = asset.Id,
        //    Status = StreamingServiceAssetStatus.Processing
        //};

        if (request.IsAlreadyUploaded)
        {
            // send to streaming service
        }

        // TODO: Delete file from blob storage after processing by streaming service



        //await _context.Assets.AddAsync(asset, cancellationToken);
        //await _context.SaveChangesAsync(cancellationToken);

        var createAssetResponse = new CreateAssetResponse
        {
            Id = asset.Id,
            SuccessMessage = Resource.Asset_has_been_created_successfully
        };

        return Envelope<CreateAssetResponse>.Result.Ok(createAssetResponse);
    }

    private async Task CreateMuxAssetAsync(CreateAssetCommand request, Guid tenantId,
        AccountInfoModel accountInfo, AssetModel asset, CancellationToken cancellationToken)
    {
        var muxConfiguration = await _muxConfigurationFactory.GetMuxSettingsAsync(tenantId,
            accountInfo.VodStreamingService == VodStreamingService.Managed);
        _muxService.Configure(muxConfiguration);

        var muxAssetResponse = await _muxService.CreateAssetAsync(request.SasTokenUrl, CreateAssetRequest.VideoQualityEnum.Basic);
        var muxAsset = new MuxAssetModel
        {
            Status = muxAssetResponse.Data.Status.ToString(),
            MuxAssetId = muxAssetResponse.Data.Id
        };
        await _context.MuxAssets.AddAsync(muxAsset, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        //var assetToUpdate = await _context.Assets.FirstOrDefaultAsync(x => x.Id == asset.Id, cancellationToken);
        asset.MuxAssetId = muxAsset.Id;
        _context.Assets.Update(asset);
        await _context.SaveChangesAsync(cancellationToken);
    }

    #region Private Methods

    /// <summary>
    /// Handle an existing asset already uploaded via SAS
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="FileNotFoundException"></exception>
    private async Task<FileMetaData> HandleExistingAssetAsync(CreateAssetCommand request, CancellationToken cancellationToken)
    {
        // Use the SAS token URL to confirm the file exists in Azure Blob Storage
        var storageService = await _storageProvider.InvokeInstanceAsync();

        return new FileMetaData { FileName = request.FileName };
    }

    /// <summary>
    /// Handle uploading a new asset to Azure Blob Storage
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async Task<FileMetaData> UploadNewAssetAsync(CreateAssetCommand request, CancellationToken cancellationToken)
    {
        var storageService = await _storageProvider.InvokeInstanceAsync();
        return await storageService.UploadFile(
            request.File,
            GetContainerName(request.Type),
            fileRenameAllowed: true,
            cancellationToken: cancellationToken
        );
    }

    /// <summary>
    /// Get the creation status for the asset. Videos and audio files are sent to
    /// streaming services for processing, while images and documents are ready immediately.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private AssetCreationStatus GetCreationStatus(AssetType type)
    {
        return type switch
        {
            AssetType.Video => AssetCreationStatus.None,
            AssetType.VideoBonusContent => AssetCreationStatus.None,
            AssetType.VideoClip => AssetCreationStatus.None,
            AssetType.VideoEpisode => AssetCreationStatus.None,
            AssetType.VideoTrailer => AssetCreationStatus.None,
            AssetType.Audio => AssetCreationStatus.None,
            AssetType.AudioBonusContent => AssetCreationStatus.None,
            AssetType.AudioClip => AssetCreationStatus.None,
            AssetType.AudioEpisode => AssetCreationStatus.None,
            AssetType.AudioTrailer => AssetCreationStatus.None,
            AssetType.Image => AssetCreationStatus.Ready,
            AssetType.Document => AssetCreationStatus.Ready,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    /// <summary>
    /// Save metadata to the database.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="asset"></param>
    /// <param name="fileMetaData"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    //private async Task<Guid?> SaveMetaData(CreateAssetCommand request, AssetModel asset, FileMetaData fileMetaData, CancellationToken cancellationToken)
    //{
    //    switch (request.Type)
    //    {
    //        case AssetType.Video:
    //        case AssetType.VideoTrailer:
    //        case AssetType.VideoEpisode:
    //        case AssetType.VideoClip:
    //        case AssetType.VideoBonusContent:
    //            return await SaveVideoMetaData(new VideoModel
    //            {
    //                AssetId = asset.Id,
    //                FileName = fileMetaData.FileName,
    //                FileSize = request.File.Length,
    //                ContentType = request.File.ContentType,
    //            }, cancellationToken);
    //        case AssetType.Audio:
    //        case AssetType.Image:
    //        case AssetType.Document:
    //        case AssetType.AudioTrailer:
    //        case AssetType.AudioEpisode:
    //        case AssetType.AudioClip:
    //        case AssetType.AudioBonusContent:
    //            return null;
    //        default:
    //            throw new ArgumentOutOfRangeException();
    //    }
    //}

    /// <summary>
    /// Save video metadata to the database.
    /// </summary>
    /// <param name="video"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    //private async Task<Guid> SaveVideoMetaData(VideoModel video, CancellationToken cancellationToken)
    //{
    //    var response = await _context.Videos.AddAsync(video, cancellationToken);
    //    await _context.SaveChangesAsync(cancellationToken);

    //    return response.Entity.Id;
    //}

    private string GetContainerName(AssetType type)
    {
        return type switch
        {
            AssetType.Video => "videos",
            AssetType.VideoBonusContent => "videos",
            AssetType.VideoClip => "videos",
            AssetType.VideoEpisode => "videos",
            AssetType.VideoTrailer => "videos",
            AssetType.Audio => "audio",
            AssetType.AudioBonusContent => "audio",
            AssetType.AudioClip => "audio",
            AssetType.AudioEpisode => "audio",
            AssetType.AudioTrailer => "audio",
            AssetType.Image => "images",
            AssetType.Document => "documents",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    #endregion
}
