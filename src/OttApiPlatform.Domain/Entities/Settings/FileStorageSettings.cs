namespace OttApiPlatform.Domain.Entities.Settings;

/// <summary>
/// Represents the settings for file storage.
/// </summary>
public class FileStorageSettings : ISettingsSchema, IMayHaveTenant
{
    #region Public Properties

    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the storage type used for file storage.
    /// </summary>
    public StorageTypes StorageType { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the tenant associated with the file storage settings.
    /// </summary>
    public Guid? TenantId { get; set; }

    #endregion Public Properties
}