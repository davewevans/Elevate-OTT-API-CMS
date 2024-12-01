namespace OttApiPlatform.WebAPI.Controllers;

[Route("api/signalr")]
[ApiController]
public class SignalRDemoController : ControllerBase
{
    private readonly IHubContext<ChatHub> _chatHubContext;
    private readonly IHubContext<VideoHub> _videoHubContext;


    public SignalRDemoController(IHubContext<ChatHub> chatHubContext, IHubContext<VideoHub> videoHubContext)
    {
        _chatHubContext = chatHubContext;
        _videoHubContext = videoHubContext;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        await _chatHubContext.Clients.All.SendAsync("ReceiveMessage", "Olivia", "Hi from API!");
        await _videoHubContext.Clients.All.SendAsync("ReceiveVideoUpdate", Guid.Parse("31580c8f-0d29-47c7-718c-08da78973967"), AssetCreationStatus.Ready);
        //await _videoHubContext.Clients.All.SendAsync("ReceiveVideoUpdate", "Hello from the API!");


        return Ok();
    }

}
