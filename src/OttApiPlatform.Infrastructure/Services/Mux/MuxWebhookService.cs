using AutoMapper;
using Microsoft.Extensions.Logging;
using OttApiPlatform.Application.Common.Contracts.Mux;
using OttApiPlatform.Application.Common.Contracts.Repository;
using OttApiPlatform.Application.Common.Models.Mux;
using OttApiPlatform.Domain.Entities.Content;
using OttApiPlatform.Domain.Enums.Content;
using OttApiPlatform.Domain.Exceptions;

namespace ElevateOTT.Infrastructure.Services.Mux;

public class MuxWebhookService : IMuxWebhookService
{
    // TODO https://docs.mux.com/guides/video/verify-webhook-signatures

    // TODO signalR to notify portal of updates from Mux

    private readonly IRepositoryManager _repositoryManager;
    private readonly ILogger<MuxWebhookService> _logger;
    private readonly IMapper _mapper;
    private readonly IConfigReaderService _configReaderService;
    //private readonly IHubContext<VideoHub> _videoHubContext;
    private readonly IVideoHubNotificationService _videoHubNotificationService;
    private readonly IApplicationDbContext _dbContext;
    private readonly ITenantResolver _tenantResolver;

    public MuxWebhookService(IRepositoryManager repositoryManager,
        ILogger<MuxWebhookService> logger,
        IMapper mapper,
        IConfiguration configuration, 
        IVideoHubNotificationService videoHubNotificationService, 
        IConfigReaderService configReaderService, 
        IApplicationDbContext dbContext, 
        ITenantResolver tenantResolver)
    {
        _repositoryManager = repositoryManager;
        _logger = logger;
        _mapper = mapper;
        _videoHubNotificationService = videoHubNotificationService;
        _configReaderService = configReaderService;
        _dbContext = dbContext;
        this._tenantResolver = tenantResolver;
    }

