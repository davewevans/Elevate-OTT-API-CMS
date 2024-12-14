using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Mux.Csharp.Sdk.Client;
using OttApiPlatform.Application.Common.Contracts;
using OttApiPlatform.Application.Common.Contracts.Mux;
using OttApiPlatform.Application.Common.Models.Mux;
using OttApiPlatform.Domain.Entities.ContentManagement;
using OttApiPlatform.Domain.Entities.Mux;

namespace OttApiPlatform.Infrastructure.Services.Mux;

/// <summary>
/// Handles Mux webhook events.
/// Ref: https://docs.mux.com/webhook-reference
/// </summary>
public class MuxWebHookHandler : IMuxWebHookHandler
{
    private readonly Dictionary<string, Func<MuxWebhookRequest, Task<bool>>> _eventHandlers;
    private readonly ILogger<MuxWebHookHandler> _logger;
    private readonly IApplicationDbContext _context;
    private readonly ICryptoService _encryptionService;
    private readonly ITenantResolver _tenantResolver;
    private readonly MuxOptions _muxOptions;
    private readonly IAssetHubNotificationService _hubContext;
    private readonly IWebHostEnvironment _env;
    private MuxSettingsModel _settings;
    private Configuration _muxConfig;

    public MuxWebHookHandler(ILogger<MuxWebHookHandler> logger, IConfigReaderService configReaderService, IApplicationDbContext context,
        ICryptoService encryptionService, ITenantResolver tenantResolver, IAssetHubNotificationService hubContext, IWebHostEnvironment env)
    {
        _logger = logger;
        _muxOptions = configReaderService.GetMuxOptions();
        _context = context;
        _encryptionService = encryptionService;
        _tenantResolver = tenantResolver;
        _hubContext = hubContext;
        _env = env;

        // Initialize event handlers
        _eventHandlers = new Dictionary<string, Func<MuxWebhookRequest, Task<bool>>>
        {
            // video.asset.* events
            { "video.asset.created", HandleVideoAssetCreated },
            { "video.asset.ready", HandleVideoAssetReady },
            { "video.asset.errored", HandleVideoAssetErrored },
            { "video.asset.updated", HandleVideoAssetUpdated },
            { "video.asset.deleted", HandleVideoAssetDeleted },
            { "video.asset.live_stream_completed", HandleVideoAssetLiveStreamCompleted },
            { "video.asset.static_renditions.ready", HandleVideoAssetStaticRenditionsReady },
            { "video.asset.static_renditions.preparing", HandleVideoAssetStaticRenditionsPreparing },
            { "video.asset.static_renditions.deleted", HandleVideoAssetStaticRenditionsDeleted },
            { "video.asset.static_renditions.errored", HandleVideoAssetStaticRenditionsErrored },
            { "video.asset.master.ready", HandleVideoAssetMasterReady },
            { "video.asset.master.preparing", HandleVideoAssetMasterPreparing },
            { "video.asset.master.deleted", HandleVideoAssetMasterDeleted },
            { "video.asset.master.errored", HandleVideoAssetMasterErrored },
            { "video.asset.track.created", HandleVideoAssetTrackCreated },
            { "video.asset.track.ready", HandleVideoAssetTrackReady },
            { "video.asset.track.errored", HandleVideoAssetTrackErrored },
            { "video.asset.track.deleted", HandleVideoAssetTrackDeleted },
            { "video.asset.warning", HandleVideoAssetWarning },

            // video.upload.* events
            { "video.upload.asset_created", HandleVideoUploadAssetCreated },
            { "video.upload.cancelled", HandleVideoUploadCancelled },
            { "video.upload.created", HandleVideoUploadCreated },
            { "video.upload.errored", HandleVideoUploadErrored },

            // video.live_stream.* events
            { "video.live_stream.created", HandleVideoLiveStreamCreated },
            { "video.live_stream.connected", HandleVideoLiveStreamConnected },
            { "video.live_stream.recording", HandleVideoLiveStreamRecording },
            { "video.live_stream.active", HandleVideoLiveStreamActive },
            { "video.live_stream.disconnected", HandleVideoLiveStreamDisconnected },
            { "video.live_stream.idle", HandleVideoLiveStreamIdle },
            { "video.live_stream.updated", HandleVideoLiveStreamUpdated },
            { "video.live_stream.enabled", HandleVideoLiveStreamEnabled },
            { "video.live_stream.disabled", HandleVideoLiveStreamDisabled },
            { "video.live_stream.deleted", HandleVideoLiveStreamDeleted },
            { "video.live_stream.warning", HandleVideoLiveStreamWarning },
            { "video.live_stream.simulcast_target.created", HandleVideoLiveStreamSimulcastTargetCreated },
            { "video.live_stream.simulcast_target.idle", HandleVideoLiveStreamSimulcastTargetIdle },
            { "video.live_stream.simulcast_target.starting", HandleVideoLiveStreamSimulcastTargetStarting },
            { "video.live_stream.simulcast_target.broadcasting", HandleVideoLiveStreamSimulcastTargetBroadcasting },
            { "video.live_stream.simulcast_target.errored", HandleVideoLiveStreamSimulcastTargetErrored },
            { "video.live_stream.simulcast_target.deleted", HandleVideoLiveStreamSimulcastTargetDeleted },
            { "video.live_stream.simulcast_target.updated", HandleVideoLiveStreamSimulcastTargetUpdated }
        };
    }

