namespace OttApiPlatform.Infrastructure.Services;

public class NotificationService : INotificationService
{
    #region Public Methods

    public Task SendSmsAsync(Message message)
    {
        return Task.CompletedTask;
    }

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        //MailAddress to = new MailAddress(email);
        //MailAddress from = new MailAddress(_configReaderService.GetSmtpOption().From);

        //MailMessage message = new MailMessage(from, to);
        //message.Subject = subject;
        //message.Body = htmlMessage;
        //message.IsBodyHtml = true;
        //SmtpClient client = new SmtpClient.
        //{
        //    Host = _configReaderService.GetSmtpOption().SmtpServer,//"smtp.gmail.com",
        //    Port = _configReaderService.GetSmtpOption().Port,
        //    EnableSsl = _configReaderService.GetSmtpOption().EnableSsl.
        //};

        //client.UseDefaultCredentials = _configReaderService.GetSmtpOption().UseDefaultCredentials;
        //client.Credentials = new NetworkCredential(_configReaderService.GetSmtpOption().UserName, _configReaderService.GetSmtpOption().Password);
        //client.Send(message);

        return Task.CompletedTask;
    }

    #endregion Public Methods
}