    public async Task<bool> HandleWebHookEvent(MuxWebhookRequest? hookRequest)
    {
        bool eventHandled = false;

        if (hookRequest == null)
        {
            _logger.LogInformation("HandleWebHookEvent invoked but hookRequest is null.");
            return eventHandled;
        }

        switch (hookRequest.Type)
        {
            case "video.asset.created":
                eventHandled = await HandleVideoAssetCreated(hookRequest);
                break;
            case "video.asset.ready":
                eventHandled = await HandleVideoAssetReady(hookRequest);
                break;
            case "video.asset.errored":
                eventHandled = await HandleVideoAssetErrored(hookRequest);
                break;
            case "video.asset.updated":
                eventHandled = await HandleVideoAssetUpdated(hookRequest);
                break;
            case "video.asset.deleted":
                eventHandled = await HandleVideoAssetDeleted(hookRequest);
                break;
            case "video.asset.live_stream_completed":
                eventHandled = await HandleVideoAssetLiveStreamCompleted(hookRequest);
                break;
            case "video.asset.static_renditions.ready":
                eventHandled = await HandleVideoAssetStaticRenditionsReady(hookRequest);
                break;
            case "video.asset.static_renditions.preparing":
                eventHandled = await HandleVideoAssetStaticRenditionsPreparing(hookRequest);
                break;
            case "video.asset.static_renditions.deleted":
                eventHandled = await HandleVideoAssetStaticRenditionsDeleted(hookRequest);
                break;
            case "video.asset.static_renditions.errored":
                eventHandled = await HandleVideoAssetStaticRenditionsErrored(hookRequest);
                break;
            case "video.asset.master.ready":
                eventHandled = await HandleVideoAssetMasterReady(hookRequest);
                break;
            case "video.asset.master.preparing":
                eventHandled = await HandleVideoAssetMasterPreparing(hookRequest);
                break;
            case "video.asset.master.deleted":
                eventHandled = await HandleVideoAssetMasterDeleted(hookRequest);
                break;
            case "video.asset.master.errored":
                eventHandled = await HandleVideoAssetMasterErrored(hookRequest);
                break;
            case "video.asset.track.created":
                eventHandled = await HandleVideoAssetTrackCreated(hookRequest);
                break;
            case "video.asset.track.ready":
                eventHandled = await HandleVideoAssetTrackReady(hookRequest);
                break;
            case "video.asset.track.errored":
                eventHandled = await HandleVideoAssetTrackErrored(hookRequest);
                break;
            case "video.asset.track.deleted":
                eventHandled = await HandleVideoAssetTrackDeleted(hookRequest);
                break;
            case "video.upload.asset_created":
                eventHandled = await HandleVideoUploadAssetCreated(hookRequest);
                break;
            case "video.upload.cancelled":
                eventHandled = await HandleVideoUploadCancelled(hookRequest);
                break;
            case "video.upload.created":
                eventHandled = await HandleVideoUploadCreated(hookRequest);
                break;
            case "video.upload.errored":
                eventHandled = await HandleVideoUploadErrored(hookRequest);
                break;
            case "video.live_stream.created":
                eventHandled = await HandleVideoLiveStreamCreated(hookRequest);
                break;
            case "video.live_stream.connected":
                eventHandled = await HandleVideoLiveStreamConnected(hookRequest);
                break;
            case "video.live_stream.recording":
                eventHandled = await HandleVideoLiveStreamRecording(hookRequest);
                break;
            case "video.live_stream.active":
                eventHandled = await HandleVideoLiveStreamActive(hookRequest);
                break;
            case "video.live_stream.disconnected":
                eventHandled = await HandleVideoLiveStreamDisconnected(hookRequest);
                break;
            case "video.live_stream.idle":
                eventHandled = await HandleVideoLiveStreamIdle(hookRequest);
                break;
            case "video.live_stream.updated":
                eventHandled = await HandleVideoLiveStreamUpdated(hookRequest);
                break;
            case "video.live_stream.enabled":
                eventHandled = await HandleVideoLiveStreamEnabled(hookRequest);
                break;
            case "video.live_stream.disabled":
                eventHandled = await HandleVideoLiveStreamDisabled(hookRequest);
                break;
            case "video.live_stream.deleted":
                eventHandled = await HandleVideoLiveStreamDeleted(hookRequest);
                break;
            case "video.live_stream.simulcast_target.created":
                eventHandled = await HandleVideoLiveStreamSimulcastTargetCreated(hookRequest);
                break;
            case "video.live_stream.simulcast_target.idle":
                eventHandled = await HandleVideoLiveStreamSimulcastTargetIdle(hookRequest);
                break;
            case "video.live_stream.simulcast_target.starting":
                eventHandled = await HandleVideoLiveStreamSimulcastTargetStarting(hookRequest);
                break;
            case "video.live_stream.simulcast_target.broadcasting":
                eventHandled = await HandleVideoLiveStreamSimulcastTargetBroadcasting(hookRequest);
                break;
            case "video.live_stream.simulcast_target.errored":
                eventHandled = await HandleVideoLiveStreamSimulcastTargetErrored(hookRequest);
                break;
            case "video.live_stream.simulcast_target.deleted":
                eventHandled = await HandleVideoLiveStreamSimulcastTargetDeleted(hookRequest);
                break;
            default:
                _logger.LogInformation("HandleWebHookEvent invoked but to type found.");
                return eventHandled;
        }

        return eventHandled;
    }

    public (string timestamp, string muxSignature) GetMuxTimestampAndSignature(string? muxHeader)
    {
        if (muxHeader == null) return ("", "");
        var splitMuxHeader = muxHeader.Split(",");
        string timestamp = splitMuxHeader[0].Split("=")[1];
        string muxSignature = splitMuxHeader[1].Split("=")[1];
        return (timestamp, muxSignature);
    }

    public bool VerifyRequestFromMux(string timestamp, string signature, string requestBody)
    {
        // ref: https://docs.mux.com/guides/video/verify-webhook-signatures

        var muxOptions = _configReaderService.GetMuxOptions();

        string secret = muxOptions.SigningSecret;
        byte[] secretBytes = Encoding.ASCII.GetBytes(secret);
        string payload = $"{timestamp}.{requestBody}";
        byte[] payloadBytes = Encoding.ASCII.GetBytes(payload);
        using var hmac = new HMACSHA256(secretBytes);
        var stream = new MemoryStream(payloadBytes);
        byte[] hash = hmac.ComputeHash(stream);
        string payloadHash = Convert.ToHexString(hash).ToLower();

        bool isWithinTolerance = false;
        if (long.TryParse(timestamp, out var timeStampLong))
        {
            var timeStampLocalTime = DateTimeOffset.FromUnixTimeSeconds(timeStampLong).ToLocalTime();
            var span = DateTime.Now.ToLocalTime() - timeStampLocalTime;
            isWithinTolerance = span.Minutes <= 5;
        }

        return signature.Equals(payloadHash) && isWithinTolerance;
    }

