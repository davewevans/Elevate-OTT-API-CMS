using OttApiPlatform.CMS.Features.ContentManagement.Assets.Commands.CreateAsset;
using OttApiPlatform.CMS.Features.ContentManagement.Assets.Commands.UpdateAsset;
using OttApiPlatform.CMS.Features.ContentManagement.Assets.Queries.GetAssetForEdit;
using OttApiPlatform.CMS.Features.ContentManagement.Assets.Queries.GetAssets;

namespace OttApiPlatform.CMS.Contracts.Consumers;

public interface IAssetsClient
{
    Task<ApiResponseWrapper<GetAssetByIdResponse>> GetAssetByIdAsync(GetAssetByIdQuery query);
    Task<ApiResponseWrapper<GetAssetsResponse>> GetAssetsAsync(GetAssetsQuery query);
    Task<ApiResponseWrapper<CreateAssetResponse>> CreateAssetAsync(CreateAssetCommand command);
    Task<ApiResponseWrapper<GetAssetByIdResponse>> UpdateAssetAsync(UpdateAssetCommand command);
}
