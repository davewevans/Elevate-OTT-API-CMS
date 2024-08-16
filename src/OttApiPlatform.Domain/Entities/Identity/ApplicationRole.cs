namespace OttApiPlatform.Domain.Entities.Identity;

/// <summary>
/// Represents a role.
/// </summary>
public class ApplicationRole : IdentityRole, IAuditable, IMayHaveTenant
{
    #region Public Constructors

    public ApplicationRole()
    {
        UserRoles = new List<ApplicationUserRole>();
        RoleClaims = new List<ApplicationRoleClaim>();
    }

    #endregion Public Constructors

    #region Public Properties

    /// <summary>
    /// Gets or sets a value indicating whether this role is static or not.
    /// </summary>
    public bool IsStatic { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this role is the default role.
    /// </summary>
    public bool IsDefault { get; set; }

    /// <summary>
    /// Gets or sets the collection of user roles associated with this role.
    /// </summary>
    public ICollection<ApplicationUserRole> UserRoles { get; set; }

    /// <summary>
    /// Gets or sets the collection of role claims associated with this role.
    /// </summary>
    public ICollection<ApplicationRoleClaim> RoleClaims { get; set; }

    public string CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public string DeletedBy { get; set; }
    public DateTime? DeletedOn { get; set; }
    public Guid? TenantId { get; set; }

    #endregion Public Properties
}