namespace OttApiPlatform.CMS.Shared
{
    public partial class Dashboard : IAsyncDisposable
    {
        #region Private Fields

        private readonly EarningReport[] _earningReports =
        {
            new()
            {
                Name = "Lunees", Title = "Reactor Engineer",
                Avatar =
                    "https://avatars2.githubusercontent.com/u/71094850?s=460&u=66c16f5bb7d27dc751f6759a82a3a070c8c7fe4b&v=4",
                Salary = "$0.99", Severity = Color.Success, SeverityTitle = Resource.Low
            },
            new()
            {
                Name = "Mikes-gh", Title = "Developer",
                Avatar = "https://avatars.githubusercontent.com/u/16208742?s=120&v=4", Salary = "$19.12K",
                Severity = Color.Secondary, SeverityTitle = Resource.Medium
            },
            new()
            {
                Name = "Garderoben", Title = "CSS Magician",
                Avatar =
                    "https://avatars2.githubusercontent.com/u/10367109?s=460&amp;u=2abf95f9e01132e8e2915def42895ffe99c5d2c6&amp;v=4",
                Salary = "$1337", Severity = Color.Primary, SeverityTitle = Resource.High
            }
        };

        #endregion Private Fields

        #region Private Properties

        [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IBreadcrumbService BreadcrumbService { get; set; }
        [Inject] private IDashboardClient DashboardClient { get; set; }
        [Inject] private IApiUrlProvider ApiUrlProvider { get; set; }
        [Inject] private IAccessTokenProvider AccessTokenProvider { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }

        private HubConnection HubConnection { get; set; }
        private HeadlinesResponse HeadlinesResponse { get; set; } = new();

        #endregion Private Properties

        #region Public Methods

        public async Task InitiateSignalRHub()
        {
            var responseWrapper = await DashboardClient.GetHeadlinesData();

            HeadlinesResponse = responseWrapper.Payload;

            if (responseWrapper.IsSuccessStatusCode)
            {
                await StartHubConnection();
                HubConnection.On<HeadlinesResponse>("SendHeadlinesData", (data) =>
                                                                         {
                                                                             HeadlinesResponse = data;
                                                                             StateHasChanged();
                                                                         });
            }
            else
            {
                SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (HubConnection is not null && HubConnection.State == HubConnectionState.Connected)
                try
                {
                    await HubConnection.StopAsync();
                }
                finally
                {
                    await HubConnection.DisposeAsync();
                    Snackbar.Add(Resource.Dashboard_Hub_is_closed, Severity.Info);
                }
        }

        #endregion Public Methods

        #region Protected Methods

        protected override async Task OnInitializedAsync()
        {
            BreadcrumbService.SetBreadcrumbItems(new List<BreadcrumbItem>
                                                 {
                                                     new(Resource.Home, "/"),
                                                     new(Resource.Dashboard, "#", true)
                                                 });

            await InitiateSignalRHub();
        }

        #endregion Protected Methods

        #region Private Methods

        private async Task StartHubConnection()
        {
            Snackbar.Add(Resource.Dashboard_Hub_is_being_initialized, Severity.Info);

            if (HubConnection is null || HubConnection.State == HubConnectionState.Disconnected)
            {
                var subDomain = NavigationManager.GetSubDomain();

                HubConnection = new HubConnectionBuilder().WithUrl($"{ApiUrlProvider.BaseHubUrl}/Hubs/DashboardHub?Bp-Tenant={subDomain}&Accept-Language={CultureInfo.CurrentCulture}",
                                                                   options =>
                                                                   {
                                                                       //options.Headers.Add("Bp-Tenant", subDomain); //Doesn't Work
                                                                       options.AccessTokenProvider = () => AccessTokenProvider.TryGetAccessToken();
                                                                   })
                                                          .Build();

                try
                {
                    await HubConnection.StartAsync();
                }
                catch (Exception ex)
                {
                    Snackbar.Add(string.Format(Resource.Unable_to_connect_to_the_dashboard_hub_due_to_an_error, ex.Message), Severity.Error);
                }

                if (HubConnection.State == HubConnectionState.Connected)
                    Snackbar.Add(Resource.Dashboard_Hub_is_now_connected, Severity.Success);

                HubConnection.Closed += OnHubConnectionOnClosed;
            }
        }

        private Task OnHubConnectionOnClosed(Exception ex)
        {
            switch (ex)
            {
                case null:
                    Snackbar.Add(Resource.Dashboard_Hub_is_closed, Severity.Info);
                    break;

                default:
                    Snackbar.Add(string.Format(Resource.Unable_to_connect_to_the_dashboard_hub_due_to_an_error, ex.Message), Severity.Error);
                    break;
            }

            return Task.CompletedTask;
        }

        #endregion Private Methods
    }
}