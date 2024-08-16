namespace OttApiPlatform.Domain.Entities.Identity;

/// <summary>
/// Represents an attachment associated with an <see cref="ApplicationUser"/>.
/// </summary>
public class ApplicationUserAttachment
{
    #region Public Properties

    /// <summary>
    /// Gets or sets the ID of the attachment.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the URI of the attached file.
    /// </summary>
    public string FileUri { get; set; }

    /// <summary>
    /// Gets or sets the name of the attached file.
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// Gets or sets the ID of the user associated with this attachment.
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="ApplicationUser"/> associated with this attachment.
    /// </summary>
    public ApplicationUser User { get; set; }

    #endregion Public Properties
}
