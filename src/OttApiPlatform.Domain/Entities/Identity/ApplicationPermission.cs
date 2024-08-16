namespace OttApiPlatform.Domain.Entities.Identity;

/// <summary>
/// Represents a permission user.
/// </summary>
public class ApplicationPermission
{
    #region Public Constructors

    public ApplicationPermission()
    {
        Permissions = new List<ApplicationPermission>();
    }

    #endregion Public Constructors

    #region Public Properties

    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the permission.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the permission is visible to tenants.
    /// </summary>
    public bool TenantVisibility { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the permission is visible to hosts.
    /// </summary>
    public bool HostVisibility { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the permission is a custom permission.
    /// </summary>
    public bool IsCustomPermission { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the parent permission.
    /// </summary>
    public Guid? ParentId { get; set; }

    /// <summary>
    /// Gets or sets the parent permission.
    /// </summary>
    [ForeignKey("ParentId")]
    public ApplicationPermission Parent { get; set; }

    /// <summary>
    /// Gets or sets the list of child permissions.
    /// </summary>
    public IList<ApplicationPermission> Permissions { get; set; }

    #endregion Public Properties
}