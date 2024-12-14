using OttApiPlatform.Domain.Entities.Mux;

namespace OttApiPlatform.Application.Common.Contracts.Mux;

public interface IMuxConfigurationService
{
    Task<MuxSettingsModel> GetMuxConfigForTenantAsync(Guid tenantId);
    Task SaveMuxConfigAsync(Guid tenantId, string tokenId, string tokenSecret);
}