    public void SetTenantViaTenantResolver(string passthrough)
    {
        var passthroughSplit = passthrough.Split("/");
        var tenantId = Guid.Parse(passthroughSplit[0]);
        _tenantResolver.SetTenantId(tenantId);
    }

    public Task TestSignalR()
    {
        throw new NotImplementedException();
    }

    #region direct upload handlers
    private async Task<bool> HandleVideoUploadCreated(MuxWebhookRequest? hookRequest)
    {
        _logger.LogInformation($"Handle Mux Web Hook for : {hookRequest.Type}");

        return await Task.FromResult(true);
    }
    private async Task<bool> HandleVideoUploadAssetCreated(MuxWebhookRequest? hookRequest)
    {
        _logger.LogInformation($"Handle Mux Web Hook for : {hookRequest.Type}");

        return await Task.FromResult(true);
    }
    private async Task<bool> HandleVideoUploadCancelled(MuxWebhookRequest? hookRequest)
    {
        _logger.LogInformation($"Handle Mux Web Hook for : {hookRequest.Type}");

        return await Task.FromResult(true);
    }
    private async Task<bool> HandleVideoUploadErrored(MuxWebhookRequest? hookRequest)
    {
        _logger.LogError($"Handle Mux Web Hook for : {hookRequest.Type}");

        return await Task.FromResult(true);
    }
    #endregion



    #region SignalR Tests
    //public async Task TestSignalR()
    //{
    //    // TODO use User instead of keeping track of connection ids

    //    await _chatHubContext.Clients.All.SendAsync("ReceiveMessage", "superdave", "Hi from MuxWebhookService!");
    //}

    #endregion

    #region video asset handlers
    public async Task<bool> HandleVideoAssetCreated(MuxWebhookRequest? hookRequest)
    {
        _logger.LogInformation($"Handle Mux Web Hook for : {hookRequest.Type}");

        if (hookRequest.Data is null) return false;

        string passthrough = GetPassthroughValue(hookRequest.Data.Passthrough);

        var videoToUpdate = _repositoryManager.Video.GetVideoByPassthrough(passthrough);

        if (videoToUpdate == null) return false;

        // Possible that 'Asset Ready' handler called first for short videos
        if (videoToUpdate.StreamCreationStatus != AssetCreationStatus.None) return true;
        videoToUpdate.StreamCreationStatus = AssetCreationStatus.Preparing;
        videoToUpdate.AssetId = hookRequest.Data.Id;

        await _repositoryManager.SaveAsync();

        return true;
    }

