namespace OttApiPlatform.Gateway.Providers;

public class TenantUrlProvider : ITenantUrlProvider
{
    #region Private Fields

    private readonly UrlOptions _optionsSnapshot;

    #endregion Private Fields

    #region Public Constructors

    public TenantUrlProvider(IOptionsSnapshot<UrlOptions> optionsSnapshot)
    {
        _optionsSnapshot = optionsSnapshot.Value;
    }

    #endregion Public Constructors

    #region Public Properties

    public string TenantUrl => _optionsSnapshot.TenantUrl;

    #endregion Public Properties
}