    /// <summary>
    /// Configures the Mux settings.
    /// </summary>
    /// <param name="settings"></param>
    public void Configure(MuxSettingsModel settings)
    {
        _settings = settings;
        _muxConfig = new Configuration();

        _muxConfig.BasePath = "https://api.mux.com";

        // Configure HTTP basic authorization: accessToken
        _muxConfig.Username = settings.Username;
        _muxConfig.Password = settings.Password;
    }

    /// <summary>
    /// Extracts the timestamp and mux signature from the Mux header.
    /// </summary>
    /// <param name="muxHeader"></param>
    /// <returns></returns>
    public (string timestamp, string muxSignature) GetMuxTimestampAndSignature(string muxHeader)
    {
        if (muxHeader == null) return ("", "");
        var splitMuxHeader = muxHeader.Split(",");
        string timestamp = splitMuxHeader[0].Split("=")[1];
        string muxSignature = splitMuxHeader[1].Split("=")[1];
        return (timestamp, muxSignature);
    }

    /// <summary>
    /// Verifies the Mux signature.
    /// </summary>
    /// <param name="signatureHeader"></param>
    /// <param name="requestBody"></param>
    /// <returns></returns>
    public bool VerifyMuxSignature(string signatureHeader, string requestBody)
    {
        try
        {
            // Split the signature header
            var segments = signatureHeader.Split(',');
            var timestampSegment = segments.FirstOrDefault(s => s.StartsWith("t="));
            var signatureSegment = segments.FirstOrDefault(s => s.StartsWith("v1="));

            if (timestampSegment == null || signatureSegment == null)
                return false;

            var timestamp = timestampSegment.Split('=')[1];
            var signature = signatureSegment.Split('=')[1];

            // Validate timestamp tolerance (5 minutes)
            if (!long.TryParse(timestamp, out var timestampLong))
                return false;

            var receivedTime = DateTimeOffset.FromUnixTimeSeconds(timestampLong);
            var currentTime = DateTimeOffset.UtcNow;
            if ((currentTime - receivedTime).TotalMinutes > 10000) // TODO: Change to 5
                return false;

            // Compute the expected signature
            var payload = $"{timestamp}.{requestBody}";
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_settings.SigningSecret));
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
            var expectedSignature = BitConverter.ToString(computedHash).Replace("-", "").ToLower();

            // Compare the provided signature with the expected signature
            return expectedSignature.Equals(signature, StringComparison.OrdinalIgnoreCase);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Gets the tenant ID from the passthrough string.
    /// Needed in case multiple values are passed in the passthrough string.
    /// Ex: "tenantId/assetId"
    /// </summary>
    /// <param name="passthrough"></param>
    public Guid ParseTenantIdFromPassthrough(string passthrough)
    {
        var passthroughSplit = passthrough.Split("/");
        
        return Guid.Parse(passthroughSplit[0]);
    }

