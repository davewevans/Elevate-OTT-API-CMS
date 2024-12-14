using OttApiPlatform.Application.Common.Contracts.Mux;
using OttApiPlatform.Application.Common.Models.Mux;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace OttApiPlatform.WebAPI.Controllers;

[Route("api/webhooks/mux")]
[ApiController]
public class MuxWebhookController : ApiController
{
    private readonly IMuxConfigurationFactory _muxConfigurationFactory;
    private readonly IMuxWebHookHandler _muxWebHookHandler;
    private readonly ITenantResolver _tenantResolver;
    private readonly IApplicationDbContext _dbContext;
    private readonly ILogger<MuxWebhookController> _logger;

    public MuxWebhookController(IMuxConfigurationFactory muxConfigurationFactory, IMuxWebHookHandler handler, 
        IApplicationDbContext dbContext, ITenantResolver tenantResolver, ILogger<MuxWebhookController> logger)
    {
        _muxConfigurationFactory = muxConfigurationFactory;
        _muxWebHookHandler = handler;
        _dbContext = dbContext;
        _tenantResolver = tenantResolver;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> HandleMuxWebhook([FromBody] MuxWebhookRequest hookRequest)
    {
        if (hookRequest?.Data == null) return BadRequest();

        var tenantId = !string.IsNullOrWhiteSpace(hookRequest.Data.Passthrough)
            ? _muxWebHookHandler.ParseTenantIdFromPassthrough(hookRequest.Data.Passthrough)
            : _muxWebHookHandler.FindTenantIdByEnvironmentId(hookRequest.Environment.Id);

        if (tenantId == Guid.Empty)
        {
            _logger.LogWarning("No valid tenant ID found for the webhook request.");
            return Ok(); // Return 200 OK to Mux to avoid retries
        }

        _tenantResolver.SetTenantId(tenantId);

        var accountInfo = await _dbContext.AccountInfo
           .FirstOrDefaultAsync(x => x.TenantId == tenantId);

        var muxSettings = await _muxConfigurationFactory.GetMuxSettingsAsync(tenantId,
            accountInfo.VodStreamingService == VodStreamingService.Managed);

        _muxWebHookHandler.Configure(muxSettings);

        Request.Body.Seek(0, SeekOrigin.Begin);

        // Retrieve the raw request body
        string requestBody;
        using (var reader = new StreamReader(Request.Body, Encoding.UTF8, leaveOpen: true))
        {
            requestBody = await reader.ReadToEndAsync();
        }

        // Retrieve the mux-signature header
        if (!Request.Headers.TryGetValue("mux-signature", out var signatureHeader))
        {
            return Unauthorized("Missing mux-signature header.");
        }

        var signatureHeaderValue = signatureHeader.ToString();
        if (!_muxWebHookHandler.VerifyMuxSignature(signatureHeaderValue, requestBody))
        {
            return Unauthorized("Invalid signature.");
        }

        var eventHandled = await _muxWebHookHandler.HandleWebHookEvent(hookRequest);
        if (!eventHandled) return BadRequest();

        return Ok();
    }
}
