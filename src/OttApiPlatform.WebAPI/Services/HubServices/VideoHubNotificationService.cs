using OttApiPlatform.Application.Common.Contracts.Hub;
using OttApiPlatform.Domain.Enums.Content;

namespace OttApiPlatform.WebAPI.Services.HubServices;

public class VideoHubNotificationService: IVideoHubNotificationService
{
    #region Private Fields

    private readonly IHubContext<VideoHub> _hubContext;
    private readonly ISignalRContextProvider _signalRContextProvider;

    public VideoHubNotificationService(IHubContext<VideoHub> hubContext, ISignalRContextProvider signalRContextProvider)
    {
        _hubContext = hubContext;
        _signalRContextProvider = signalRContextProvider;
    }

    #endregion Private Fields

    public async Task NotifyCreationStatus(Guid videoId, AssetCreationStatus status)
    {
        //
        // TODO send to user instead of all
        //
        // Update client with new status via SignalR
        //var userNameIdentifier = _signalRContextProvider.GetUserNameIdentifier(Context);
      
        //_signalRContextProvider.

        await _hubContext.Clients.All.SendAsync("ReceiveVideoUpdate", videoId, status);
    }
}