    private async Task<bool> HandleVideoAssetReady(MuxWebhookRequest? hookRequest)
    {
        _logger.LogInformation($"Handle Mux Web Hook for : {hookRequest.Type}");

        if (hookRequest.Data is null) return false;

        string passthrough = GetPassthroughValue(hookRequest.Data.Passthrough);

        var videoToUpdate = _repositoryManager.Video.GetVideoByPassthrough(passthrough);
        if (videoToUpdate is null) return false;

        if (videoToUpdate.StreamCreationStatus == AssetCreationStatus.Ready) return true;

        videoToUpdate.StreamCreationStatus = AssetCreationStatus.Ready;
        videoToUpdate.AssetId = hookRequest.Data.Id;
        videoToUpdate.Duration = TimeSpan.FromSeconds(hookRequest.Data.Duration);
        videoToUpdate.IsTestAsset = hookRequest.Data.Test;
        videoToUpdate.Mp4Support = hookRequest.Data.Mp4Support == "standard";
        videoToUpdate.IsHostedOnMux = true;

        if (hookRequest.Data.PlaybackIds != null)
        {
            var muxOptions = _configReaderService.GetMuxOptions();
            string? publicPlaybackId = GetPublicPlaybackId(hookRequest.Data.PlaybackIds);
            string? signedPlaybackId = GetSignedPlaybackId(hookRequest.Data.PlaybackIds);
            string baseStreamUrl = muxOptions.BaseStreamUrl;
            string baseImageUrl = muxOptions.BaseImageUrl;
            videoToUpdate.SignedPlaybackId = signedPlaybackId;
            if (!string.IsNullOrEmpty(publicPlaybackId))
            {
                videoToUpdate.StreamUrl = $"{baseStreamUrl}/{publicPlaybackId}.m3u8";
                videoToUpdate.PublicPlaybackId = publicPlaybackId;
                videoToUpdate.ThumbnailUrl = GetVideoImageUrlAtMux(publicPlaybackId, baseImageUrl, 140, 64);
                videoToUpdate.PlayerImageUrl = GetVideoImageUrlAtMux(publicPlaybackId, baseImageUrl, 1920, 1080);
                videoToUpdate.CatalogImageUrl = GetVideoImageUrlAtMux(publicPlaybackId, baseImageUrl, 1480, 840);
                videoToUpdate.FeaturedCatalogImageUrl = GetVideoImageUrlAtMux(publicPlaybackId, baseImageUrl, 1900, 800);
                videoToUpdate.AnimatedGifUrl = GetVideoAnimatedGifUrlAtMux(publicPlaybackId, baseImageUrl, 1920);
            }
        }

        await _repositoryManager.SaveAsync();

        // TODO remove
        await Task.Delay(5000);

        // Update client with new status via SignalR
        await _videoHubNotificationService.NotifyCreationStatus(videoToUpdate.Id, videoToUpdate.StreamCreationStatus);

       // await _videoHubContext.Clients.All.SendAsync(VideoHub.ReceiveUpdateMethod, videoToUpdate.Id, videoToUpdate.StreamCreationStatus);

        return true;

    }

    private async Task<bool> HandleVideoAssetUpdated(MuxWebhookRequest? hookRequest)
    {
        // ref: https://docs.mux.com/api-reference/video#operation/update-asset

        string passthrough = GetPassthroughValue(hookRequest.Data.Passthrough);

        var videoToUpdate = await GetVideoByAssetId(hookRequest.Data.Id);
        if (videoToUpdate == null) return false;

        await _repositoryManager.SaveAsync();

        // Update client with new status via SignalR
        await _videoHubNotificationService.NotifyCreationStatus(videoToUpdate.Id, videoToUpdate.StreamCreationStatus);

        _logger.LogInformation($"Handle Mux Web Hook for : {hookRequest.Type}");

        return true;
    }

    private async Task<bool> HandleVideoAssetDeleted(MuxWebhookRequest? hookRequest)
    {
        if (hookRequest?.Data == null) return false;

        // Video deleted during API request

        _logger.LogInformation($"Handle Mux Web Hook for : {hookRequest.Type}");

        return await Task.FromResult(true);
    }

    private async Task<bool> HandleVideoAssetLiveStreamCompleted(MuxWebhookRequest? hookRequest)
    {

        //
        // TODO save live stream as video asset on complete
        //

        if (hookRequest?.Data == null) return false;
        _logger.LogInformation($"Handle Mux Web Hook for : {hookRequest.Type}");

        return true;
    }

    private async Task<bool> HandleVideoAssetErrored(MuxWebhookRequest? hookRequest)
    {
        if (hookRequest?.Data?.Errors?.Messages == null) return false;

        var videoToUpdate = await GetVideoByAssetId(hookRequest.Data.Id);
        if (videoToUpdate == null) throw new VideoNotFoundException();

        videoToUpdate.StreamCreationStatus = AssetCreationStatus.Errored;

        // Update client with new status via SignalR
        await _videoHubNotificationService.NotifyCreationStatus(videoToUpdate.Id, videoToUpdate.StreamCreationStatus);

        await _repositoryManager.SaveAsync();

        _logger.LogInformation($"Handle Mux Web Hook for : {hookRequest.Type}");
        _logger.LogInformation($"Error type: {hookRequest.Data.Errors.Type}");
        _logger.LogInformation($"Error messages:");
        foreach (var message in hookRequest.Data.Errors.Messages)
        {
            _logger.LogInformation(message);
        }

        return true;
    }
    #endregion

    #region video asset track handlers
    private async Task<bool> HandleVideoAssetTrackDeleted(MuxWebhookRequest? hookRequest)
    {
        _logger.LogInformation($"Handle Mux Web Hook for : {hookRequest.Type}");

        return true;
    }

