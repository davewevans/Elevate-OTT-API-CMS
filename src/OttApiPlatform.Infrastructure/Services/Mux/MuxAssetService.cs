using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mux.Csharp.Sdk.Api;
using Mux.Csharp.Sdk.Client;
using Mux.Csharp.Sdk.Model;
using OttApiPlatform.Application.Common.Contracts.Mux;
using OttApiPlatform.Application.Common.Contracts.Repository;
using OttApiPlatform.Application.Features.ContentManagement.Videos.Commands.CreateAssetAtMux;

namespace OttApiPlatform.Infrastructure.Services.Mux;

public class MuxAssetService : IMuxAssetService
{
    private readonly IConfigReaderService _configReaderService;
    private readonly ILogger<MuxAssetService> _logger;
    private readonly Configuration _muxConfig;
    private MuxOptions _muxOptions;

    public MuxAssetService(ILogger<MuxAssetService> logger,
        IRepositoryManager repository, IConfigReaderService configReaderService)
    {
        _logger = logger;
        _configReaderService = configReaderService;
        _muxOptions = _configReaderService.GetMuxOptions();

        //_muxConfig = new Configuration
        //{
        //    BasePath = _muxOptions.BasePath,
        //    Username = _muxOptions.Username,
        //    Password = _muxOptions.Password
        //};
    }

    //    list assets
    //    get asset
    //    delete asset
    //    update asset
    //    get asset input info
    //    create playback id
    //    get playback id
    //    delete playback id
    //    update mp4 support
    //    update master access
    //    create asset track
    //    delete asset track

    public Task GetAssetFromMuxAsync(string assetId)
    {
        throw new NotImplementedException();
    }

    public Task ListAssetsFromMuxByTenantAsync(Guid tenantId)
    {
        throw new NotImplementedException();
    }



    public async Task<CreateAssetAtMuxResponse> CreateAssetAtMuxAsync(CreateAssetAtMuxCommand createAssetAtMuxCommand)
    {
        var apiInstance = new AssetsApi(_muxConfig);

        var input = new InputSettings(createAssetAtMuxCommand.BlobUrl, 
            languageCode: createAssetAtMuxCommand.LanguageCode, 
            closedCaptions: createAssetAtMuxCommand.ClosedCaption);

        // ref: https://docs.mux.com/api-reference/video#operation/create-asset

        var assetCreationRequest = new CreateAssetRequest
        {
            Input = new List<InputSettings> { input },
            PlaybackPolicy = new List<PlaybackPolicy> { PlaybackPolicy.Public },
            Test = createAssetAtMuxCommand.IsTestAsset,
            Mp4Support = createAssetAtMuxCommand.Mp4Support ? CreateAssetRequest.Mp4SupportEnum.Standard : CreateAssetRequest.Mp4SupportEnum.None,
            Passthrough = createAssetAtMuxCommand.Passthrough
        };

        var asset = await apiInstance.CreateAssetAsync(assetCreationRequest);
        return new CreateAssetAtMuxResponse { AssetId = asset.Data.Id };
    }

    public Task UpdateAssetAtMuxAsync()
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAssetFromMuxAsync(string assetId)
    {
        var apiInstance = new AssetsApi(_muxConfig);
        await apiInstance.DeleteAssetAsync(assetId);
    }

    public async Task<bool> AssetExistsAsync(string assetId)
    {
        var apiInstance = new AssetsApi(_muxConfig);
        try
        {
            // Exception thrown if asset doesn't exist at Mux
            var response = await apiInstance.GetAssetAsync(assetId);
            return response.Data.Id.Equals(assetId);
        }
        catch (ApiException ex)
        {
            _logger.LogError(ex.Message);
            return false;
        }
    }
}