    /// <summary>
    /// Finds the tenant ID by the environment ID.
    /// </summary>
    /// <param name="environmentId"></param>
    /// <returns></returns>
    public Guid FindTenantIdByEnvironmentId(string environmentId)
    {
        var tenantId = _context.MuxSettings
            .Where(x => x.EnvironmentId == environmentId)
            .Select(x => x.TenantId)
            .FirstOrDefault();

        return tenantId;
    }

    /// <summary>
    /// Handles a webhook event from Mux.
    /// Ref: https://docs.mux.com/webhook-reference
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<bool> HandleWebHookEvent(MuxWebhookRequest request)
    {
        if (request == null)
        {
            _logger.LogInformation("HandleWebHookEvent invoked but request is null.");
            return false;
        }

        if (_eventHandlers.TryGetValue(request.Type, out var handler))
        {
            try
            {
                return await handler(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error handling webhook event: {request.Type}");
                return false;
            }
        }

        _logger.LogInformation($"Unhandled event type: {request.Type}");
        return false;
    }

    #region Event Handlers

    private async Task<bool> HandleVideoAssetCreated(MuxWebhookRequest request)
    {
        // TODO: remove
        _logger.LogInformation($"Handle Mux Web Hook for : {request.Type}");

        if (request.Data is null)
        {
            _logger.LogWarning("Hook request data is null.");
            return false;
        }

        var asset = await _context.Assets.Include(a => a.MuxAsset)
            .FirstOrDefaultAsync(x => x.MuxAsset.MuxAssetId == request.Data.Id);

        if (asset == null)
        {
            _logger.LogWarning($"Asset with MuxAssetId {request.Data.Id} not found.");
            return false;
        }

        if (asset.CreationStatus == AssetCreationStatus.Preparing) return true;

        asset.IsTestAsset = request.Data.Test;
        string playbackId = request.Data.PlaybackIds.FirstOrDefault()?.Id;
        asset.Url = $"{_muxOptions.BaseStreamUrl}/{playbackId}.m3u8";

        asset.CreationStatus = asset.CreationStatus != AssetCreationStatus.Ready
            ? AssetCreationStatus.Preparing : AssetCreationStatus.Ready;
        asset.MuxAsset.Status = !request.Data.Status.Equals("ready") ? request.Data.Status : "ready";
        asset.MuxAsset.CreatedAt = request.CreatedAt;
        asset.MuxAsset.VideoQuality = request.Data.VideoQuality;
        asset.MuxAsset.MP4Support = request.Data.Mp4Support;
        asset.MuxAsset.MasterAccess = request.Data.MasterAccess;
        asset.MuxAsset.EncodingTier = request.Data.EncodingTier;
        asset.MuxAsset.PlaybackIds = request.Data.PlaybackIds.Select(playbackId =>
            new MuxPlaybackIdModel { PlaybackId = playbackId.Id, Policy = playbackId.Policy }).ToList();

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Asset created successfully.");

            // Notify the client using SignalR
            
            //await _hubContext.NotifyCreationStatus(asset.Id, asset.CreationStatus);
            //await _hubContext.Clients.Group(asset.TenantId.ToString())
            //    .SendAsync("AssetCreated", new
            //    {
            //        asset.Id,
            //        CreationStatus = asset.CreationStatus.ToString()
            //    });

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving asset.");
            return false;
        }
    }

