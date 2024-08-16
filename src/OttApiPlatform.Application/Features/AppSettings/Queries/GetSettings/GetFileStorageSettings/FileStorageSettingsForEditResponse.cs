namespace OttApiPlatform.Application.Features.AppSettings.Queries.GetSettings.GetFileStorageSettings;

public class FileStorageSettingsForEditResponse
{
    #region Public Properties

    public string Id { get; set; }
    public int StorageType { get; set; }

    #endregion Public Properties

    #region Public Methods

    public static FileStorageSettingsForEditResponse MapFromEntity(FileStorageSettings fileStorageSettings)
    {
        return new()
        {
            Id = fileStorageSettings.Id.ToString(),
            StorageType = (int)fileStorageSettings.StorageType
        };
    }

    #endregion Public Methods
}