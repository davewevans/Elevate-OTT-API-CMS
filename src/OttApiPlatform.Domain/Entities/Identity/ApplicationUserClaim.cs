namespace OttApiPlatform.Domain.Entities.Identity;

/// <summary>
/// Represents a claim associated with an <see cref="ApplicationUser"/>.
/// </summary>
public class ApplicationUserClaim : IdentityUserClaim<string>
{
    #region Public Properties

    /// <summary>
    /// Gets or sets the user associated with this claim.
    /// </summary>
    public ApplicationUser User { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this claim is excluded.
    /// </summary>
    public bool IsExcluded { get; set; }

    #endregion Public Properties
}