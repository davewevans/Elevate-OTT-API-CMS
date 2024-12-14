using Azure;
using Azure.Core;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Communication;
using Azure.ResourceManager.Resources;
using Azure.Communication.Email;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace OttApiPlatform.Infrastructure.Services;

public class NotificationService : INotificationService
{
    private readonly IConfigReaderService _configReaderService;
    private readonly SmtpOption _smtpOptions;

    public NotificationService(IConfigReaderService configReaderService)
    {
        _configReaderService = configReaderService;
        _smtpOptions = _configReaderService.GetSmtpOption();
    }

    #region Public Methods

    public Task SendSmsAsync(Message message)
    {
        return Task.CompletedTask;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage, string textMessage = "")
    {
        switch (_smtpOptions.EmailService)
        {
            case EmailService.AzureCommunicationServices:
                await SendAzureEmailAsync(email, subject, htmlMessage, textMessage);
                break;
            case EmailService.Postmark:
                await SendPostmarkEmailAsync(email, subject, htmlMessage, textMessage);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    #endregion Public Methods


    #region Private Methods

    private async Task SendAzureEmailAsync(string email, string subject, string htmlMessage, string textMessage)
    {
        // ref: https://learn.microsoft.com/en-us/azure/communication-services/quickstarts/email/send-email?tabs=windows%2Cconnection-string%2Csend-email-and-get-status-async%2Csync-client&pivots=programming-language-csharp

        string connectionString = _smtpOptions.AzureCommunicationServicesConnectionString;
        EmailClient emailClient = new EmailClient(connectionString);
        var sender = _smtpOptions.Sender;

        // Send the email message with WaitUntil.Started
        EmailSendOperation emailSendOperation = await emailClient.SendAsync(
            Azure.WaitUntil.Started,
            sender,
            email,
            subject,
            htmlMessage);

        // Call UpdateStatus on the email send operation to poll for the status manually.       
        try
        {
            while (true)
            {
                await emailSendOperation.UpdateStatusAsync();
                if (emailSendOperation.HasCompleted)
                {
                    break;
                }
                await Task.Delay(100);
            }

            if (emailSendOperation.HasValue)
            {
                // TODO log
                Console.WriteLine($"Email queued for delivery. Status = {emailSendOperation.Value.Status}");
            }
        }
        catch (RequestFailedException ex)
        {
            // TODO log
            Console.WriteLine($"Email send failed with Code = {ex.ErrorCode} and Message = {ex.Message}");
        }

        /// Get the OperationId so that it can be used for tracking the message for troubleshooting
        string operationId = emailSendOperation.Id;
        Console.WriteLine($"Email operation id = {operationId}");
    }

    private async Task SendPostmarkEmailAsync(string email, string subject, string htmlMessage, string textMessage)
    {
        using var client = new HttpClient();

        // Set up the headers
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        client.DefaultRequestHeaders.Add("X-Postmark-Server-Token", _smtpOptions.PostmarkApiKey);

        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

        // Prepare the email data
        var emailData = new
        {
            From = _smtpOptions.PostmarkFrom,
            To = email,
            Subject = subject,
            HtmlBody = htmlMessage,
            TextBody = textMessage,
            MessageStream = "outbound"
        };

        var content = new StringContent(JsonSerializer.Serialize(emailData), Encoding.UTF8, "application/json");

        // Send the email
        var response = await client.PostAsync(_smtpOptions.PostmarkApiUrl, content);

        // Handle the response
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Email sent successfully.");
        }
        else
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Failed to send email. Status code: {response.StatusCode}");
            Console.WriteLine($"Response: {responseContent}");
        }
    }

    #endregion
}