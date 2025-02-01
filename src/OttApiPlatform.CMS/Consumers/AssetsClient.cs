using OttApiPlatform.CMS.Features.ContentManagement.Assets.Commands.CreateAsset;
using OttApiPlatform.CMS.Features.ContentManagement.Assets.Commands.UpdateAsset;
using OttApiPlatform.CMS.Features.ContentManagement.Assets.Queries.GetAssetForEdit;
using OttApiPlatform.CMS.Features.ContentManagement.Assets.Queries.GetAssets;

namespace OttApiPlatform.CMS.Consumers;

public class AssetsClient : IAssetsClient
{
    #region Private Fields

    private readonly IHttpService _httpService;

    #endregion Private Fields

    #region Public Constructors

    public AssetsClient(IHttpService httpService)
    {
        _httpService = httpService;
    }

    #endregion


    #region Public Method

    public async Task<ApiResponseWrapper<GetAssetByIdResponse>> GetAssetByIdAsync(GetAssetByIdQuery query)
    {
        throw new NotImplementedException();
    }

    public async Task<ApiResponseWrapper<GetAssetsResponse>> GetAssetsAsync(GetAssetsQuery query)
    {
        throw new NotImplementedException();
    }

    public async Task<ApiResponseWrapper<CreateAssetResponse>> CreateAssetAsync(CreateAssetCommand command)
    {
        return await _httpService.Post<CreateAssetCommand, CreateAssetResponse>("assets", command);
    }

    public async Task<ApiResponseWrapper<GetAssetByIdResponse>> UpdateAssetAsync(UpdateAssetCommand command)
    {
        throw new NotImplementedException();
    }

    #endregion
}