    private async Task<bool> HandleVideoAssetReady(MuxWebhookRequest request)
    {
        if (_env.IsDevelopment())
        {
            _logger.LogInformation($"Handle Mux Web Hook for : {request.Type}");
        }

        if (request.Data is null) return false;

        var asset = await _context.Assets.Include(a => a.MuxAsset)
            .FirstOrDefaultAsync(x => x.MuxAsset.MuxAssetId == request.Data.Id);

        if (asset == null)
        {
            _logger.LogWarning($"Asset with MuxAssetId {request.Data.Id} not found.");
            return false;
        }

        if (asset.CreationStatus == AssetCreationStatus.Ready) return true;

        asset.CreationStatus = AssetCreationStatus.Ready;
        asset.MuxAsset.Status = request.Data.Status;
        var tracks = request.Data.Tracks.Select(track => new MuxAssetTrackModel
            {
                TrackId = track.Id,
                Type = track.Type,
                MaxWidth = track.MaxWidth,
                MaxHeight = track.MaxHeight,
                MaxFrameRate = track.MaxFrameRate,
                MaxChannels = track.MaxChannels,
                Duration = track.Duration,
                LanguageCode = track.LanguageCode,
            })
            .ToList();

        asset.MuxAsset.AssetTracks = tracks;
        asset.MuxAsset.MaxResolutionTier = request.Data.MaxResolutionTier;
        asset.MuxAsset.MaxStoredFrameRate = request.Data.MaxStoredFrameRate;
        asset.MuxAsset.Duration = request.Data.Duration;
        asset.MuxAsset.ResolutionTier = request.Data.ResolutionTier;

        // TODO remove
        //await Task.Delay(5000);

        // TODO: Update client with new status via SignalR
        //await _videoHubNotificationService.NotifyCreationStatus(videoToUpdate.Id, videoToUpdate.StreamCreationStatus);
        // await _videoHubContext.Clients.All.SendAsync(VideoHub.ReceiveUpdateMethod, videoToUpdate.Id, videoToUpdate.StreamCreationStatus);

        try
        {
            await _context.SaveChangesAsync();
            if (_env.IsDevelopment())
            {
                _logger.LogInformation("Asset created successfully.");
            }

            // Notify the client using SignalR
            //await _hubContext.Clients.Group(asset.TenantId.ToString())
            //    .SendAsync("AssetReady", new
            //    {
            //        asset.Id,
            //        CreationStatus = asset.CreationStatus.ToString()
            //    });

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving asset.");
            return false;
        }
    }

    private async Task<bool> HandleVideoAssetErrored(MuxWebhookRequest request)
    {
        if (_env.IsDevelopment())
        {
            _logger.LogInformation("Handling 'video.asset.errored' event for Asset ID: {0}", request.Data.Id);
        }

        // Fetch the relevant Asset record
        var asset = await _context.Assets.FirstOrDefaultAsync(a => a.MuxAsset.MuxAssetId == request.Data.Id);
        if (asset == null)
        {
            _logger.LogWarning("Asset with Mux Asset ID {0} not found.", request.Data.Id);
            return false;
        }

        // Update the asset's status to errored
        asset.CreationStatus = AssetCreationStatus.Errored;
        //asset.ErrorMessage = request.Data.Error.Message;
        await _context.SaveChangesAsync();

        // Notify the client using SignalR
        //await _hubContext.Clients.Group(asset.TenantId.ToString())
        //    .SendAsync("AssetErrored", new
        //    {
        //        asset.Id,
        //        ErrorMessage = asset.ErrorMessage
        //    });

        _logger.LogInformation("Asset {0} marked as Errored with message: {1}", asset.Id, request.Data.Errors.Messages);
        return true;
    }

    private async Task<bool> HandleVideoAssetUpdated(MuxWebhookRequest request)
    {
        if (_env.IsDevelopment())
        {
            _logger.LogInformation("Handling 'video.asset.updated' event for Asset ID: {0}", request.Data.Id);
        }

        // Fetch the relevant Asset record
        var asset = await _context.Assets.FirstOrDefaultAsync(a => a.MuxAsset.MuxAssetId == request.Data.Id);
        if (asset == null)
        {
            _logger.LogWarning("Asset with Mux Asset ID {0} not found.", request.Data.Id);
            return false;
        }

        asset.MuxAsset.VideoQuality = request.Data.VideoQuality;
        asset.MuxAsset.MP4Support = request.Data.Mp4Support;
        asset.MuxAsset.MasterAccess = request.Data.MasterAccess;
        asset.MuxAsset.EncodingTier = request.Data.EncodingTier;
        asset.MuxAsset.MaxResolutionTier = request.Data.MaxResolutionTier;
        asset.MuxAsset.MaxStoredFrameRate = request.Data.MaxStoredFrameRate;
        asset.MuxAsset.Duration = request.Data.Duration;
        asset.MuxAsset.ResolutionTier = request.Data.ResolutionTier;

        await _context.SaveChangesAsync();

        // Notify the client using SignalR
        //await _hubContext.Clients.Group(asset.TenantId.ToString())
        //    .SendAsync("AssetUpdated", new
        //    {
        //        asset.Id,
        //        asset.Duration,
        //        asset.Resolution
        //    });

        _logger.LogInformation("Asset {0} updated with new details.", asset.Id);
        return true;
    }

