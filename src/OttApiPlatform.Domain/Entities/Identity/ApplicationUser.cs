using OttApiPlatform.Domain.Entities.ContentAccessManagement;

namespace OttApiPlatform.Domain.Entities.Identity;

/// <summary>
/// Represents a user.
/// </summary>
public class ApplicationUser : IdentityUser, IAuditable, IMayHaveTenant
{
    #region Public Constructors

    public ApplicationUser()
    {
        Claims = new List<ApplicationUserClaim>();
        Logins = new List<ApplicationUserLogin>();
        Tokens = new List<ApplicationUserToken>();
        UserRoles = new List<ApplicationUserRole>();
        UserAttachments = new List<ApplicationUserAttachment>();
    }

    #endregion Public Constructors

    #region Public Properties

    public Guid? TenantId { get; set; }


    /// <summary>
    /// Gets or sets the name of the user.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the surname of the user.
    /// </summary>
    public string Surname { get; set; }

    /// <summary>
    /// Gets the full name of the user.
    /// </summary>
    public string FullName => $"{Name} {Surname}";

    /// <summary>
    /// Gets or sets the job title of the user.
    /// </summary>
    public string JobTitle { get; set; }

    /// <summary>
    /// Gets or sets the URI of the user's avatar.
    /// </summary>
    public string AvatarUri { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the user is suspended.
    /// </summary>
    public bool IsSuspended { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the user is static.
    /// </summary>
    public bool IsStatic { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the user is a super administrator.
    /// </summary>
    public bool IsSuperAdmin { get; set; }

    /// <summary>
    /// Gets or sets the refresh token of the user.
    /// </summary>
    public string RefreshToken { get; set; }

    /// <summary>
    /// Gets or sets the time span of the refresh token of the user.
    /// </summary>
    public DateTime RefreshTokenTimeSpan { get; set; }

    /// <summary>
    /// Gets or sets the claims of the user.
    /// </summary>
    public List<ApplicationUserClaim> Claims { get; set; }
    
    /// <summary>
    /// Gets or sets the logins of the user.
    /// </summary>
    public List<ApplicationUserLogin> Logins { get; set; }

    /// <summary>
    /// Gets or sets the tokens of the user.
    /// </summary>
    public List<ApplicationUserToken> Tokens { get; set; }

    /// <summary>
    /// Gets or sets the roles of the user.
    /// </summary>
    public List<ApplicationUserRole> UserRoles { get; set; }

    /// <summary>
    /// Gets or sets the attachments of the user.
    /// </summary>
    public List<ApplicationUserAttachment> UserAttachments { get; set; }

    public string CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public string DeletedBy { get; set; }
    public DateTime? DeletedOn { get; set; }

    #endregion Public Properties

    #region Navigational Properties

    public Tenant Tenant { get; set; }

    // public ICollection<UserSubscriptionModel> UserSubscriptions { get; set; }

    #endregion

}