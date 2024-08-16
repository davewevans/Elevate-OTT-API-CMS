namespace OttApiPlatform.Domain.Entities.Identity;

/// <summary>
/// Represents a join entity between an <see cref="ApplicationUser"/> and a <see
/// cref="ApplicationRole"/> in the application.
/// </summary>
public class ApplicationUserRole : IdentityUserRole<string>
{
    #region Public Properties

    /// <summary>
    /// Gets or sets the user associated with this role.
    /// </summary>
    public ApplicationUser User { get; set; }

    /// <summary>
    /// Gets or sets the role associated with this user.
    /// </summary>
    public ApplicationRole Role { get; set; }

    #endregion Public Properties
}