namespace OttApiPlatform.WebAPI.Hubs;

//[Authorize]
public class VideoHub : Hub
{
    //ref: https://newbedev.com/call-signalr-core-hub-method-from-controller

    #region Private Fields

    private readonly ISignalRContextProvider _signalRContextProvider;

    public VideoHub(ISignalRContextProvider signalRContextProvider)
    {
        _signalRContextProvider = signalRContextProvider;
    }

    #endregion Private Fields

    public static string ReceiveUpdateMethodName => "ReceiveVideoUpdate";

    public override async Task OnConnectedAsync()
    {
        Console.WriteLine("Connected!");

        if (Context.User?.Identity != null)
        {
            var name = _signalRContextProvider.GetUserName(Context);
            //await Groups.AddToGroupAsync(Context.ConnectionId, name);
        }

        //await Test();
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        Console.WriteLine("Disconnected!");
    }

    public Task Test()
    {
        return Clients.Caller.SendAsync("ReceiveVideoUpdate", "Hello from Video Hub on backend!");
    }

    //public Task NotifyVideoCreationStatus(Guid? videoId, AssetCreationStatus status)
    //{
    //    return Clients.Caller.SendAsync(ReceiveUpdateMethodName, videoId, status);
    //}
}
