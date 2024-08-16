namespace OttApiPlatform.Application.Common.Contracts.Application;

/// <summary>
/// Represents a service for reading application settings.
/// </summary>
public interface IAppSettingsReaderService
{
    #region Public Methods

    /// <summary>
    /// Retrieves the identity settings for editing.
    /// </summary>
    /// <returns>An envelope containing the identity settings response.</returns>
    Task<Envelope<IdentitySettingsForEditResponse>> GetIdentitySettings();

    /// <summary>
    /// Retrieves the token settings for editing.
    /// </summary>
    /// <returns>An envelope containing the token settings response.</returns>
    Task<Envelope<TokenSettingsForEditResponse>> GetTokenSettings();

    /// <summary>
    /// Retrieves the file storage settings for editing.
    /// </summary>
    /// <returns>An envelope containing the file storage settings response.</returns>
    Task<Envelope<FileStorageSettingsForEditResponse>> GetFileStorageSettings();

    #endregion Public Methods
}