    private async Task<bool> HandleVideoAssetTrackErrored(MuxWebhookRequest? hookRequest)
    {
        _logger.LogInformation($"Handle Mux Web Hook for : {hookRequest.Type}");

        return true;
    }

    private async Task<bool> HandleVideoAssetTrackReady(MuxWebhookRequest? hookRequest)
    {
        _logger.LogInformation($"Handle Mux Web Hook for : {hookRequest.Type}");

        return true;
    }

    private async Task<bool> HandleVideoAssetTrackCreated(MuxWebhookRequest? hookRequest)
    {
        _logger.LogInformation($"Handle Mux Web Hook for : {hookRequest.Type}");

        return true;
    }
    #endregion

    #region live stream handlers
    private async Task<bool> HandleVideoLiveStreamCreated(MuxWebhookRequest? hookRequest)
    {
        // Saved in db after creation in service
        _logger.LogInformation($"Handle Mux Web Hook for : {hookRequest.Type}");

        return true;
    }

    private async Task<bool> HandleVideoLiveStreamActive(MuxWebhookRequest? hookRequest)
    {
        //var directUpload = await _repositoryManager.LiveStream
        //    .GetLiveStreamByPassthroughAsync(hookRequest.Data.Passthrough, true);

        //if (directUpload == null) return false;

        //directUpload.Status = LiveStreamStatus.Active;
        //await _repositoryManager.SaveAsync();

        _logger.LogInformation($"Handle Mux Web Hook for : {hookRequest.Type}");

        return true;
    }

    private async Task<bool> HandleVideoLiveStreamEnabled(MuxWebhookRequest? hookRequest)
    {

        //var directUpload = await _repositoryManager.LiveStream
        //    .GetLiveStreamByPassthroughAsync(hookRequest.Data.Passthrough, true);

        //if (directUpload == null) return false;

        //directUpload.Status = LiveStreamStatus.Idle;
        //await _repositoryManager.SaveAsync();

        _logger.LogInformation($"Handle Mux Web Hook for : {hookRequest.Type}");

        return true;
    }
    private async Task<bool> HandleVideoLiveStreamIdle(MuxWebhookRequest? hookRequest)
    {
        //var directUpload = await _repositoryManager.LiveStream
        //    .GetLiveStreamByPassthroughAsync(hookRequest.Data.Passthrough, true);

        //if (directUpload == null) return false;

        //directUpload.Status = LiveStreamStatus.Idle;
        //await _repositoryManager.SaveAsync();

        _logger.LogInformation($"Handle Mux Web Hook for : {hookRequest.Type}");

        return true;
    }

    private async Task<bool> HandleVideoLiveStreamDisabled(MuxWebhookRequest? hookRequest)
    {

        //var directUpload = await _repositoryManager.LiveStream
        //    .GetLiveStreamByPassthroughAsync(hookRequest.Data.Passthrough, true);

        //if (directUpload == null) return false;

        //directUpload.Status = LiveStreamStatus.Disabled;
        //await _repositoryManager.SaveAsync();

        _logger.LogInformation($"Handle Mux Web Hook for : {hookRequest.Type}");

        return true;
    }

    private async Task<bool> HandleVideoLiveStreamUpdated(MuxWebhookRequest? hookRequest)
    {

        //var directUpload = await _repositoryManager.LiveStream
        //    .GetLiveStreamByPassthroughAsync(hookRequest.Data.Passthrough, true);

        //if (directUpload == null) return false;

        //if (HasLiveStreamDataChanged(directUpload, hookRequest))
        //{
        //    directUpload.StreamKey = hookRequest.Data.StreamKey;
        //    directUpload.LatencyMode = MapToLatencyMode(hookRequest.Data.LatencyMode);
        //    directUpload.ReconnectWindow = hookRequest.Data.ReconnectWindow;
        //    directUpload.MaxContinuousDuration = hookRequest.Data.MaxContinuousDuration;
        //    await _repositoryManager.SaveAsync();
        //}

        _logger.LogInformation($"Handle Mux Web Hook for : {hookRequest.Type}");

        return true;
    }

    private async Task<bool> HandleVideoLiveStreamDisconnected(MuxWebhookRequest? hookRequest)
    {
        _logger.LogInformation($"Handle Mux Web Hook for : {hookRequest.Type}");

        return true;
    }

