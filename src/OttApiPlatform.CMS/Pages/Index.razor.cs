namespace OttApiPlatform.CMS.Pages;

public partial class Index
{
    #region Private Properties

    private HubConnection HubConnection { get; }

    [Inject] private ISnackbar Snackbar { get; set; }

    #endregion Private Properties

    #region Public Methods

    public async void CloseHubConnection()
    {
        if (HubConnection is not null && HubConnection.State == HubConnectionState.Connected)
            try
            {
                await HubConnection.StopAsync();
            }
            finally
            {
                await HubConnection.DisposeAsync();
                Snackbar.Add("Dashboard Hub is closed.", Severity.Info);
            }
    }

    #endregion Public Methods
}