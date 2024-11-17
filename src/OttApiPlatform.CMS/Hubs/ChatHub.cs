namespace OttApiPlatform.CMS.Hubs;

public class ChatHub
{
    public HubConnection? HubConnection;

    private readonly HttpClient _httpClient;

    private const string HubPath = "Hubs/ChatHub";

    public ChatHub(IHttpClientFactory clientFactory)
    {
        _httpClient = clientFactory.CreateClient("HubUrl");
    }

    public void Connect(string username)
    {
        string hubUrl = $"{_httpClient.BaseAddress}{HubPath}?username={username}";

        Console.WriteLine("hub url: " + hubUrl);

        HubConnection = new HubConnectionBuilder()
            .ConfigureLogging(opts => opts.SetMinimumLevel(LogLevel.Trace))
            .WithUrl(hubUrl)
            .WithAutomaticReconnect()
            .Build();
    }
}
