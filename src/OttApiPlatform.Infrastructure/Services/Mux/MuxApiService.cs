using Microsoft.Extensions.Logging;
using Mux.Csharp.Sdk.Api;
using Mux.Csharp.Sdk.Client;
using Mux.Csharp.Sdk.Model;
using OttApiPlatform.Application.Common.Contracts;
using OttApiPlatform.Application.Common.Contracts.Mux;
using OttApiPlatform.Application.Common.Models.Mux;
using OttApiPlatform.Domain.Entities.ContentManagement;
using OttApiPlatform.Domain.Entities.Mux;
using System.Diagnostics;
using System.Net.Http;
using LiveStreamStatus = Mux.Csharp.Sdk.Model.LiveStreamStatus;

namespace OttApiPlatform.Infrastructure.Services.Mux;

/// <summary>
/// Handles Mux API calls and webhook events.
/// Ref: https://github.com/muxinc/mux-csharp
/// Ref: https://www.nuget.org/packages/Mux.Csharp.Sdk
/// Ref: https://docs.mux.com/api-reference
/// Ref: https://docs.mux.com/webhook-reference
/// </summary>
public class MuxApiService : IMuxApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<MuxApiService> _logger;

    private readonly IApplicationDbContext _context;
    private readonly ICryptoService _encryptionService;
    private readonly ITenantResolver _tenantResolver;

    private readonly MuxOptions _muxOptions;

    private MuxSettingsModel _settings;
    private Configuration _muxConfig;

    private AssetsApi _assetsApi;
    private LiveStreamsApi _liveStreamsApi;
    private DeliveryUsageApi _deliveryUsageApi;
    private SigningKeysApi _signingKeysApi;
    private VideoViewsApi _videoViewsApi;
    private SpacesApi _spacesApi;
    private DRMConfigurationsApi _drmConfigurationsApi;
    private ErrorsApi _errorsApi;
    private MetricsApi _metricsApi;
    private PlaybackIDApi _playbackIdApi;
    private PlaybackRestrictionsApi _playbackRestrictionsApi;
    private MonitoringApi _monitoringApi;
    private RealTimeApi _realTimeApi;
    private TranscriptionVocabulariesApi _transcriptionVocabulariesApi;

    private readonly string _assetsEndpoint;

    public MuxApiService(HttpClient httpClient, IConfigReaderService configReaderService, IApplicationDbContext context, 
        ICryptoService encryptionService, ITenantResolver tenantResolver, ILogger<MuxApiService> logger)
    {
        _httpClient = httpClient;
        _muxOptions = configReaderService.GetMuxOptions();
        _assetsEndpoint = $"{_muxOptions.BaseUrl}{_muxOptions.AssetPath}";

        _context = context;
        _encryptionService = encryptionService;
        _tenantResolver = tenantResolver;

        _logger = logger;
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

        _assetsApi = new AssetsApi(_muxConfig);
        _liveStreamsApi = new LiveStreamsApi(_muxConfig);
        _deliveryUsageApi = new DeliveryUsageApi(_muxConfig);
        _signingKeysApi = new SigningKeysApi(_muxConfig);
        _videoViewsApi = new VideoViewsApi(_muxConfig);
        _spacesApi = new SpacesApi(_muxConfig);
        _drmConfigurationsApi = new DRMConfigurationsApi(_muxConfig);
        _errorsApi = new ErrorsApi(_muxConfig);
        _metricsApi = new MetricsApi(_muxConfig);
        _playbackIdApi = new PlaybackIDApi(_muxConfig);
        _playbackRestrictionsApi = new PlaybackRestrictionsApi(_muxConfig);
        _monitoringApi = new MonitoringApi(_muxConfig);
        _realTimeApi = new RealTimeApi(_muxConfig);
        _transcriptionVocabulariesApi = new TranscriptionVocabulariesApi(_muxConfig);

        //var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_settings.TokenId}:{_settings.TokenSecret}"));
        //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);
        //_httpClient.BaseAddress = new Uri(_muxOptions.BaseUrl);
    }

    #region Mux Assets Api calls

    // Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/AssetsApi.md

    /// <summary>
    /// Create a new Mux asset
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/CreateAssetRequest.md
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/AssetResponse.md
    /// </summary>
    /// <param name="inputUrl"></param>
    /// <param name="videoQuality"></param>
    /// <returns></returns>
    public async Task<AssetResponse> CreateAssetAsync(string inputUrl, CreateAssetRequest.VideoQualityEnum? videoQuality)
    {
        var createAssetRequest = new CreateAssetRequest
        {
            Passthrough = _tenantResolver.GetTenantId().ToString(),
            Input =
            [
                new InputSettings
                {
                    Url = inputUrl
                }
            ],
            PlaybackPolicy =
            [
                PlaybackPolicy.Public,
                PlaybackPolicy.Signed
            ],
            VideoQuality = videoQuality ?? CreateAssetRequest.VideoQualityEnum.Basic
        };

        try
        {
            return await _assetsApi.CreateAssetAsync(createAssetRequest);
        }
        catch (ApiException e)
        {
            _logger.LogError("Exception when calling AssetsApi.CreateAsset: " + e.Message);
            _logger.LogError("Status Code: " + e.ErrorCode);
            _logger.LogError(e.StackTrace);
            throw;
        }
    }

    /// <summary>
    /// Retrieve an existing Mux asset
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/AssetResponse.md
    /// </summary>
    /// <param name="assetId"></param>
    /// <returns></returns>
    public async Task<AssetResponse> GetAssetAsync(string assetId)
    {
        try
        {
            return await _assetsApi.GetAssetAsync(assetId);
        }
        catch (ApiException ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }

    /// <summary>
    /// List all Mux assets
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/ListAssetsResponse.md
    /// </summary>
    /// <param name="liveStreamId">Filter response to return all the assets for this live stream only (optional) </param>
    /// <param name="uploadId">Filter response to return an asset created from this direct upload only (optional) </param>
    /// <param name="limit">Number of items to include in the response (optional)</param>
    /// <param name="page">Offset by this many pages, of the size of `limit` (optional)</param>
    /// <returns></returns>
    public async Task<ListAssetsResponse> ListAssetsAsync(string liveStreamId, string uploadId, int limit = 25, int page = 1)
    {
        try
        {
            return await _assetsApi.ListAssetsAsync(limit, page, liveStreamId, uploadId);
        }
        catch (ApiException e)
        {
            _logger.LogError("Exception when calling AssetsApi.ListAssets: " + e.Message);
            _logger.LogError("Status Code: " + e.ErrorCode);
            _logger.LogError(e.StackTrace);
            throw;
        }
    }

    /// <summary>
    /// Update Mux asset
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/UpdateAssetRequest.md
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/AssetResponse.md
    /// </summary>
    /// <param name="assetId"></param>
    /// <param name="newPassthrough"></param>
    /// <returns></returns>
    public async Task<AssetResponse> UpdateAssetAsync(string assetId, string newPassthrough)
    {
        var updateAssetRequest = new UpdateAssetRequest
        {
            Passthrough =  newPassthrough
        };

        try
        {
            return await _assetsApi.UpdateAssetAsync(assetId, updateAssetRequest);
        }
        catch (ApiException e)
        {
            _logger.LogError("Exception when calling AssetsApi.UpdateAsset: " + e.Message);
            _logger.LogError("Status Code: " + e.ErrorCode);
            _logger.LogError(e.StackTrace);
            throw;
        }
    }

    /// <summary>
    /// Delete a Mux asset
    /// </summary>
    /// <param name="assetId"></param>
    /// <returns></returns>
    public async Task DeleteAssetAsync(string assetId)
    {
        try
        {
            await _assetsApi.DeleteAssetAsync(assetId);
        }
        catch (ApiException e)
        {
            _logger.LogError("Exception when calling AssetsApi.DeleteAsset: " + e.Message);
            _logger.LogError("Status Code: " + e.ErrorCode);
            _logger.LogError(e.StackTrace);
        }
    }

    /// <summary>
    /// Create playback ID for an asset
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/CreatePlaybackIDRequest.md
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/CreatePlaybackIDResponse.md
    /// </summary>
    /// <param name="assetId"></param>
    /// <param name="policy"></param>
    /// <param name="drmConfigurationId"></param>
    /// <returns></returns>
    public async Task<CreatePlaybackIDResponse> CreatePlaybackIdAsync(string assetId, PlaybackPolicy policy = PlaybackPolicy.Public, string drmConfigurationId = null)
    {
        var createPlaybackIdRequest = new CreatePlaybackIDRequest
        {
            Policy = policy,
            DrmConfigurationId = drmConfigurationId
        }; 

        try
        {
            return await _assetsApi.CreateAssetPlaybackIdAsync(assetId, createPlaybackIdRequest);
        }
        catch (ApiException e)
        {
            _logger.LogError("Exception when calling AssetsApi.CreateAssetPlaybackId: " + e.Message);
            _logger.LogError("Status Code: " + e.ErrorCode);
            _logger.LogError(e.StackTrace);
            throw;
        }
    }

    /// <summary>
    /// Get asset playback id
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/GetAssetPlaybackIDResponse.md
    /// </summary>
    /// <param name="assetId"></param>
    /// <param name="playbackId"></param>
    /// <returns></returns>
    public async Task<GetAssetPlaybackIDResponse> GetAssetPlaybackIdAsync(string assetId, string playbackId)
    {
        try
        {
            return await _assetsApi.GetAssetPlaybackIdAsync(assetId, playbackId);
        }
        catch (ApiException e)
        {
            _logger.LogError("Exception when calling AssetsApi.GetAssetPlaybackId: " + e.Message);
            _logger.LogError("Status Code: " + e.ErrorCode);
            _logger.LogError(e.StackTrace);
            throw;
        }
    }

    /// <summary>
    /// Delete playback ID
    /// </summary>
    /// <param name="assetId"></param>
    /// <param name="playbackId"></param>
    /// <returns></returns>
    public async Task DeletePlaybackIdAsync(string assetId, string playbackId)
    {
        try
        {
            await _assetsApi.DeleteAssetPlaybackIdAsync(assetId, playbackId);
        }
        catch (ApiException e)
        {
            _logger.LogError("Exception when calling AssetsApi.DeleteAssetPlaybackId: " + e.Message);
            _logger.LogError("Status Code: " + e.ErrorCode);
            _logger.LogError(e.StackTrace);
        }
    }

    /// <summary>
    /// Create a new asset track
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/CreateTrackRequest.md
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/CreateTrackResponse.md
    /// </summary>
    /// <param name="assetId"></param>
    /// <returns></returns>
    public async Task<CreateTrackResponse> CreateAssetTrackAsync(string assetId)
    {
        var createTrackRequest = new CreateTrackRequest();

        try
        {
            return await _assetsApi.CreateAssetTrackAsync(assetId, createTrackRequest);
        }
        catch (ApiException e)
        {
            _logger.LogError("Exception when calling AssetsApi.CreateAssetTrack: " + e.Message);
            _logger.LogError("Status Code: " + e.ErrorCode);
            _logger.LogError(e.StackTrace);
            throw;
        }
    }

    /// <summary>
    /// Delete an asset track
    /// </summary>
    /// <param name="assetId"></param>
    /// <param name="trackId"></param>
    /// <returns></returns>
    public async Task DeleteAssetTrackAsync(string assetId, string trackId)
    {
        try
        {
            await _assetsApi.DeleteAssetTrackAsync(assetId, trackId);
        }
        catch (ApiException e)
        {
            _logger.LogError("Exception when calling AssetsApi.DeleteAssetTrack: " + e.Message);
            _logger.LogError("Status Code: " + e.ErrorCode);
            _logger.LogError(e.StackTrace);
        }
    }

    /// <summary>
    /// Generate subtitles for an asset track
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/GenerateTrackSubtitlesRequest.md
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/GenerateTrackSubtitlesResponse.md
    /// </summary>
    /// <param name="assetId"></param>
    /// <param name="trackId"></param>
    /// <param name="subtitleSettings"></param>
    /// <returns></returns>
    public async Task<GenerateTrackSubtitlesResponse> GenerateAssetTrackSubtitlesAsync(string assetId, string trackId, List<AssetGeneratedSubtitleSettings> subtitleSettings)
    {
        var generateTrackSubtitlesRequest = new GenerateTrackSubtitlesRequest(subtitleSettings);

        try
        {
            return await _assetsApi.GenerateAssetTrackSubtitlesAsync(assetId, trackId, generateTrackSubtitlesRequest);
        }
        catch (ApiException e)
        {
            _logger.LogError("Exception when calling AssetsApi.GenerateAssetTrackSubtitles: " + e.Message);
            _logger.LogError("Status Code: " + e.ErrorCode);
            _logger.LogError(e.StackTrace);
            throw;
        }
    }

    /// <summary>
    /// Get input information for an asset
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/GetAssetInputInfoResponse.md
    /// </summary>
    /// <param name="assetId"></param>
    /// <returns></returns>
    public async Task<GetAssetInputInfoResponse> GetAssetInputInfoAsync(string assetId)
    {
        try
        {
            return await _assetsApi.GetAssetInputInfoAsync(assetId);
        }
        catch (ApiException e)
        {
            _logger.LogError("Exception when calling AssetsApi.GetAssetInputInfo: " + e.Message);
            _logger.LogError("Status Code: " + e.ErrorCode);
            _logger.LogError(e.StackTrace);
            throw;
        }
    }

    /// <summary>
    /// Update master access for an asset
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/UpdateAssetMasterAccessRequest.md
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/AssetResponse.md
    /// </summary>
    /// <param name="assetId"></param>
    /// <param name="masterAccess"></param>
    /// <returns></returns>
    public async Task<AssetResponse> UpdateAssetMasterAccessAsync(string assetId, UpdateAssetMasterAccessRequest.MasterAccessEnum masterAccess)
    {
        var updateAssetMasterAccessRequest = new UpdateAssetMasterAccessRequest
        {
            MasterAccess = masterAccess
        };

        try
        {
            return await _assetsApi.UpdateAssetMasterAccessAsync(assetId, updateAssetMasterAccessRequest);
        }
        catch (ApiException e)
        {
            _logger.LogError("Exception when calling AssetsApi.UpdateAssetMasterAccess: " + e.Message);
            _logger.LogError("Status Code: " + e.ErrorCode);
            _logger.LogError(e.StackTrace);
            throw;
        }
    }

    /// <summary>
    /// Update MP4 support for an asset
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/UpdateAssetMP4SupportRequest.md
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/AssetResponse.md
    /// </summary>
    /// <param name="assetId"></param>
    /// <param name="mp4Support"></param>
    /// <returns></returns>
    public async Task<AssetResponse> UpdateAssetMp4SupportAsync(string assetId, UpdateAssetMP4SupportRequest.Mp4SupportEnum mp4Support)
    {
        var updateAssetMp4SupportRequest = new UpdateAssetMP4SupportRequest
        {
            Mp4Support = mp4Support
        };

        try
        {
            return await _assetsApi.UpdateAssetMp4SupportAsync(assetId, updateAssetMp4SupportRequest);
        }
        catch (ApiException e)
        {
            _logger.LogError("Exception when calling AssetsApi.UpdateAssetMp4Support: " + e.Message);
            _logger.LogError("Status Code: " + e.ErrorCode);
            _logger.LogError(e.StackTrace);
            throw;
        }
    }

    #endregion

    #region Mux Live Stream API calls

    // Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/LiveStreamsApi.md

    /// <summary>
    /// Create a new live stream
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/CreateLiveStreamRequest.md
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/LiveStreamResponse.md
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<LiveStreamResponse> CreateLiveStreamAsync(CreateLiveStreamRequest request)
    {
        try
        {
            return await _liveStreamsApi.CreateLiveStreamAsync(request);
        }
        catch (ApiException e)
        {
            _logger.LogError($"Exception when calling LiveStreamsApi.CreateLiveStream: {e.Message}");
            throw;
        }
    }

    /// <summary>
    /// Create a new live stream playback ID
    /// Ref:https://github.com/muxinc/mux-csharp/blob/main/docs/CreatePlaybackIDRequest.md
    /// Ref:https://github.com/muxinc/mux-csharp/blob/main/docs/CreatePlaybackIDResponse.md
    /// </summary>
    /// <param name="liveStreamId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<CreatePlaybackIDResponse> CreateLiveStreamPlaybackIdAsync(string liveStreamId, CreatePlaybackIDRequest request)
    {
        try
        {
            return await _liveStreamsApi.CreateLiveStreamPlaybackIdAsync(liveStreamId, request);
        }
        catch (ApiException e)
        {
            _logger.LogError($"Exception when calling LiveStreamsApi.CreateLiveStreamPlaybackId: {e.Message}");
            throw;
        }
    }

    /// <summary>
    /// Get live stream details
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/LiveStreamResponse.md
    /// </summary>
    /// <param name="liveStreamId"></param>
    /// <returns></returns>
    public async Task<LiveStreamResponse> GetLiveStreamAsync(string liveStreamId)
    {
        try
        {
            return await _liveStreamsApi.GetLiveStreamAsync(liveStreamId);
        }
        catch (ApiException e)
        {
            _logger.LogError($"Exception when calling LiveStreamsApi.GetLiveStream: {e.Message}");
            throw;
        }
    }

    /// <summary>
    /// List all live streams
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/ListLiveStreamsResponse.md
    /// </summary>
    /// <param name="limit"></param>
    /// <param name="page"></param>
    /// <param name="streamKey"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    public async Task<ListLiveStreamsResponse> ListLiveStreamsAsync(int? limit = null, int? page = null, string streamKey = null, LiveStreamStatus? status = null)
    {
        try
        {
            return await _liveStreamsApi.ListLiveStreamsAsync(limit, page, streamKey, status);
        }
        catch (ApiException e)
        {
            _logger.LogError($"Exception when calling LiveStreamsApi.ListLiveStreams: {e.Message}");
            throw;
        }
    }

    /// <summary>
    /// Create a new simulcast target for a live stream
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/CreateSimulcastTargetRequest.md
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/SimulcastTargetResponse.md
    /// </summary>
    /// <param name="liveStreamId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<SimulcastTargetResponse> CreateLiveStreamSimulcastTargetAsync(string liveStreamId, CreateSimulcastTargetRequest request)
    {
        try
        {
            return await _liveStreamsApi.CreateLiveStreamSimulcastTargetAsync(liveStreamId, request);
        }
        catch (ApiException e)
        {
            _logger.LogError($"Exception when calling LiveStreamsApi.CreateLiveStreamSimulcastTarget: {e.Message}");
            throw;
        }
    }

    /// <summary>
    /// Delete a live stream
    /// </summary>
    /// <param name="liveStreamId"></param>
    /// <returns></returns>
    public async Task DeleteLiveStreamAsync(string liveStreamId)
    {
        try
        {
            await _liveStreamsApi.DeleteLiveStreamAsync(liveStreamId);
        }
        catch (ApiException e)
        {
            _logger.LogError($"Exception when calling LiveStreamsApi.DeleteLiveStream: {e.Message}");
            throw;
        }
    }

    /// <summary>
    /// Enable a live stream
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/EnableLiveStreamResponse.md
    /// </summary>
    /// <param name="liveStreamId"></param>
    /// <returns></returns>
    public async Task<EnableLiveStreamResponse> EnableLiveStreamAsync(string liveStreamId)
    {
        try
        {
            return await _liveStreamsApi.EnableLiveStreamAsync(liveStreamId);
        }
        catch (ApiException e)
        {
            _logger.LogError($"Exception when calling LiveStreamsApi.EnableLiveStream: {e.Message}");
            throw;
        }
    }

    /// <summary>
    /// Disable a live stream
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/DisableLiveStreamResponse.md
    /// </summary>
    /// <param name="liveStreamId"></param>
    /// <returns></returns>
    public async Task<DisableLiveStreamResponse> DisableLiveStreamAsync(string liveStreamId)
    {
        try
        {
            return await _liveStreamsApi.DisableLiveStreamAsync(liveStreamId);
        }
        catch (ApiException e)
        {
            _logger.LogError($"Exception when calling LiveStreamsApi.DisableLiveStream: {e.Message}");
            throw;
        }
    }

    /// <summary>
    /// Reset stream key for a live stream
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/LiveStreamResponse.md
    /// </summary>
    /// <param name="liveStreamId"></param>
    /// <returns></returns>
    public async Task<LiveStreamResponse> ResetStreamKeyAsync(string liveStreamId)
    {
        try
        {
            return await _liveStreamsApi.ResetStreamKeyAsync(liveStreamId);
        }
        catch (ApiException e)
        {
            _logger.LogError($"Exception when calling LiveStreamsApi.ResetStreamKey: {e.Message}");
            throw;
        }
    }

    /// <summary>
    /// Signal live stream complete
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/SignalLiveStreamCompleteResponse.md
    /// </summary>
    /// <param name="liveStreamId"></param>
    /// <returns></returns>
    public async Task<SignalLiveStreamCompleteResponse> SignalLiveStreamCompleteAsync(string liveStreamId)
    {
        try
        {
            return await _liveStreamsApi.SignalLiveStreamCompleteAsync(liveStreamId);
        }
        catch (ApiException e)
        {
            _logger.LogError($"Exception when calling LiveStreamsApi.SignalLiveStreamComplete: {e.Message}");
            throw;
        }
    }

    /// <summary>
    /// Update a live stream
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/UpdateLiveStreamRequest.md
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/LiveStreamResponse.md
    /// </summary>
    /// <param name="liveStreamId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<LiveStreamResponse> UpdateLiveStreamAsync(string liveStreamId, UpdateLiveStreamRequest request)
    {
        try
        {
            return await _liveStreamsApi.UpdateLiveStreamAsync(liveStreamId, request);
        }
        catch (ApiException e)
        {
            _logger.LogError($"Exception when calling LiveStreamsApi.UpdateLiveStream: {e.Message}");
            throw;
        }
    }

    #endregion

    #region Mux Signing Keys API calls

    // Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/SigningKeysApi.md

    /// <summary>
    /// Create a new signing key
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/SigningKeyResponse.md
    /// </summary>
    /// <returns></returns>
    public async Task<SigningKeyResponse> CreateSigningKeyAsync()
    {
        try
        {
            return await _signingKeysApi.CreateSigningKeyAsync();
        }
        catch (ApiException e)
        {
            _logger.LogError($"Exception when calling SigningKeysApi.CreateSigningKey: {e.Message}");
            throw;
        }
    }

    /// <summary>
    /// Delete a signing key
    /// </summary>
    /// <param name="signingKeyId"></param>
    /// <returns></returns>
    public async Task DeleteSigningKeyAsync(string signingKeyId)
    {
        try
        {
            await _signingKeysApi.DeleteSigningKeyAsync(signingKeyId);
        }
        catch (ApiException e)
        {
            _logger.LogError($"Exception when calling SigningKeysApi.DeleteSigningKey: {e.Message}");
            throw;
        }
    }

    /// <summary>
    /// Get a signing key
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/SigningKeyResponse.md
    /// </summary>
    /// <param name="signingKeyId"></param>
    /// <returns></returns>
    public async Task<SigningKeyResponse> GetSigningKeyAsync(string signingKeyId)
    {
        try
        {
            return await _signingKeysApi.GetSigningKeyAsync(signingKeyId);
        }
        catch (ApiException e)
        {
            _logger.LogError($"Exception when calling SigningKeysApi.GetSigningKey: {e.Message}");
            throw;
        }
    }

    /// <summary>
    /// List all signing keys
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/ListSigningKeysResponse.md
    /// </summary>
    /// <param name="limit"></param>
    /// <param name="page"></param>
    /// <returns></returns>
    public async Task<ListSigningKeysResponse> ListSigningKeysAsync(int? limit = null, int? page = null)
    {
        try
        {
            return await _signingKeysApi.ListSigningKeysAsync(limit, page);
        }
        catch (ApiException e)
        {
            _logger.LogError($"Exception when calling SigningKeysApi.ListSigningKeys: {e.Message}");
            throw;
        }
    }


    #endregion

    #region Mux Delivery Usage API calls

    // Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/DeliveryUsageApi.md

    /// <summary>
    /// List delivery usage
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/ListDeliveryUsageResponse.md
    /// </summary>
    /// <param name="assetId">Filter response to return delivery usage for this asset only. You cannot specify both the `asset_id` and `live_stream_id` parameters together. (optional)</param>
    /// <param name="liveStreamId">Filter response to return delivery usage for assets for this live stream. You cannot specify both the `asset_id` and `live_stream_id` parameters together. (optional) </param>
    /// <param name="timeframe">Time window to get delivery usage information. timeframe[0] indicates the start time, timeframe[1] indicates the end time in seconds since the Unix epoch. Default time window is 1 hour representing usage from 13th to 12th hour from when the request is made. (optional) </param>
    /// <param name="page">Offset by this many pages, of the size of `limit` (optional) </param>
    /// <param name="limit">Number of items to include in the response (optional)</param>
    /// <returns></returns>
    public async Task<ListDeliveryUsageResponse> ListDeliveryUsageAsync(string assetId, string liveStreamId, List<string> timeframe = null, int page = 1, int limit = 100)
    {
        try
        {
            return await _deliveryUsageApi.ListDeliveryUsageAsync(page, limit, assetId, liveStreamId, timeframe);
        }
        catch (ApiException e)
        {
            _logger.LogError("Exception when calling DeliveryUsageApi.ListDeliveryUsage: " + e.Message);
            _logger.LogError("Status Code: " + e.ErrorCode);
            _logger.LogError(e.StackTrace);
            throw;
        }
    }

    #endregion

    #region Mux Video Views API calls

    // Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/VideoViewsApi.md

    /// <summary>
    /// Get video view
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/VideoViewResponse.md
    /// </summary>
    /// <param name="videoViewId"></param>
    /// <returns></returns>
    public async Task<VideoViewResponse> GetVideoViewAsync(string videoViewId)
    {
        try
        {
            return await _videoViewsApi.GetVideoViewAsync(videoViewId);
        }
        catch (ApiException e)
        {
            _logger.LogError($"Exception when calling VideoViewsApi.GetVideoView: {e.Message}");
            throw;
        }
    }

    /// <summary>
    /// List video views
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/ListVideoViewsResponse.md
    /// </summary>
    /// <param name="limit"></param>
    /// <param name="page"></param>
    /// <param name="viewerId"></param>
    /// <param name="errorId"></param>
    /// <param name="orderDirection"></param>
    /// <param name="filters"></param>
    /// <param name="metricFilters"></param>
    /// <param name="timeframe"></param>
    /// <returns></returns>
    public async Task<ListVideoViewsResponse> ListVideoViewsAsync(
        int? limit = null,
        int? page = null,
        string viewerId = null,
        int? errorId = null,
        string orderDirection = null,
        List<string> filters = null,
        List<string> metricFilters = null,
        List<string> timeframe = null)
    {
        try
        {
            return await _videoViewsApi.ListVideoViewsAsync(limit, page, viewerId, errorId, orderDirection, filters, metricFilters, timeframe);
        }
        catch (ApiException e)
        {
            _logger.LogError($"Exception when calling VideoViewsApi.ListVideoViews: {e.Message}");
            throw;
        }
    }

    #endregion

    #region Mux DRM Configurations Api calls

    // Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/DRMConfigurationsApi.md

    /// <summary>
    /// Create a new DRM configuration
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/DRMConfigurationResponse.md
    /// </summary>
    /// <param name="drmConfigurationId"></param>
    /// <returns></returns>
    public async Task<DRMConfigurationResponse> GetDrmConfigurationAsync(string drmConfigurationId)
    {
        try
        {
            return await _drmConfigurationsApi.GetDrmConfigurationAsync(drmConfigurationId);
        }
        catch (ApiException e)
        {
            _logger.LogError($"Exception when calling DRMConfigurationsApi.GetDrmConfiguration: {e.Message}");
            throw;
        }
    }

    /// <summary>
    /// List all DRM configurations
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/ListDRMConfigurationsResponse.md
    /// </summary>
    /// <param name="page"></param>
    /// <param name="limit"></param>
    /// <returns></returns>
    public async Task<ListDRMConfigurationsResponse> ListDrmConfigurationsAsync(int? page = null, int? limit = null)
    {
        try
        {
            return await _drmConfigurationsApi.ListDrmConfigurationsAsync(page, limit);
        }
        catch (ApiException e)
        {
            _logger.LogError($"Exception when calling DRMConfigurationsApi.ListDrmConfigurations: {e.Message}");
            throw;
        }
    }

    #endregion

    #region Mux Errors Api calls

    // Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/ErrorsApi.md

    /// <summary>
    /// Get error details
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/ListErrorsResponse.md
    /// </summary>
    /// <param name="filters"></param>
    /// <param name="metricFilters"></param>
    /// <param name="timeframe"></param>
    /// <returns></returns>
    public async Task<ListErrorsResponse> ListErrorsAsync(
        List<string> filters = null,
        List<string> metricFilters = null,
        List<string> timeframe = null)
    {
        try
        {
            return await _errorsApi.ListErrorsAsync(filters, metricFilters, timeframe);
        }
        catch (ApiException e)
        {
            _logger.LogError($"Exception when calling ErrorsApi.ListErrors: {e.Message}");
            throw;
        }
    }

    #endregion

    #region Mux Metrics Api calls

    // Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/MetricsApi.md

    /// <summary>
    /// Get metric details
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/GetMetricTimeseriesDataResponse.md
    /// </summary>
    /// <param name="metricId"></param>
    /// <param name="timeframe"></param>
    /// <param name="filters"></param>
    /// <param name="metricFilters"></param>
    /// <param name="measurement"></param>
    /// <param name="orderDirection"></param>
    /// <param name="groupBy"></param>
    /// <returns></returns>
    public async Task<GetMetricTimeseriesDataResponse> GetMetricTimeseriesDataAsync(
        string metricId,
        List<string> timeframe = null,
        List<string> filters = null,
        List<string> metricFilters = null,
        string measurement = null,
        string orderDirection = null,
        string groupBy = null)
    {
        try
        {
            return await _metricsApi.GetMetricTimeseriesDataAsync(metricId, timeframe, filters, metricFilters, measurement, orderDirection, groupBy);
        }
        catch (ApiException e)
        {
            _logger.LogError($"Exception when calling MetricsApi.GetMetricTimeseriesData: {e.Message}");
            throw;
        }
    }

    /// <summary>
    /// Get overall values for a metric
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/GetOverallValuesResponse.md
    /// </summary>
    /// <param name="metricId"></param>
    /// <param name="timeframe"></param>
    /// <param name="filters"></param>
    /// <param name="metricFilters"></param>
    /// <param name="measurement"></param>
    /// <returns></returns>
    public async Task<GetOverallValuesResponse> GetOverallValuesAsync(
        string metricId,
        List<string> timeframe = null,
        List<string> filters = null,
        List<string> metricFilters = null,
        string measurement = null)
    {
        try
        {
            return await _metricsApi.GetOverallValuesAsync(metricId, timeframe, filters, metricFilters, measurement);
        }
        catch (ApiException e)
        {
            _logger.LogError($"Exception when calling MetricsApi.GetOverallValues: {e.Message}");
            throw;
        }
    }

    /// <summary>
    /// List all metric values
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/ListAllMetricValuesResponse.md
    /// </summary>
    /// <param name="timeframe"></param>
    /// <param name="filters"></param>
    /// <param name="metricFilters"></param>
    /// <param name="dimension"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public async Task<ListAllMetricValuesResponse> ListAllMetricValuesAsync(
        List<string> timeframe = null,
        List<string> filters = null,
        List<string> metricFilters = null,
        string dimension = null,
        string value = null)
    {
        try
        {
            return await _metricsApi.ListAllMetricValuesAsync(timeframe, filters, metricFilters, dimension, value);
        }
        catch (ApiException e)
        {
            _logger.LogError($"Exception when calling MetricsApi.ListAllMetricValues: {e.Message}");
            throw;
        }
    }

    /// <summary>
    /// List breakdown values for a metric
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/ListBreakdownValuesResponse.md
    /// </summary>
    /// <param name="metricId"></param>
    /// <param name="groupBy"></param>
    /// <param name="measurement"></param>
    /// <param name="filters"></param>
    /// <param name="metricFilters"></param>
    /// <param name="limit"></param>
    /// <param name="page"></param>
    /// <param name="orderBy"></param>
    /// <param name="orderDirection"></param>
    /// <param name="timeframe"></param>
    /// <returns></returns>
    public async Task<ListBreakdownValuesResponse> ListBreakdownValuesAsync(
        string metricId,
        string groupBy = null,
        string measurement = null,
        List<string> filters = null,
        List<string> metricFilters = null,
        int? limit = null,
        int? page = null,
        string orderBy = null,
        string orderDirection = null,
        List<string> timeframe = null)
    {
        try
        {
            return await _metricsApi.ListBreakdownValuesAsync(metricId, groupBy, measurement, filters, metricFilters, limit, page, orderBy, orderDirection, timeframe);
        }
        catch (ApiException e)
        {
            _logger.LogError($"Exception when calling MetricsApi.ListBreakdownValues: {e.Message}");
            throw;
        }
    }

    /// <summary>
    /// List insights for a metric
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/ListBreakdownValuesResponse.md
    /// </summary>
    /// <param name="metricId"></param>
    /// <param name="measurement"></param>
    /// <param name="orderDirection"></param>
    /// <param name="timeframe"></param>
    /// <param name="filters"></param>
    /// <param name="metricFilters"></param>
    /// <returns></returns>
    public async Task<ListInsightsResponse> ListInsightsAsync(
        string metricId,
        string measurement = null,
        string orderDirection = null,
        List<string> timeframe = null,
        List<string> filters = null,
        List<string> metricFilters = null)
    {
        try
        {
            return await _metricsApi.ListInsightsAsync(metricId, measurement, orderDirection, timeframe, filters, metricFilters);
        }
        catch (ApiException e)
        {
            _logger.LogError($"Exception when calling MetricsApi.ListInsights: {e.Message}");
            throw;
        }
    }

    #endregion

    #region Mux Playback ID Api calls

    // Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/PlaybackIDApi.md

    /// <summary>
    /// Get asset or live stream ID
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/GetAssetOrLiveStreamIdResponse.md
    /// </summary>
    /// <param name="playbackId"></param>
    /// <returns></returns>
    public async Task<GetAssetOrLiveStreamIdResponse> GetAssetOrLivestreamIdAsync(string playbackId)
    {
        try
        {
            return await _playbackIdApi.GetAssetOrLivestreamIdAsync(playbackId);
        }
        catch (ApiException e)
        {
            _logger.LogError($"Exception when calling PlaybackIDApi.GetAssetOrLivestreamId: {e.Message}");
            throw;
        }
    }

    #endregion

    #region Mux Playback Restrictions Api calls

    // Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/PlaybackRestrictionsApi.md

    /// <summary>
    /// Create a new playback restriction
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/CreatePlaybackRestrictionRequest.md
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/PlaybackRestrictionResponse.md
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<PlaybackRestrictionResponse> CreatePlaybackRestrictionAsync(CreatePlaybackRestrictionRequest request)
    {
        try
        {
            return await _playbackRestrictionsApi.CreatePlaybackRestrictionAsync(request);
        }
        catch (ApiException e)
        {
            _logger.LogError($"Exception when calling PlaybackRestrictionsApi.CreatePlaybackRestriction: {e.Message}");
            throw;
        }
    }

    /// <summary>
    /// Delete a playback restriction
    /// </summary>
    /// <param name="playbackRestrictionId"></param>
    /// <returns></returns>
    public async Task DeletePlaybackRestrictionAsync(string playbackRestrictionId)
    {
        try
        {
            await _playbackRestrictionsApi.DeletePlaybackRestrictionAsync(playbackRestrictionId);
        }
        catch (ApiException e)
        {
            _logger.LogError($"Exception when calling PlaybackRestrictionsApi.DeletePlaybackRestriction: {e.Message}");
            throw;
        }
    }

    /// <summary>
    /// Get playback restriction details
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/PlaybackRestrictionResponse.md
    /// </summary>
    /// <param name="playbackRestrictionId"></param>
    /// <returns></returns>
    public async Task<PlaybackRestrictionResponse> GetPlaybackRestrictionAsync(string playbackRestrictionId)
    {
        try
        {
            return await _playbackRestrictionsApi.GetPlaybackRestrictionAsync(playbackRestrictionId);
        }
        catch (ApiException e)
        {
            _logger.LogError($"Exception when calling PlaybackRestrictionsApi.GetPlaybackRestriction: {e.Message}");
            throw;
        }
    }

    /// <summary>
    /// List all playback restrictions
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/ListPlaybackRestrictionsResponse.md
    /// </summary>
    /// <param name="page"></param>
    /// <param name="limit"></param>
    /// <returns></returns>
    public async Task<ListPlaybackRestrictionsResponse> ListPlaybackRestrictionsAsync(int? page = null, int? limit = null)
    {
        try
        {
            return await _playbackRestrictionsApi.ListPlaybackRestrictionsAsync(page, limit);
        }
        catch (ApiException e)
        {
            _logger.LogError($"Exception when calling PlaybackRestrictionsApi.ListPlaybackRestrictions: {e.Message}");
            throw;
        }
    }

    /// <summary>
    /// Update a playback restriction
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/UpdateReferrerDomainRestrictionRequest.md
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/PlaybackRestrictionResponse.md
    /// </summary>
    /// <param name="playbackRestrictionId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<PlaybackRestrictionResponse> UpdateReferrerDomainRestrictionAsync(string playbackRestrictionId, UpdateReferrerDomainRestrictionRequest request)
    {
        try
        {
            return await _playbackRestrictionsApi.UpdateReferrerDomainRestrictionAsync(playbackRestrictionId, request);
        }
        catch (ApiException e)
        {
            _logger.LogError($"Exception when calling PlaybackRestrictionsApi.UpdateReferrerDomainRestriction: {e.Message}");
            throw;
        }
    }

    /// <summary>
    /// Update a playback restriction
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/UpdateUserAgentRestrictionRequest.md
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/PlaybackRestrictionResponse.md
    /// </summary>
    /// <param name="playbackRestrictionId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<PlaybackRestrictionResponse> UpdateUserAgentRestrictionAsync(string playbackRestrictionId, UpdateUserAgentRestrictionRequest request)
    {
        try
        {
            return await _playbackRestrictionsApi.UpdateUserAgentRestrictionAsync(playbackRestrictionId, request);
        }
        catch (ApiException e)
        {
            _logger.LogError($"Exception when calling PlaybackRestrictionsApi.UpdateUserAgentRestriction: {e.Message}");
            throw;
        }
    }

    #endregion

    #region Mux Monitoring Api calls

    // Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/MonitoringApi.md

    /// <summary>
    /// Get monitoring breakdown
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/GetMonitoringBreakdownResponse.md
    /// </summary>
    /// <param name="monitoringMetricId"></param>
    /// <param name="dimension"></param>
    /// <param name="timestamp"></param>
    /// <param name="filters"></param>
    /// <param name="orderBy"></param>
    /// <param name="orderDirection"></param>
    /// <returns></returns>
    public async Task<GetMonitoringBreakdownResponse> GetMonitoringBreakdownAsync(
        string monitoringMetricId,
        string dimension = null,
        int? timestamp = null,
        List<string> filters = null,
        string orderBy = null,
        string orderDirection = null)
    {
        try
        {
            return await _monitoringApi.GetMonitoringBreakdownAsync(monitoringMetricId, dimension, timestamp, filters, orderBy, orderDirection);
        }
        catch (ApiException e)
        {
            _logger.LogError($"Exception when calling MonitoringApi.GetMonitoringBreakdown: {e.Message}");
            throw;
        }
    }

    /// <summary>
    /// Get monitoring breakdown timeseries
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/GetMonitoringBreakdownTimeseriesResponse.md
    /// </summary>
    /// <param name="monitoringMetricId"></param>
    /// <param name="dimension"></param>
    /// <param name="timeframe"></param>
    /// <param name="filters"></param>
    /// <param name="limit"></param>
    /// <param name="orderBy"></param>
    /// <param name="orderDirection"></param>
    /// <returns></returns>
    public async Task<GetMonitoringBreakdownTimeseriesResponse> GetMonitoringBreakdownTimeseriesAsync(
        string monitoringMetricId,
        string dimension = null,
        List<string> timeframe = null,
        List<string> filters = null,
        int? limit = null,
        string orderBy = null,
        string orderDirection = null)
    {
        try
        {
            return await _monitoringApi.GetMonitoringBreakdownTimeseriesAsync(monitoringMetricId, dimension, timeframe, filters, limit, orderBy, orderDirection);
        }
        catch (ApiException e)
        {
            _logger.LogError($"Exception when calling MonitoringApi.GetMonitoringBreakdownTimeseries: {e.Message}");
            throw;
        }
    }

    /// <summary>
    /// Get monitoring histogram
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/GetMonitoringHistogramTimeseriesResponse.md
    /// </summary>
    /// <param name="monitoringHistogramMetricId"></param>
    /// <param name="filters"></param>
    /// <returns></returns>
    public async Task<GetMonitoringHistogramTimeseriesResponse> GetMonitoringHistogramTimeseriesAsync(
        string monitoringHistogramMetricId,
        List<string> filters = null)
    {
        try
        {
            return await _monitoringApi.GetMonitoringHistogramTimeseriesAsync(monitoringHistogramMetricId, filters);
        }
        catch (ApiException e)
        {
            _logger.LogError($"Exception when calling MonitoringApi.GetMonitoringHistogramTimeseries: {e.Message}");
            throw;
        }
    }

    /// <summary>
    /// Get monitoring timeseries
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/GetMonitoringTimeseriesResponse.md
    /// </summary>
    /// <param name="monitoringMetricId"></param>
    /// <param name="filters"></param>
    /// <param name="timestamp"></param>
    /// <returns></returns>
    public async Task<GetMonitoringTimeseriesResponse> GetMonitoringTimeseriesAsync(
        string monitoringMetricId,
        List<string> filters = null,
        int? timestamp = null)
    {
        try
        {
            return await _monitoringApi.GetMonitoringTimeseriesAsync(monitoringMetricId, filters, timestamp);
        }
        catch (ApiException e)
        {
            _logger.LogError($"Exception when calling MonitoringApi.GetMonitoringTimeseries: {e.Message}");
            throw;
        }
    }

    /// <summary>
    /// List monitoring dimensions
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/ListMonitoringDimensionsResponse.md
    /// </summary>
    /// <returns></returns>
    public async Task<ListMonitoringDimensionsResponse> ListMonitoringDimensionsAsync()
    {
        try
        {
            return await _monitoringApi.ListMonitoringDimensionsAsync();
        }
        catch (ApiException e)
        {
            _logger.LogError($"Exception when calling MonitoringApi.ListMonitoringDimensions: {e.Message}");
            throw;
        }
    }

    /// <summary>
    /// List monitoring metrics
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/ListMonitoringMetricsResponse.md
    /// </summary>
    /// <returns></returns>
    public async Task<ListMonitoringMetricsResponse> ListMonitoringMetricsAsync()
    {
        try
        {
            return await _monitoringApi.ListMonitoringMetricsAsync();
        }
        catch (ApiException e)
        {
            _logger.LogError($"Exception when calling MonitoringApi.ListMonitoringMetrics: {e.Message}");
            throw;
        }
    }

    #endregion

    #region Mux Transcription Vocabularies Api calls

    // Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/TranscriptionVocabulariesApi.md

    /// <summary>
    /// Create a new transcription vocabulary
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/CreateTranscriptionVocabularyRequest.md
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/TranscriptionVocabularyResponse.md
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<TranscriptionVocabularyResponse> CreateTranscriptionVocabularyAsync(CreateTranscriptionVocabularyRequest request)
    {
        try
        {
            return await _transcriptionVocabulariesApi.CreateTranscriptionVocabularyAsync(request);
        }
        catch (ApiException e)
        {
            _logger.LogError($"Exception when calling TranscriptionVocabulariesApi.CreateTranscriptionVocabulary: {e.Message}");
            throw;
        }
    }

    /// <summary>
    /// Delete a transcription vocabulary
    /// </summary>
    /// <param name="vocabularyId"></param>
    /// <returns></returns>
    public async Task DeleteTranscriptionVocabularyAsync(string vocabularyId)
    {
        try
        {
            await _transcriptionVocabulariesApi.DeleteTranscriptionVocabularyAsync(vocabularyId);
        }
        catch (ApiException e)
        {
            _logger.LogError($"Exception when calling DeleteTranscriptionVocabulary: {e.Message}");
            throw;
        }
    }

    /// <summary>
    /// Get transcription vocabulary details
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/TranscriptionVocabularyResponse.md
    /// </summary>
    /// <param name="vocabularyId"></param>
    /// <returns></returns>
    public async Task<TranscriptionVocabularyResponse> GetTranscriptionVocabularyAsync(string vocabularyId)
    {
        try
        {
            return await _transcriptionVocabulariesApi.GetTranscriptionVocabularyAsync(vocabularyId);
        }
        catch (ApiException e)
        {
            _logger.LogError($"Exception when calling GetTranscriptionVocabulary: {e.Message}");
            throw;
        }
    }

    /// <summary>
    /// List all transcription vocabularies
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/ListTranscriptionVocabulariesResponse.md
    /// </summary>
    /// <param name="limit"></param>
    /// <param name="page"></param>
    /// <returns></returns>
    public async Task<ListTranscriptionVocabulariesResponse> ListTranscriptionVocabulariesAsync(
        int? limit = null,
        int? page = null)
    {
        try
        {
            return await _transcriptionVocabulariesApi.ListTranscriptionVocabulariesAsync(limit, page);
        }
        catch (ApiException e)
        {
            _logger.LogError($"Exception when calling ListTranscriptionVocabularies: {e.Message}");
            throw;
        }
    }

    /// <summary>
    /// Update a transcription vocabulary
    /// Ref: https://github.com/muxinc/mux-csharp/blob/main/docs/TranscriptionVocabularyResponse.md
    /// </summary>
    /// <param name="vocabularyId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<TranscriptionVocabularyResponse> UpdateTranscriptionVocabularyAsync(
        string vocabularyId,
        UpdateTranscriptionVocabularyRequest request)
    {
        try
        {
            return await _transcriptionVocabulariesApi.UpdateTranscriptionVocabularyAsync(vocabularyId, request);
        }
        catch (ApiException e)
        {
            _logger.LogError($"Exception when calling UpdateTranscriptionVocabulary: {e.Message}");
            throw;
        }
    }

    #endregion
}