    private async Task<bool> HandleVideoAssetDeleted(MuxWebhookRequest request)
    {
        if (_env.IsDevelopment())
        {
            _logger.LogInformation("Handling 'video.asset.deleted' event for Asset ID: {0}", request.Data.Id);
        }

        // Fetch the relevant Asset record
        var asset = await _context.Assets.FirstOrDefaultAsync(a => a.MuxAsset.MuxAssetId == request.Data.Id);
        if (asset == null)
        {
            _logger.LogWarning("Asset with Mux Asset ID {0} not found.", request.Data.Id);
            return false;
        }

        // Delete the asset record from the database
        _context.Assets.Remove(asset);
        await _context.SaveChangesAsync();

        // Notify the client using SignalR
        //await _hubContext.Clients.Group(asset.TenantId.ToString())
        //    .SendAsync("AssetDeleted", new
        //    {
        //        asset.Id
        //    });

        _logger.LogInformation("Asset {0} deleted successfully.", asset.Id);
        return true;
    }

    private async Task<bool> HandleVideoAssetLiveStreamCompleted(MuxWebhookRequest request)
    {
        if (_env.IsDevelopment())
        {
            _logger.LogInformation("Handling video.asset.live_stream_completed");
        }
        return true;
    }

    private async Task<bool> HandleVideoAssetStaticRenditionsReady(MuxWebhookRequest request)
    {
        if (_env.IsDevelopment())
        {
            _logger.LogInformation("Handling video.asset.static_renditions.ready");
        }
        return true;
    }

    private async Task<bool> HandleVideoAssetStaticRenditionsPreparing(MuxWebhookRequest request)
    {
        if (_env.IsDevelopment())
        {
            _logger.LogInformation("Handling video.asset.static_renditions.preparing");
        }
        
        return true;
    }

    private async Task<bool> HandleVideoAssetStaticRenditionsDeleted(MuxWebhookRequest request)
    {
        if (_env.IsDevelopment())
        {
            _logger.LogInformation("Handling video.asset.static_renditions.deleted");
        }
        return true;
    }

    private async Task<bool> HandleVideoAssetStaticRenditionsErrored(MuxWebhookRequest request)
    {
        if (_env.IsDevelopment())
        {
            _logger.LogInformation("Handling video.asset.static_renditions.errored");
        }
        return true;
    }

    private async Task<bool> HandleVideoAssetMasterReady(MuxWebhookRequest request)
    {
        if (_env.IsDevelopment())
        {
            _logger.LogInformation("Handling video.asset.master.ready");
        }
        return true;
    }

    private async Task<bool> HandleVideoAssetMasterPreparing(MuxWebhookRequest request)
    {
        if (_env.IsDevelopment())
        {
            _logger.LogInformation("Handling video.asset.master.preparing");
        }
        return true;
    }

    private async Task<bool> HandleVideoAssetMasterDeleted(MuxWebhookRequest request)
    {
        if (_env.IsDevelopment())
        {
            _logger.LogInformation("Handling video.asset.master.deleted");
        }
        return true;
    }

    private async Task<bool> HandleVideoAssetMasterErrored(MuxWebhookRequest request)
    {
        if (_env.IsDevelopment())
        {
            _logger.LogInformation("Handling video.asset.master.errored");
        }
        return true;
    }

