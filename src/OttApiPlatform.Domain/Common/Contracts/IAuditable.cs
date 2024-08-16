namespace OttApiPlatform.Domain.Common.Contracts;

/// <summary>
/// Represents an entity that is auditable, i.e., that tracks creation, modification, and deletion metadata.
/// </summary>
public interface IAuditable
{
    #region Public Properties

    /// <summary>
    /// Gets or sets the name of the user who created the entity.
    /// </summary>
    public string CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the entity was created.
    /// </summary>
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// Gets or sets the name of the user who last modified the entity.
    /// </summary>
    public string ModifiedBy { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the entity was last modified.
    /// </summary>
    public DateTime? ModifiedOn { get; set; }

    /// <summary>
    /// Gets or sets the name of the user who deleted the entity.
    /// </summary>
    public string DeletedBy { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the entity was deleted.
    /// </summary>
    public DateTime? DeletedOn { get; set; }

    #endregion Public Properties
}