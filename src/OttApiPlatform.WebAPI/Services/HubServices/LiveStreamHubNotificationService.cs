namespace OttApiPlatform.WebAPI.Services.HubServices;

public class LiveStreamHubNotificationService : ILiveStreamHubNotificationService
{
    #region Private Fields

    private readonly IHubContext<LiveStreamHub> _hubContext;

    public LiveStreamHubNotificationService(IHubContext<LiveStreamHub> hubContext)
    {
        _hubContext = hubContext;
    }

    #endregion Private Fields
}
