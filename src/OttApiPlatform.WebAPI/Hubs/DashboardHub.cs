namespace OttApiPlatform.WebAPI.Hubs;

[Authorize]
public class DashboardHub : Hub
{
    #region Public Methods

    public override Task OnConnectedAsync()
    {
        return base.OnConnectedAsync();
    }

    #endregion Public Methods
}