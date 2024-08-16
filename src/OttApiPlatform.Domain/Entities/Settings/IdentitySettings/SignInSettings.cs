namespace OttApiPlatform.Domain.Entities.Settings.IdentitySettings;

/// <summary>
/// Represents the sign in settings for user accounts.
/// </summary>
public class SignInSettings : ISettingsSchema, IMayHaveTenant
{
    /// <summary>
    /// Gets or sets the unique identifier of the sign-in settings.
    /// </summary>
    public Guid Id { get; set; }

    ///// <summary>
    ///// Gets or sets a value indicating whether users must have a confirmed email before signing in.
    ///// </summary>
    //public bool RequireConfirmedEmail { get; set; }

    ///// <summary>
    ///// Gets or sets a value indicating whether users must have a confirmed phone number before signing in.
    ///// </summary>
    //public bool RequireConfirmedPhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether users must have a confirmed account before signing in.
    /// </summary>
    public bool RequireConfirmedAccount { get; set; }

    public Guid? TenantId { get; set; }
}
