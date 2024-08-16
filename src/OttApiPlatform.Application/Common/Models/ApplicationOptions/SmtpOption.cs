namespace OttApiPlatform.Application.Common.Models.ApplicationOptions;

/// <summary>
/// Represents options for SMTP configuration.
/// </summary>
public class SmtpOption
{
    #region Public Fields

    /// <summary>
    /// Gets the name of the section in the configuration file where these options can be found.
    /// </summary>
    public const string Section = "SmtpOption";

    #endregion Public Fields

    #region Public Properties

    /// <summary>
    /// Gets or sets the email address used as the "From" field in emails sent via SMTP.
    /// </summary>
    public string From { get; set; }

    /// <summary>
    /// Gets or sets the SMTP server address.
    /// </summary>
    public string SmtpServer { get; set; }

    /// <summary>
    /// Gets or sets the port number of the SMTP server.
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// Gets or sets the username to use when authenticating with the SMTP server.
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Gets or sets the password to use when authenticating with the SMTP server.
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Gets or sets the URL of the application.
    /// </summary>
    public string AppUrl { get; set; }

    /// <summary>
    /// Gets or sets the URL of the application's admin interface.
    /// </summary>
    public string AppUrlAdmin { get; set; }

    /// <summary>
    /// Gets or sets the email address to send exception error notifications to.
    /// </summary>
    public string ExceptionErrorEmail { get; set; }

    /// <summary>
    /// Whether to use SSL encryption when connecting to the SMTP server.
    /// </summary>
    public bool EnableSsl { get; set; }

    /// <summary>
    /// Whether to use default credentials when authenticating with the SMTP server.
    /// </summary>
    public bool UseDefaultCredentials { get; set; }

    #endregion Public Properties
}