namespace OttApiPlatform.WebAPI.Hubs;

[Authorize]
public class AssetHub : Hub
{
    private readonly ISignalRContextProvider _signalRContextProvider;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AssetHub(ISignalRContextProvider signalRContextProvider, IHttpContextAccessor httpContextAccessor) 
    {
        _signalRContextProvider = signalRContextProvider;
        _httpContextAccessor = httpContextAccessor;
    }

    public override async Task OnConnectedAsync()
    {
        
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        
    }
}