    private async Task<bool> HandleVideoLiveStreamRecording(MuxWebhookRequest? hookRequest)
    {
        _logger.LogInformation($"Handle Mux Web Hook for : {hookRequest.Type}");

        return true;
    }

    private async Task<bool> HandleVideoLiveStreamConnected(MuxWebhookRequest? hookRequest)
    {
        _logger.LogInformation($"Handle Mux Web Hook for : {hookRequest.Type}");

        return true;
    }

    private async Task<bool> HandleVideoLiveStreamDeleted(MuxWebhookRequest? hookRequest)
    {

        //var directUpload = await _repositoryManager.LiveStream
        //    .GetLiveStreamByPassthroughAsync(hookRequest.Data.Passthrough, true);

        //if (directUpload == null) throw new LiveStreamNotFoundException();

        //if (directUpload.TenantId.HasValue)
        //{
        //    await _service.LiveStreamService.DeleteLiveStreamForTenantAsync(directUpload.TenantId.Value,
        //        directUpload.Id, trackChanges: false);
        //}

        _logger.LogInformation($"Handle Mux Web Hook for : {hookRequest.Type}");

        return true;
    }
    #endregion

    #region live stream simulcast handlers
    private async Task<bool> HandleVideoLiveStreamSimulcastTargetDeleted(MuxWebhookRequest? hookRequest)
    {
        _logger.LogInformation($"Handle Mux Web Hook for : {hookRequest.Type}");

        return true;
    }

    private async Task<bool> HandleVideoLiveStreamSimulcastTargetErrored(MuxWebhookRequest? hookRequest)
    {
        _logger.LogInformation($"Handle Mux Web Hook for : {hookRequest.Type}");

        return true;
    }

    private async Task<bool> HandleVideoLiveStreamSimulcastTargetBroadcasting(MuxWebhookRequest? hookRequest)
    {
        _logger.LogInformation($"Handle Mux Web Hook for : {hookRequest.Type}");

        return true;
    }

    private async Task<bool> HandleVideoLiveStreamSimulcastTargetStarting(MuxWebhookRequest? hookRequest)
    {
        _logger.LogInformation($"Handle Mux Web Hook for : {hookRequest.Type}");

        return true;
    }

    private async Task<bool> HandleVideoLiveStreamSimulcastTargetIdle(MuxWebhookRequest? hookRequest)
    {
        _logger.LogInformation($"Handle Mux Web Hook for : {hookRequest.Type}");

        return true;
    }

    private async Task<bool> HandleVideoLiveStreamSimulcastTargetCreated(MuxWebhookRequest? hookRequest)
    {
        _logger.LogInformation($"Handle Mux Web Hook for : {hookRequest.Type}");

        return true;
    }

    #endregion

    #region video asset master handlers
    private async Task<bool> HandleVideoAssetMasterErrored(MuxWebhookRequest? hookRequest)
    {
        _logger.LogInformation($"Handle Mux Web Hook for : {hookRequest.Type}");

        return true;
    }

    private async Task<bool> HandleVideoAssetMasterDeleted(MuxWebhookRequest? hookRequest)
    {
        _logger.LogInformation($"Handle Mux Web Hook for : {hookRequest.Type}");

        return true;
    }

    private async Task<bool> HandleVideoAssetMasterPreparing(MuxWebhookRequest? hookRequest)
    {
        _logger.LogInformation($"Handle Mux Web Hook for : {hookRequest.Type}");

        return true;
    }

    private async Task<bool> HandleVideoAssetMasterReady(MuxWebhookRequest? hookRequest)
    {
        _logger.LogInformation($"Handle Mux Web Hook for : {hookRequest.Type}");

        return true;
    }
    #endregion

    #region video asset static renditions handlers
    private async Task<bool> HandleVideoAssetStaticRenditionsPreparing(MuxWebhookRequest? hookRequest)
    {
        if (hookRequest?.Data?.StaticRenditions == null) return false;

        var video = await GetVideoByAssetId(hookRequest.Data.Id);
        if (video == null) throw new VideoNotFoundException();

        if (video.StreamCreationStatus != AssetCreationStatus.Preparing)
        {
            video.StreamCreationStatus = AssetCreationStatus.Preparing;
            await _repositoryManager.SaveAsync();
        }

        _logger.LogInformation($"Handle Mux Web Hook for : {hookRequest.Type}");

        return true;
    }

