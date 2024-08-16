namespace OttApiPlatform.Application.Common.Models;

/// <summary>
/// Represents information of a message.
/// </summary>
public class Message
{
    #region Public Properties

    /// <summary>
    /// Gets or sets the email address of the sender.
    /// </summary>
    public string From { get; set; }

    /// <summary>
    /// Gets or sets the email address of the recipient.
    /// </summary>
    public string To { get; set; }

    /// <summary>
    /// Gets or sets the subject of the message.
    /// </summary>
    public string Subject { get; set; }

    /// <summary>
    /// Gets or sets the body of the message.
    /// </summary>
    public string Body { get; set; }

    #endregion Public Properties
}