using OttApiPlatform.Application.Common.Models.Mux;
using OttApiPlatform.Domain.Entities.Mux;

namespace OttApiPlatform.Application.Common.Contracts.Mux;

public interface IMuxWebHookHandler
{
    /// <summary>
    /// Handles a webhook event from Mux.
    /// </summary>
    /// <param name="hookRequest">The webhook request payload.</param>
    /// <returns>A boolean indicating whether the event was successfully handled.</returns>
    Task<bool> HandleWebHookEvent(MuxWebhookRequest hookRequest);
    void Configure(MuxSettingsModel settings);
    (string timestamp, string muxSignature) GetMuxTimestampAndSignature(string muxHeader);
    bool VerifyMuxSignature(string signatureHeader, string requestBody);
    Guid ParseTenantIdFromPassthrough(string passthrough);
    Guid FindTenantIdByEnvironmentId(string environmentId);
}
