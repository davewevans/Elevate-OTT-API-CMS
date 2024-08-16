namespace OttApiPlatform.Domain.Entities.Identity;

/// <summary>
/// Represents a login associated with an <see cref="ApplicationUser"/>.
/// </summary>
public class ApplicationUserLogin : IdentityUserLogin<string>
{
    #region Public Properties

    /// <summary>
    /// Gets or sets the user associated with this login.
    /// </summary>
    public ApplicationUser User { get; set; }

    #endregion Public Properties
}