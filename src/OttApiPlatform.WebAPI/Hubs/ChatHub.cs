namespace OttApiPlatform.WebAPI.Hubs;

public class ChatHub : Hub
{
    private static Dictionary<string, string> Users = new Dictionary<string, string>();

    public override async Task OnConnectedAsync()
    {
        Console.WriteLine("Connected");

        string username = Context.GetHttpContext().Request.Query["username"];
         Users.Add(Context.ConnectionId, username);
        await AddMessageToChat(string.Empty, $"{username} joined the party!");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        Console.WriteLine("Disconnected");

        string username = Users.FirstOrDefault(u => u.Key == Context.ConnectionId).Value;
        await AddMessageToChat(string.Empty, $"{username} left!");
    }

    public Task AddMessageToChat(string user, string message)
    {
        return Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}
