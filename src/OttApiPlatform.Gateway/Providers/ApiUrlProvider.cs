namespace OttApiPlatform.Gateway.Providers;

public class ApiUrlProvider : IApiUrlProvider
{
    #region Private Fields

    private readonly UrlOptions _optionsSnapshot;

    #endregion Private Fields

    #region Public Constructors

    public ApiUrlProvider(IOptions<UrlOptions> optionsSnapshot)
    {
        _optionsSnapshot = optionsSnapshot.Value;
    }

    #endregion Public Constructors

    #region Public Properties

    public string BaseUrl => _optionsSnapshot.BaseApiUrl;

    #endregion Public Properties
}