    private async Task<bool> HandleVideoAssetStaticRenditionsReady(MuxWebhookRequest? hookRequest)
    {
        if (hookRequest?.Data?.StaticRenditions?.Files == null) return false;

        if (hookRequest.Data.PlaybackIds != null)
        {
            var muxOptions = _configReaderService.GetMuxOptions();

            string playbackId = GetPublicPlaybackId(hookRequest.Data.PlaybackIds);
            string baseStreamUrl = muxOptions.BaseStreamUrl;

            var video = await GetVideoByAssetId(hookRequest.Data.Id);
            if (video == null) throw new VideoNotFoundException();

            // ref: https://docs.mux.com/guides/video/enable-static-mp4-renditions#why-enable-mp4-support

            // low, medium, high
            string? fileName = GetStaticRenditionFileName(hookRequest);
            if (fileName == null) return false;

            video.StreamCreationStatus = AssetCreationStatus.Ready;
            video.DownloadUrl = $"{baseStreamUrl}/{playbackId}/{fileName}";

            await _repositoryManager.SaveAsync();
        }


        _logger.LogInformation($"Handle Mux Web Hook for : {hookRequest.Type}");

        return true;
    }

    private async Task<bool> HandleVideoAssetStaticRenditionsErrored(MuxWebhookRequest? hookRequest)
    {
        if (hookRequest?.Data?.NewAssetSettings == null) return false;

        var video = await GetVideoByAssetId(hookRequest.Data.Id);
        if (video == null) throw new VideoNotFoundException();

        video.StreamCreationStatus = AssetCreationStatus.Errored;
        await _repositoryManager.SaveAsync();

        _logger.LogInformation($"Handle Mux Web Hook for : {hookRequest.Type}");

        return true;
    }

    private async Task<bool> HandleVideoAssetStaticRenditionsDeleted(MuxWebhookRequest? hookRequest)
    {
        if (hookRequest?.Data?.NewAssetSettings == null) return false;

        var video = await GetVideoByAssetId(hookRequest.Data.Id);
        if (video == null) throw new VideoNotFoundException();

        video.StreamCreationStatus = AssetCreationStatus.Deleted;
        await _repositoryManager.SaveAsync();

        _logger.LogInformation($"Handle Mux Web Hook for : {hookRequest.Type}");

        return true;
    }
    #endregion

    #region private methods

    private string GetPassthroughValue(string passthrough)
    {
        var passthroughSplit = passthrough.Split("/");
        return passthroughSplit.Length > 1 ? passthroughSplit[1] : passthroughSplit[0];
    }

    //private async Task CreateNewVideoAsset(MuxWebhookRequest hookRequest)
    //{

    //    //if (tenantId == null) return;

    //    var videoCreation = new VideoModel
    //    {
    //        //TenantId = tenantId,
    //        AssetId = hookRequest.Data.Id,
    //        StreamCreationStatus = AssetCreationStatus.Ready,
    //        IsHostedOnMux = true,
    //        Mp4Support = hookRequest.Data.Mp4Support == "standard",
    //        IsTestAsset = hookRequest.Data.Test,
    //        Passthrough = hookRequest.Data.Passthrough,
    //        Duration = TimeSpan.FromSeconds(hookRequest.Data.Duration),
    //    };

    //    if (hookRequest.Data.PlaybackIds != null)
    //    {
    //        var muxOptions = _configReaderService.GetMuxOptions();

    //        string? playbackId = GetPublicPlaybackId(hookRequest.Data.PlaybackIds);
    //        string baseStreamUrl = muxOptions.BaseStreamUrl;
    //        // videoCreation.VideoImages = GetVideoImageUrls(playbackId, baseStreamUrl);
    //        videoCreation.StreamUrl = $"{baseStreamUrl}/{playbackId}.m3u8";
    //        videoCreation.PublicPlaybackId = playbackId;
    //    }

    //    //_repositoryManager.Video.CreateVideoForTenant(tenantId, videoCreation);
    //    //await _repositoryManager.SaveAsync();
    //}

