namespace OttApiPlatform.Domain.Entities.Identity;

/// <summary>
/// Represents a token associated with an <see cref="ApplicationUser"/>.
/// </summary>
public class ApplicationUserToken : IdentityUserToken<string>
{
    #region Public Properties

    /// <summary>
    /// Gets or sets the user associated with this token.
    /// </summary>
    public ApplicationUser User { get; set; }

    #endregion Public Properties
}