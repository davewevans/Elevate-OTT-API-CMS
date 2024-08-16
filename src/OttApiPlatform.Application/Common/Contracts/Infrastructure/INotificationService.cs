namespace OttApiPlatform.Application.Common.Contracts.Infrastructure;

/// <summary>
/// Sends messages via SMS, email, ..etc.
/// </summary>
public interface INotificationService
{
    #region Public Methods

    /// <summary>
    /// Sends an SMS message asynchronously.
    /// </summary>
    /// <param name="message">The message to send.</param>
    Task SendSmsAsync(Message message);

    /// <summary>
    /// Sends an email message asynchronously.
    /// </summary>
    /// <param name="email">The recipient email address.</param>
    /// <param name="subject">The email subject.</param>
    /// <param name="htmlMessage">The HTML body of the email.</param>
    Task SendEmailAsync(string email, string subject, string htmlMessage);

    #endregion Public Methods
}