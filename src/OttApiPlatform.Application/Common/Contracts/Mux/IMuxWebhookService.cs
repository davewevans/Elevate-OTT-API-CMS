using OttApiPlatform.Application.Common.Models.Mux;

namespace OttApiPlatform.Application.Common.Contracts.Mux;

public interface IMuxWebhookService
{
    Task<bool> HandleWebHookEvent(MuxWebhookRequest hookRequest);
    (string timestamp, string muxSignature) GetMuxTimestampAndSignature(string muxHeader);
    bool VerifyRequestFromMux(string timestamp, string signature, string requestBody);
    void SetTenantViaTenantResolver(string passthrough);
    Task TestSignalR();
}
