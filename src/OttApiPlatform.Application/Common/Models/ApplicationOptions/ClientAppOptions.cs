namespace OttApiPlatform.Application.Common.Models.ApplicationOptions;

/// <summary>
/// Represents options for the client application.
/// </summary>
public class ClientAppOptions
{
    #region Public Fields

    /// <summary>
    /// Gets the name of the section in the configuration file where these options can be found.
    /// </summary>
    public const string Section = "ClientApp";

    #endregion Public Fields

    #region Public Properties

    /// <summary>
    /// Gets or sets the host name for single-tenant applications.
    /// </summary>
    public string SingleTenantHostName { get; set; }

    /// <summary>
    /// Gets or sets the host name for multi-tenant applications.
    /// </summary>
    public string MultiTenantHostName { get; set; }

    /// <summary>
    /// Gets or sets the URL for confirming email changes.
    /// </summary>
    public string ConfirmEmailChangeUrl { get; set; }

    /// <summary>
    /// Gets or sets the URL for confirming email addresses.
    /// </summary>
    public string ConfirmEmailUrl { get; set; }

    /// <summary>
    /// Gets or sets the URL for confirming email addresses with a return URL.
    /// </summary>
    public string ConfirmEmailUrlWithReturnUrl { get; set; }

    /// <summary>
    /// Gets or sets the URL for resetting passwords.
    /// </summary>
    public string ResetPasswordUrl { get; set; }

    #endregion Public Properties
}