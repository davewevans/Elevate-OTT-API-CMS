namespace OttApiPlatform.WebAPI.Services.HubServices;

public class AssetHubNotificationService : IAssetHubNotificationService
{
    #region Private Fields

    private readonly IHubContext<AssetHub> _hubContext;
    private readonly ISignalRContextProvider _signalRContextProvider;

    public AssetHubNotificationService(IHubContext<AssetHub> hubContext, ISignalRContextProvider signalRContextProvider)
    {
        _hubContext = hubContext;
        _signalRContextProvider = signalRContextProvider;
    }

    #endregion Private Fields

    public async Task SendMessage(string user, string message)
    {
        await _hubContext.Clients.User(user).SendAsync("ReceiveMessage", message);
    }

    public async Task BroadcastMessage(string message)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveNotification", message);
    }

    public async Task NotifyCreationStatus(Guid assetId, AssetCreationStatus status)
    {
        //
        // TODO send to user instead of all
        //
        // Update client with new status via SignalR
        //var userNameIdentifier = _signalRContextProvider.GetUserNameIdentifier(Context);
      
        //_signalRContextProvider.
        //var user = _signalRContextProvider.GetUserName()
        //var tenantId = _signalRContextProvider.GetTenantId();
        //await _hubContext.Clients.Group(user).SendAsync("ReceiveAssetUpdate", assetId, status);
    }
}
