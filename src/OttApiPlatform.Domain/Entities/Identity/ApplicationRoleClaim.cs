namespace OttApiPlatform.Domain.Entities.Identity;

/// <summary>
/// Represents a claim with an <see cref="ApplicationRole"/>.
/// </summary>
public class ApplicationRoleClaim : IdentityRoleClaim<string>
{
    #region Public Properties

    /// <summary>
    /// Gets or sets the role associated with this claim.
    /// </summary>
    public ApplicationRole Role { get; set; }

    #endregion Public Properties
}