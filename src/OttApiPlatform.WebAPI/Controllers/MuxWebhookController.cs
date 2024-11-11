using OttApiPlatform.Application.Common.Contracts.Mux;
using OttApiPlatform.Application.Common.Models.Mux;

namespace OttApiPlatform.WebAPI.Controllers;

[Route("api/mux-webhook")]
[ApiController]
public class MuxWebhookController : ApiController
{
    private readonly IMuxWebhookService? _webhookService;

    private readonly IApplicationDbContext _dbContext;

    public MuxWebhookController(IMuxWebhookService? webhookService, IApplicationDbContext dbContext)
    {
        _webhookService = webhookService;
        _dbContext = dbContext;
    }

    [HttpPost("callback")]
    public async Task<IActionResult> Post([FromBody] MuxWebhookRequest? hookRequest)
    {
        string muxHeader = Request.Headers["mux-signature"];
        (string timestamp, string muxSignature) =
            _webhookService.GetMuxTimestampAndSignature(muxHeader);

        Request.Body.Seek(0, SeekOrigin.Begin);
        var rawRequestBody = await new StreamReader(Request.Body).ReadToEndAsync();

        bool requestFromMux =
            _webhookService.VerifyRequestFromMux(timestamp, muxSignature, rawRequestBody);

        if (!requestFromMux) return BadRequest();

        if (hookRequest?.Data == null) return Ok();

        // Get tenant ID from passthrough value and set tenant via tenant resolver 
        _webhookService.SetTenantViaTenantResolver(hookRequest.Data.Passthrough);

        var eventHandled = await _webhookService.HandleWebHookEvent(hookRequest);
        if (!eventHandled) return BadRequest();

        return Ok();
    }
}
