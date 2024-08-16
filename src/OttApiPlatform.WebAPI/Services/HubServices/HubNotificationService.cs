namespace OttApiPlatform.WebAPI.Services.HubServices;

public class HubNotificationService : IHubNotificationService
{
    #region Private Fields

    private readonly IHubContext<DataExportHub> _hubContext;

    #endregion Private Fields

    #region Public Constructors

    public HubNotificationService(IHubContext<DataExportHub> hubContext)
    {
        _hubContext = hubContext;
    }

    #endregion Public Constructors

    #region Public Methods

    public async Task NotifyReportIssuer(string userNameIdentifier, FileMetaData fileMetaData, ReportStatus status)
    {
        // Send a notification to the specified user with the given user name identifier, passing in
        // the file metadata and report status.
        await _hubContext.Clients.User(userNameIdentifier).SendAsync("NotifyReportIssuer", fileMetaData, status);

        // TODO: Uncomment the following line to send the notification to all clients connected to the hub.
        //await _hubContext.Clients.All.SendAsync("NotifyReportSubscriber", $"Hi=>{userName}");
    }

    public async Task RefreshReportsViewer(string userNameIdentifier)
    {
        // Send a message to the specified user with the given user name identifier to refresh the
        // reports viewer.
        await _hubContext.Clients.User(userNameIdentifier).SendAsync("RefreshReportsViewer");
    }

    #endregion Public Methods
}