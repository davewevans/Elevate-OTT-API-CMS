namespace OttApiPlatform.WebAPI.Hubs;

//[Authorize]
public class LiveStreamHub : Hub
{
    #region Private Fields

    private readonly IBackgroundJobClient _backgroundJob;

    private readonly ISignalRContextProvider _signalRContextProvider;

    #endregion Private Fields

    public override async Task OnConnectedAsync()
    {
        Console.WriteLine("Connected!");

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        Console.WriteLine("Disconnected!");
    }

}