    private async Task<bool> HandleVideoAssetTrackCreated(MuxWebhookRequest request)
    {
        if (_env.IsDevelopment())
        {
            _logger.LogInformation("Handling video.asset.track.created");
        }
        return true;
    }

    private async Task<bool> HandleVideoAssetTrackReady(MuxWebhookRequest request)
    {
        if (_env.IsDevelopment())
        {
            _logger.LogInformation("Handling video.asset.track.ready");
        }
        return true;
    }

    private async Task<bool> HandleVideoAssetTrackErrored(MuxWebhookRequest request)
    {
        if (_env.IsDevelopment())
        {
            _logger.LogInformation("Handling video.asset.track.errored");
        }
        return true;
    }

    private async Task<bool> HandleVideoAssetTrackDeleted(MuxWebhookRequest request)
    {
        if (_env.IsDevelopment())
        {
            _logger.LogInformation("Handling video.asset.track.deleted");
        }
        return true;
    }

    private async Task<bool> HandleVideoAssetWarning(MuxWebhookRequest request)
    {
        if (_env.IsDevelopment())
        {
            _logger.LogInformation("Handling video.asset.warning");
        }
        return true;
    }

    private async Task<bool> HandleVideoUploadAssetCreated(MuxWebhookRequest request)
    {
        if (_env.IsDevelopment())
        {
            _logger.LogInformation("Handling video.upload.asset_created");
        }
        return true;
    }

    private async Task<bool> HandleVideoUploadCancelled(MuxWebhookRequest request)
    {
        if (_env.IsDevelopment())
        {
            _logger.LogInformation("Handling video.upload.cancelled");
        }
        return true;
    }

    private async Task<bool> HandleVideoUploadCreated(MuxWebhookRequest request)
    {
        if (_env.IsDevelopment())
        {
            _logger.LogInformation("Handling video.upload.created");
        }
        return true;
    }

    private async Task<bool> HandleVideoUploadErrored(MuxWebhookRequest request)
    {
        if (_env.IsDevelopment())
        {
            _logger.LogInformation("Handling video.upload.errored");
        }
        return true;
    }

    private async Task<bool> HandleVideoLiveStreamCreated(MuxWebhookRequest request)
    {
        if (_env.IsDevelopment())
        {
            _logger.LogInformation("Handling video.live_stream.created");
        }
        return true;
    }

    private async Task<bool> HandleVideoLiveStreamConnected(MuxWebhookRequest request)
    {
        if (_env.IsDevelopment())
        {
            _logger.LogInformation("Handling video.live_stream.connected");
        }
        return true;
    }

    private async Task<bool> HandleVideoLiveStreamRecording(MuxWebhookRequest request)
    {
        if (_env.IsDevelopment())
        {
            _logger.LogInformation("Handling video.live_stream.recording");
        }
        return true;
    }

    private async Task<bool> HandleVideoLiveStreamActive(MuxWebhookRequest request)
    {
        if (_env.IsDevelopment())
        {
            _logger.LogInformation("Handling video.live_stream.active");
        }
        return true;
    }

    private async Task<bool> HandleVideoLiveStreamDisconnected(MuxWebhookRequest request)
    {
        if (_env.IsDevelopment())
        {
            _logger.LogInformation("Handling video.live_stream.disconnected");
        }
        return true;
    }

    private async Task<bool> HandleVideoLiveStreamIdle(MuxWebhookRequest request)
    {
        if (_env.IsDevelopment())
        {
            _logger.LogInformation("Handling video.live_stream.idle");
        }
        return true;
    }

    private async Task<bool> HandleVideoLiveStreamUpdated(MuxWebhookRequest request)
    {
        if (_env.IsDevelopment())
        {
            _logger.LogInformation("Handling video.live_stream.updated");
        }
        return true;
    }

    private async Task<bool> HandleVideoLiveStreamEnabled(MuxWebhookRequest request)
    {
        if (_env.IsDevelopment())
        {
            _logger.LogInformation("Handling video.live_stream.enabled");
        }
        return true;
    }

    private async Task<bool> HandleVideoLiveStreamDisabled(MuxWebhookRequest request)
    {
        if (_env.IsDevelopment())
        {
            _logger.LogInformation("Handling video.live_stream.disabled");
        }
        return true;
    }

