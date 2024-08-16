namespace OttApiPlatform.CMS.Consumers;

public class DashboardClient : IDashboardClient
{
    #region Private Fields

    private readonly IHttpService _httpService;

    #endregion Private Fields

    #region Public Constructors

    public DashboardClient(IHttpService httpService)
    {
        _httpService = httpService;
    }

    #endregion Public Constructors

    #region Public Methods

    public async Task<ApiResponseWrapper<HeadlinesResponse>> GetHeadlinesData()
    {
        return await _httpService.Post<HeadlinesResponse>("Dashboard/GetHeadlinesData");
    }

    #endregion Public Methods
}