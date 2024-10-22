namespace OttApiPlatform.Application.Common.Contracts.Infrastructure;

/// <summary>
/// Retrieves the global configuration for the application from the appsettings.json.
/// </summary>
public interface IConfigReaderService
{
    #region Public Methods

    /// <summary>
    /// Gets the options for the application user.
    /// </summary>
    AppUserOptions GetAppUserOptions();

    /// <summary>
    /// Gets the options for the application lockout.
    /// </summary>
    AppLockoutOptions GetAppLockoutOptions();

    /// <summary>
    /// Gets the options for the application password.
    /// </summary>
    AppPasswordOptions GetAppPasswordOptions();

    /// <summary>
    /// Gets the options for the application sign-in.
    /// </summary>
    AppSignInOptions GetAppSignInOptions();

    /// <summary>
    /// Gets the options for the user access token and refresh token.
    /// </summary>
    AppTokenOptions GetAppTokenOptions();

    /// <summary>
    /// Gets the options for the application file storage.
    /// </summary>
    AppFileStorageOptions GetAppFileStorageOptions();

    /// <summary>
    /// Gets the JWT options.
    /// </summary>
    JwtOptions GetJwtOptions();

    /// <summary>
    /// Gets the SMTP options.
    /// </summary>
    SmtpOption GetSmtpOption();

    /// <summary>
    /// Gets the options for the client application.
    /// </summary>
    ClientAppOptions GetClientAppOptions();

    /// <summary>
    /// Gets the options for the application tenant.
    /// </summary>
    AppTenantOptions GetAppTenantOptions();

    /// <summary>
    /// Gets the options for the license info.
    /// </summary>
    /// <returns></returns>
    LicenseInfoOptions GetLicenseInfoOptions();

    /// <summary>
    /// Gets the subdomain.
    /// </summary>
    string GetSubDomain();

    #endregion Public Methods
}