using OttApiPlatform.Domain.Entities.Mux;

namespace OttApiPlatform.Application.Common.Contracts.Mux;

public interface IMuxConfigurationFactory
{
    Task<MuxSettingsModel> GetMuxSettingsAsync(Guid tenantId, bool useManagedSettings);
}
