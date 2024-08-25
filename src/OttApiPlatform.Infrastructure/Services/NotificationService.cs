using Azure;
using Azure.Core;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Communication;
using Azure.ResourceManager.Resources;
using Azure.Communication.Email;

namespace OttApiPlatform.Infrastructure.Service;

public class NotificationService : INotificationService
{

    private readonly IConfigReaderService _configReaderService;

    public NotificationService(IConfigReaderService configReaderService)
    {
        _configReaderService = configReaderService;
    }

    #region Public Methods

    public Task SendSmsAsync(Message message)
    {
        return Task.CompletedTask;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        // ref: https://learn.microsoft.com/en-us/azure/communication-services/quickstarts/email/send-email?tabs=windows%2Cconnection-string%2Csend-email-and-get-status-async%2Csync-client&pivots=programming-language-csharp

        var smtpOptions = _configReaderService.GetSmtpOption();
        string connectionString = smtpOptions.AzureCommunicationServicesConnectionString;
        EmailClient emailClient = new EmailClient(connectionString);
        var sender = smtpOptions.Sender;

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

    #endregion Public Methods
}