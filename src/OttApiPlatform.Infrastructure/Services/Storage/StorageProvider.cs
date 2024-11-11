namespace OttApiPlatform.Infrastructure.Services.Storage;

public class StorageProvider : IStorageProvider
{
    #region Private Fields

    private readonly IStorageFactory _storageFactory;
    private readonly IAppSettingsReaderService _appSettingsReaderService;

    #endregion Private Fields

    #region Public Constructors

    public StorageProvider(IStorageFactory storageFactory, IAppSettingsReaderService appSettingsReaderService)
    {
        _storageFactory = storageFactory;
        _appSettingsReaderService = appSettingsReaderService;
    }

    #endregion Public Constructors

    #region Public Methods

    public async Task<IFileStorageService> InvokeInstanceAsync()
    {
        var storageTypeResponse = await _appSettingsReaderService.GetFileStorageSettings();
        var storageType = storageTypeResponse.Payload.StorageType;
        return _storageFactory.CreateInstance((StorageTypes)storageType);
    }

    public IFileStorageService InvokeInstanceForAzureStorage()
    {
        return _storageFactory.CreateInstance(StorageTypes.AzureStorageService);
    }

    #endregion Public Methods
}