    private async Task<bool> HandleVideoLiveStreamDeleted(MuxWebhookRequest request)
    {
        if (_env.IsDevelopment())
        {
            _logger.LogInformation("Handling video.live_stream.deleted");
        }
        return true;
    }

    private async Task<bool> HandleVideoLiveStreamWarning(MuxWebhookRequest request)
    {
        if (_env.IsDevelopment())
        {
            _logger.LogInformation("Handling video.live_stream.warning");
        }
        return true;
    }

    private async Task<bool> HandleVideoLiveStreamSimulcastTargetCreated(MuxWebhookRequest request)
    {
        if (_env.IsDevelopment())
        {
            _logger.LogInformation("Handling video.live_stream.simulcast_target.created");
        }
        return true;
    }

    private async Task<bool> HandleVideoLiveStreamSimulcastTargetIdle(MuxWebhookRequest request)
    {
        if (_env.IsDevelopment())
        {
            _logger.LogInformation("Handling video.live_stream.simulcast_target.idle");
        }
        return true;
    }

    private async Task<bool> HandleVideoLiveStreamSimulcastTargetStarting(MuxWebhookRequest request)
    {
        if (_env.IsDevelopment())
        {
            _logger.LogInformation("Handling video.live_stream.simulcast_target.starting");
        }
        return true;
    }

    private async Task<bool> HandleVideoLiveStreamSimulcastTargetBroadcasting(MuxWebhookRequest request)
    {
        if (_env.IsDevelopment())
        {
            _logger.LogInformation("Handling video.live_stream.simulcast_target.broadcasting");
        }
        return true;
    }

    private async Task<bool> HandleVideoLiveStreamSimulcastTargetErrored(MuxWebhookRequest request)
    {
        if (_env.IsDevelopment())
        {
            _logger.LogInformation("Handling video.live_stream.simulcast_target.errored");
        }
        return true;
    }

    private async Task<bool> HandleVideoLiveStreamSimulcastTargetDeleted(MuxWebhookRequest request)
    {
        if (_env.IsDevelopment())
        {
            _logger.LogInformation("Handling video.live_stream.simulcast_target.deleted");
        }
        return true;
    }

    private async Task<bool> HandleVideoLiveStreamSimulcastTargetUpdated(MuxWebhookRequest request)
    {
        if (_env.IsDevelopment())
        {
            _logger.LogInformation("Handling video.live_stream.simulcast_target.updated");
        }
        return true;
    }

    #endregion


    #region Helper Methods

    private string GetVideoImageUrlAtMux(string playbackId, string baseStreamUrl, int width, int height) =>
        $"{baseStreamUrl}/{playbackId}/thumbnail.png?width={width}&height={height}&fit_mode=pad";

    private string GetVideoAnimatedGifUrlAtMux(string playbackId, string baseStreamUrl, int width) =>
        $"{baseStreamUrl}/{playbackId}/animated.gif?width={width}";

    private string? GetStaticRenditionFileName(MuxWebhookRequest request)
    {
        string? fileName = request?.Data?.StaticRenditions?.Files?.Count switch
        {
            1 =>
                $"{request.Data.StaticRenditions.Files[0].Name}",
            2 =>
                $"{request.Data.StaticRenditions.Files[1].Name}",
            3 =>
                $"{request.Data.StaticRenditions.Files[2].Name}",
            4 =>
                $"{request.Data.StaticRenditions.Files[3].Name}",
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

    private bool HasLiveStreamDataChanged(LiveStreamModel directUpload, MuxWebhookRequest request)
    {
        if (request?.Data == null) return false;

        if (directUpload.StreamKey != request.Data.StreamKey) return true;
        if (directUpload.LatencyMode != MapToLatencyMode(request.Data.LatencyMode)) return true;
        if (directUpload.ReconnectWindow != request.Data.ReconnectWindow) return true;
        if (directUpload.MaxContinuousDuration != request.Data.MaxContinuousDuration) return true;

        return false;
    }

    #endregion
}

