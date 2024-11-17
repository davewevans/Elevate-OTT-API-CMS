namespace OttApiPlatform.CMS.Hubs;

public class VideoHub
{
    public HubConnection? HubConnection;

    private readonly HttpClient _httpClient;
    
    private const string HubPath = "Hubs/VideoHub";

    public VideoHub(IHttpClientFactory clientFactory)
    {
        _httpClient = clientFactory.CreateClient("HubUrl");
    }

    public void Connect()
    {
        string hubUrl = $"{_httpClient.BaseAddress}{HubPath}";

        Console.WriteLine("hub url: " + hubUrl);

        HubConnection = new HubConnectionBuilder()
            .ConfigureLogging(opts => opts.SetMinimumLevel(LogLevel.Trace))
            .WithUrl(hubUrl)
            .WithAutomaticReconnect()
            .Build();
    }
}
