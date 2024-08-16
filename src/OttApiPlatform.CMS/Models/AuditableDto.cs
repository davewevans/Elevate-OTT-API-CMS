namespace OttApiPlatform.CMS.Models;

/// <summary>
/// Indicates that any class inherits this interface has audit properties as shadow properties.
/// </summary>
/// <remarks>
/// This class is only allowed to be used with DTO's classes that should return audit data. This
/// class is not allowed to be used with entity classes.
/// </remarks>
public abstract class AuditableDto
{
    #region Public Properties

    public DateTime CreatedOn { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public string ModifiedBy { get; set; }

    #endregion Public Properties
}