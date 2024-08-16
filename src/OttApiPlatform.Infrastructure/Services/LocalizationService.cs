namespace OttApiPlatform.Infrastructure.Services;

public class LocalizationService : ILocalizationService
{
    #region Private Fields

    private readonly IStringLocalizer _localizer;

    #endregion Private Fields

    #region Public Constructors

    public LocalizationService(IStringLocalizerFactory factory)
    {
        // Gets the full name of the assembly containing the Resource class and creates an
        // AssemblyName object with that name.
        var assemblyName = new AssemblyName(typeof(Resource).GetTypeInfo().Assembly.FullName ?? throw new InvalidOperationException());

        // Creates an IStringLocalizer for the Resource class with the given assembly name using the
        // IStringLocalizerFactory passed as a parameter.
        _localizer = factory.Create(nameof(Resource), assemblyName.Name ?? throw new InvalidOperationException());
    }

    #endregion Public Constructors

    #region Public Methods

    public string GetString(string key)
    {
        // Returns the localized string corresponding to the given key.
        return _localizer[key];
    }

    #endregion Public Methods
}