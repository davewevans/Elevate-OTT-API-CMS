namespace OttApiPlatform.Infrastructure.Services.Storage;

public class StorageFactory : IStorageFactory
{
    #region Private Fields

    // Defines a private readonly field to store the mapping of StorageTypes to IFileStorageService instances.
    private readonly Dictionary<StorageTypes, IFileStorageService> _storageServicesByType;

    // Defines a private readonly field to store all available IFileStorageService instances.
    private readonly IEnumerable<IFileStorageService> _fileStorageServices;

    #endregion Private Fields

    #region Public Constructors

    public StorageFactory(IEnumerable<IFileStorageService> fileStorageServices)
    {
        // Initializes the _fileStorageServices field with the IEnumerable passed as a parameter.
        _fileStorageServices = fileStorageServices ?? throw new ArgumentNullException(nameof(fileStorageServices));

        // Initializes the _storageServicesByType field with a dictionary that maps StorageTypes to
        // the corresponding IFileStorageService instances.
        _storageServicesByType = Enum.GetValues<StorageTypes>()
                                     .ToDictionary(
                                                   storageType => storageType,
                                                   storageType => _fileStorageServices.FirstOrDefault(s => s.GetType().Name == storageType.ToString())
                                                  );
    }

    #endregion Public Constructors

    #region Public Methods

    public IFileStorageService CreateInstance(StorageTypes storageType)
    {
        // Check if the specified storage type exists in the dictionary of available storage services.
        if (!_storageServicesByType.ContainsKey(storageType))
            // If the storage type does not exist, throw an ArgumentException with a message
            // indicating the invalid storage type and the name of the parameter.
            throw new ArgumentException($@"Unknown storage type '{storageType}'.", nameof(storageType));

        // If the storage type exists, return the corresponding storage service instance.
        return _storageServicesByType[storageType];
    }

    #endregion Public Methods
}