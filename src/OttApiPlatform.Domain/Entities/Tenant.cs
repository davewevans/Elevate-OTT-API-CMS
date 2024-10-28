namespace OttApiPlatform.Domain.Entities;

/// <summary>
/// Represents a tenant in a multi-tenant application.
/// </summary>
public class Tenant : IAuditable
{
    #region Public Properties

    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the tenant.
    /// </summary>
    public string Name { get; set; }

    public string LicenseKey { get; set; }

    public string SubDomain { get; set; }

    public string CustomDomain { get; set; }

    public string StorageFileNamePrefix { get; set; }

    public string CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public string ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string DeletedBy { get; set; }

    public DateTime? DeletedOn { get; set; }

    #endregion Public Properties
}