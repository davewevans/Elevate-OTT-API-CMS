namespace OttApiPlatform.Application.Common.Models;

/// <summary>
/// Base class for DTOs that require auditing.
/// </summary>
public abstract class AuditableDto
{
    #region Public Properties

    /// <summary>
    /// Gets or sets the date and time when the DTO was created.
    /// </summary>
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// Gets or sets the name of the user who created the DTO.
    /// </summary>
    public string CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the DTO was last modified.
    /// </summary>
    public DateTime? ModifiedOn { get; set; }

    /// <summary>
    /// Gets or sets the name of the user who last modified the DTO.
    /// </summary>
    public string ModifiedBy { get; set; }

    /// <summary>
    /// Gets or sets the name of the user who deleted the DTO.
    /// </summary>
    public string DeletedBy { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the DTO was deleted.
    /// </summary>
    public DateTime? DeletedOn { get; set; }

    #endregion Public Properties
}