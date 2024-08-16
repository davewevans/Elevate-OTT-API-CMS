namespace OttApiPlatform.CMS.Pages.POC.Army;

public partial class Applicants : IAsyncDisposable
{
    #region Public Properties

    public int ActivePanelIndex { get; set; } = 0;

    #endregion Public Properties

    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private IAccessTokenProvider AccessTokenProvider { get; set; }
    [Inject] private IApiUrlProvider ApiUrlProvider { get; set; }
    [Inject] private IApplicantsClient ApplicantsClient { get; set; }
    [Inject] private IBreadcrumbService BreadcrumbService { get; set; }
    [Inject] private IDialogService DialogService { get; set; }
    [Inject] private IJSRuntime JsRuntime { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }

    private string SearchString { get; set; }
    private bool IsHubConnectionClosed { get; set; }
    private ApplicantsResponse ApplicantsResponse { get; set; }
    private GetApplicantsQuery GetApplicantsQuery { get; set; } = new();
    private HubConnection HubConnection { get; set; }
    private MudTable<ApplicantItem> Table { get; set; }

    #endregion Private Properties

    #region Public Methods

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
                Snackbar.Add(Resource.Reporting_Hub_is_closed, Severity.Info);
            }
    }

    public async Task CloseHubConnection()
    {
        ActivePanelIndex = 0;

        if (HubConnection is null)
            return;

        switch (HubConnection.State)
        {
            case HubConnectionState.Connected:
                try
                {
                    await HubConnection.StopAsync();
                }
                finally
                {
                    await HubConnection.DisposeAsync();
                    IsHubConnectionClosed = true;
                    Snackbar.Add(Resource.Reporting_Hub_is_closed, Severity.Info);
                }
                break;

            case HubConnectionState.Disconnected:
                Snackbar.Add(Resource.Reporting_Hub_is_already_closed, Severity.Success);
                break;
        }
    }

    #endregion Public Methods

    #region Protected Methods

    protected override async Task OnInitializedAsync()
    {
        BreadcrumbService.SetBreadcrumbItems(new List<BreadcrumbItem>
        {
            new(Resource.Home, "/"),
            new(Resource.Proof_of_Concepts, "#", true),
            new(Resource.Applicants, "#", true)
        });

        await StartHubConnection();

        HubConnection.On("NotifyReportIssuer", (Func<FileMetaData, ReportStatus, Task>)(async (fileMetaData, reportStatus) =>
        {
            switch (reportStatus)
            {
                case ReportStatus.Pending:
                    Snackbar.Add(Resource.Your_report_is_being_initiated, Severity.Info);
                    break;

                case ReportStatus.InProgress:
                    Snackbar.Add(Resource.Your_report_is_being_generated, Severity.Warning);
                    break;

                case ReportStatus.Completed:
                    Snackbar.Add(string.Format(Resource.Your_report_0_is_ready_to_download, fileMetaData.FileName), Severity.Success);
                    await ShowDownloadFileDialogue(fileMetaData);
                    break;

                case ReportStatus.Failed:
                    Snackbar.Add(Resource.Your_report_generation_has_failed, Severity.Error);
                    break;

                case ReportStatus.NotFound:
                    Snackbar.Add(Resource.There_are_no_data_to_generate_report, Severity.Error);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(reportStatus), reportStatus, null);
            }
        }));
    }

    #endregion Protected Methods

    #region Private Methods

    private void AddApplicant()
    {
        NavigationManager.NavigateTo("poc/army/addApplicant");
    }

    private void EditApplicant(string id)
    {
        NavigationManager.NavigateTo($"poc/army/editApplicant/{id}");
    }

    private void ViewApplicant(string id)
    {
        NavigationManager.NavigateTo($"poc/army/viewApplicant/{id}");
    }

    private async Task DeleteApplicant(string id)
    {
        var dialog = await DialogService.ShowAsync<DeleteConfirmationDialog>(Resource.Delete);

        var dialogResult = await dialog.Result;

        if (!dialogResult.Canceled)
        {
            var responseWrapper = await ApplicantsClient.DeleteApplicant(id);

            if (responseWrapper.IsSuccessStatusCode)
            {
                Snackbar.Add(responseWrapper.Payload, Severity.Success);
                await Table.ReloadServerData();
            }
            else
            {
                SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
            }
        }
    }

    private void FilterApplicants(string searchString)
    {
        if (ApplicantsResponse is null)
            return;

        SearchString = searchString;

        Table.ReloadServerData();
    }

    private async Task ExportApplicantToPdfOnDemand()
    {
        var dialogParameters = new DialogParameters
            {
                {"ContentText", Resource.Exporting_data_may_take_a_while},
                {"ButtonText", Resource.ExportAsPdfOnDemand},
                {"Color", Color.Error}
            };

        var dialogOptions = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

        var dialog = await DialogService.ShowAsync<GenericDialog>(Resource.Export, dialogParameters, dialogOptions);

        var dialogResult = await dialog.Result;

        if (!dialogResult.Canceled)
        {
            var responseWrapper = await ApplicantsClient.ExportAsPdf(new ExportApplicantsQuery
            {
                SearchText = GetApplicantsQuery.SearchText,
                SortBy = GetApplicantsQuery.SortBy,
                IsOnDemand = true
            });

            if (responseWrapper.IsSuccessStatusCode)
            {
                Snackbar.Add(responseWrapper.Payload.FileUrl, Severity.Success);
                await JsRuntime.InvokeVoidAsync("triggerFileDownload", responseWrapper.Payload.FileName, responseWrapper.Payload.FileUrl);
            }
            else
            {
                SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
            }
        }
    }

    private async Task ExportApplicantToPdfInBackground()
    {
        ActivePanelIndex = 0;

        var dialogParameters = new DialogParameters
        {
            {"ContentText", Resource.Exporting_data_may_take_a_while},
            {"ButtonText", Resource.ExportAsPdfInBackground},
            {"Color", Color.Error}
        };

        if (HubConnection.State == HubConnectionState.Connected)
        {
            var dialogOptions = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

            var dialog = await DialogService.ShowAsync<GenericDialog>(Resource.Export, dialogParameters, dialogOptions);

            var dialogResult = await dialog.Result;

            if (!dialogResult.Canceled)
            {
                await HubConnection.SendAsync("ExportApplicantToPdf", new ExportApplicantsQuery
                {
                    SearchText = GetApplicantsQuery.SearchText,
                    SortBy = GetApplicantsQuery.SortBy,
                    IsOnDemand = false
                });

                ActivePanelIndex = 1;
            }
        }
        else
        {
            Snackbar.Add(Resource.Reporting_Hub_is_not_active, Severity.Warning);
        }
    }

    private async Task<TableData<ApplicantItem>> ServerReload(TableState state)
    {
        ActivePanelIndex = 0;

        GetApplicantsQuery.SearchText = SearchString;

        GetApplicantsQuery.PageNumber = state.Page + 1;

        GetApplicantsQuery.RowsPerPage = state.PageSize;

        GetApplicantsQuery.SortBy = state.SortDirection == SortDirection.None ? string.Empty : $"{state.SortLabel} {state.SortDirection}";

        var responseWrapper = await ApplicantsClient.GetApplicants(GetApplicantsQuery);

        var tableData = new TableData<ApplicantItem>();

        if (responseWrapper.IsSuccessStatusCode)
        {
            if (responseWrapper.Payload != null)
                ApplicantsResponse = responseWrapper.Payload;

            tableData = new TableData<ApplicantItem>()
            {
                TotalItems = ApplicantsResponse.Applicants.TotalRows,
                Items = ApplicantsResponse.Applicants.Items
            };
        }
        else
        {
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }

        return tableData;
    }

    private async Task ShowDownloadFileDialogue(FileMetaData fileMetaData)
    {
        var dialogParameters = new DialogParameters
        {
            {"ContentText", Resource.Your_report_is_ready_to_download},
            {"ButtonText", Resource.Download},
            {"Color", Color.Error}
        };

        var dialogOptions = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

        var dialog = await DialogService.ShowAsync<GenericDialog>(Resource.Export, dialogParameters, dialogOptions);

        var dialogResult = await dialog.Result;

        if (!dialogResult.Canceled)
            await JsRuntime.InvokeVoidAsync("triggerFileDownload", fileMetaData.FileName, fileMetaData.FileUri);
    }

    private async Task StartHubConnection()
    {
        if (HubConnection is null || HubConnection.State == HubConnectionState.Disconnected)
        {
            Snackbar.Add(Resource.Reporting_Hub_is_being_initialized, Severity.Info);

            var subDomain = NavigationManager.GetSubDomain();

            HubConnection = new HubConnectionBuilder().WithUrl($"{ApiUrlProvider.BaseHubUrl}/Hubs/DataExportHub?Bp-Tenant={subDomain}&Accept-Language={CultureInfo.CurrentCulture}",
                                                               options =>
                                                               {
                                                                   //options.Headers.Add("Bp-Tenant", subDomain); //Doesn't Work
                                                                   //options.Headers.Add("Accept-Language", culture); //Doesn't Work.
                                                                   options.AccessTokenProvider = () => AccessTokenProvider.TryGetAccessToken();
                                                               }).Build();

            try
            {
                await HubConnection.StartAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add(string.Format(Resource.Unable_to_connect_to_the_reporting_hub_due_to_an_error, ex.Message), Severity.Error);

            }

            if (HubConnection.State == HubConnectionState.Connected)
            {
                IsHubConnectionClosed = false;
                Snackbar.Add(Resource.Reporting_Hub_is_now_connected, Severity.Success);
            }

            HubConnection.Closed += OnHubConnectionClosed;
        }
    }

    private async Task OnHubConnectionToggledChanged(bool toggled)
    {
        if (toggled)
            await CloseHubConnection();
        else
            await StartHubConnection();
    }

    private Task OnHubConnectionClosed(Exception exception)
    {
        switch (exception)
        {
            case null:
                Snackbar.Add(Resource.Reporting_Hub_is_closed, Severity.Info);
                break;

            default:
                Snackbar.Add($"Reporting Hub connection closed due to an error: {exception.Message}", Severity.Error);
                IsHubConnectionClosed = true;
                break;
        }

        return Task.CompletedTask;
    }

    #endregion Private Methods
}