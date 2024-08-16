namespace OttApiPlatform.WebAPI.Controllers;

[Route("api/[controller]")]
[BpAuthorize]
public class DashboardController : ApiController
{
    #region Private Fields

    private readonly IHubContext<DashboardHub> _dashboardHubContext;
    private readonly TimerManager _timerManager;

    #endregion Private Fields

    #region Public Constructors

    public DashboardController(IHubContext<DashboardHub> dashboardHubContext, TimerManager timerManager)
    {
        _dashboardHubContext = dashboardHubContext;
        _timerManager = timerManager;
    }

    #endregion Public Constructors

    #region Public Methods

    [HttpPost("GetHeadlinesData")]
    public async Task<IActionResult> GetHeadlinesData()
    {
        var response = await Mediator.Send(new GetHeadlinesQuery());

        _timerManager.PrepareTimer(() => _dashboardHubContext.Clients.All.SendAsync("SendHeadlinesData", response.Payload));

        return TryGetResult(response);
    }

    #endregion Public Methods
}