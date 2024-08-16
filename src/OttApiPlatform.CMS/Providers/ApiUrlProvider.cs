namespace OttApiPlatform.CMS.Providers;

public class ApiUrlProvider : IApiUrlProvider
{
    #region Private Fields

    private readonly IConfiguration _configuration;

    #endregion Private Fields

    #region Public Constructors

    public ApiUrlProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    #endregion Public Constructors

    #region Public Properties

    public string BaseUrl => _configuration["BaseApiUrl"];
    public string BaseHubUrl => _configuration["BaseApiUrl"]?.Split("/api/")[0];

    #endregion Public Properties
}