namespace OttApiPlatform.CMS.Services;

public class LocalizationService : ILocalizationService
{
    #region Private Fields

    private readonly IStringLocalizer _localizer;

    #endregion Private Fields

    #region Public Constructors

    public LocalizationService(IStringLocalizerFactory factory)
    {
        var assemblyName = new AssemblyName(typeof(Resource).GetTypeInfo().Assembly.FullName ?? throw new InvalidOperationException());

        _localizer = factory.Create("OttApiPlatform.AppResources.Resource", assemblyName.Name ?? throw new InvalidOperationException());
    }

    #endregion Public Constructors

    #region Public Methods

    public string GetString(string key)
    {
        return _localizer[key];
    }

    #endregion Public Methods
}