    private async Task<VideoModel?> GetVideoByAssetId(string assetId)
    {
        var video = await _repositoryManager.Video.FindVideoByConditionAsync(x =>
            x.AssetId.Equals(assetId), true);

        return video;
    }
    //private List<AssetImageModel> GetVideoImageUrls(string playbackId, string baseStreamUrl, Guid tenantId, Guid videoId)
    //{
    //    // ref: https://docs.mux.com/guides/video/get-images-from-a-video

    //    var videoImages = new List<AssetImageModel>
    //        {
    //            new()
    //            {
    //                Name = "Player_Image",
    //                AssetImageType = AssetImageType.PlayerImage,
    //                Url = GetVideoImageUrlAtMux(playbackId, baseStreamUrl, 140, 64),
    //                Width = 1920,
    //                Height = 1080,
    //                TenantId = tenantId,
    //                VideoId = videoId
    //            },
    //            new()
    //            {
    //                Name = "Catalog_Image",
    //                AssetImageType = AssetImageType.CatalogImage,
    //                Url = GetVideoImageUrlAtMux(playbackId, baseStreamUrl, 140, 64),
    //                Width = 1480,
    //                Height = 840,
    //                TenantId = tenantId,
    //                VideoId = videoId
    //            },
    //            new()
    //            {
    //                Name = "Featured_Catalog_Image",
    //                AssetImageType = AssetImageType.FeaturedCatalogImage,
    //                Url = GetVideoImageUrlAtMux(playbackId, baseStreamUrl, 140, 64),
    //                Width = 1900,
    //                Height = 800,
    //                TenantId = tenantId,
    //                VideoId = videoId
    //            },
    //            new()
    //            {
    //                Name = "Animated_Gif_Url",
    //                AssetImageType = AssetImageType.AnimatedGif,
    //                Url = $"{baseStreamUrl}/{playbackId}/animated.gif",
    //                Width = 640,
    //                Height = 0,
    //                TenantId = tenantId,
    //                VideoId = videoId
    //            },
    //        };

    //    return videoImages;
    //}

    private string GetVideoImageUrlAtMux(string playbackId, string baseStreamUrl, int width, int height) =>
        $"{baseStreamUrl}/{playbackId}/thumbnail.png?width={width}&height={height}&fit_mode=pad";

    private string GetVideoAnimatedGifUrlAtMux(string playbackId, string baseStreamUrl, int width) =>
        $"{baseStreamUrl}/{playbackId}/animated.gif?width={width}";

    private string? GetStaticRenditionFileName(MuxWebhookRequest? hookRequest)
    {
        string? fileName = hookRequest?.Data?.StaticRenditions?.Files?.Count switch
        {
            1 =>
                $"{hookRequest.Data.StaticRenditions.Files[0].Name}",
            2 =>
                $"{hookRequest.Data.StaticRenditions.Files[1].Name}",
            3 =>
                $"{hookRequest.Data.StaticRenditions.Files[2].Name}",
            4 =>
                $"{hookRequest.Data.StaticRenditions.Files[3].Name}",
            _ => null
        };

        return fileName;
    }

    public string? GetPublicPlaybackId(List<PlaybackId> playbackIds)
    {
        var id = playbackIds
            .FirstOrDefault(x => x.Policy.Equals("public"))
            ?.Id;
        return id;
    }

    public string? GetSignedPlaybackId(List<PlaybackId> playbackIds)
    {
        var id = playbackIds
            .FirstOrDefault(x => x.Policy.Equals("signed"))
            ?.Id;
        return id;
    }

    private LatencyMode MapToLatencyMode(string latencyMode)
    {
        return latencyMode switch
        {
            "low" => LatencyMode.Low,
            "reduced" => LatencyMode.Reduced,
            "standard" => LatencyMode.Standard,
            _ => LatencyMode.Standard
        };
    }

    private bool HasLiveStreamDataChanged(LiveStreamModel directUpload, MuxWebhookRequest hookRequest)
    {
        if (hookRequest?.Data == null) return false;

        if (directUpload.StreamKey != hookRequest.Data.StreamKey) return true;
        if (directUpload.LatencyMode != MapToLatencyMode(hookRequest.Data.LatencyMode)) return true;
        if (directUpload.ReconnectWindow != hookRequest.Data.ReconnectWindow) return true;
        if (directUpload.MaxContinuousDuration != hookRequest.Data.MaxContinuousDuration) return true;

        return false;
    }
    #endregion
}

