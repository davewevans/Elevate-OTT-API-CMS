namespace OttApiPlatform.Domain.Entities;

/// <summary>
/// Represents a report in the system.
/// </summary>
public class Report : IAuditable, IMustHaveTenant
{
    #region Public Properties

    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the report.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the query string used to generate the report.
    /// </summary>
    public string QueryString { get; set; }

    /// <summary>
    /// Gets or sets the name of the file associated with the report.
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// Gets or sets the URI of the file associated with the report.
    /// </summary>
    public string FileUri { get; set; }

    /// <summary>
    /// Gets or sets the content type of the file associated with the report.
    /// </summary>
    public string ContentType { get; set; }

    /// <summary>
    /// Gets or sets the status of the report.
    /// </summary>
    public int Status { get; set; }

    public string CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public string ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string DeletedBy { get; set; }

    public DateTime? DeletedOn { get; set; }

    public Guid TenantId { get; set; }

    #endregion Public Properties
}