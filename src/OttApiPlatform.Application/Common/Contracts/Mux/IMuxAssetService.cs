using OttApiPlatform.Application.Features.ContentManagement.Videos.Commands.CreateAssetAtMux;

namespace OttApiPlatform.Application.Common.Contracts.Mux;

public interface IMuxAssetService
{
    Task GetAssetFromMuxAsync(string assetId);
    Task ListAssetsFromMuxByTenantAsync(Guid tenantId);
    Task<CreateAssetAtMuxResponse> CreateAssetAtMuxAsync(CreateAssetAtMuxCommand createAssetAtMuxCommand);
    Task UpdateAssetAtMuxAsync();
    Task DeleteAssetFromMuxAsync(string assetId);
    Task<bool> AssetExistsAsync(string assetId);

}
