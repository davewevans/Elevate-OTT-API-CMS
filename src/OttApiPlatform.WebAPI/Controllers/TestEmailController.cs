using Microsoft.AspNetCore.Mvc;
using OttApiPlatform.Infrastructure.Service;

namespace OttApiPlatform.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestEmailController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public TestEmailController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpPost("SendTestEmail")]
    public async Task<IActionResult> SendTestEmail([FromBody] TestEmailRequest request)
    {

        string subject = string.IsNullOrEmpty(request.Subject) ? "Test Email" : request.Subject;
        string htmlMessage = string.IsNullOrEmpty(request.HtmlMessage) ? "<h1>This is a test email</h1><p>Sent from the Elevate OTT API.</p>" : request.HtmlMessage;

        await _notificationService.SendEmailAsync(request.Email, subject, htmlMessage, "This is a test email.");

        return Ok(new { Message = "Test email sent successfully." });
    }
}

public class TestEmailRequest
{
    public string Email { get; set; }
    public string Subject { get; set; }
    public string HtmlMessage { get; set; }
}

