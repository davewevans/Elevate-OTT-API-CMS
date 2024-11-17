using OttApiPlatform.CMS.Features.ContentManagement.Categories.Queries.GetCategories;
using OttApiPlatform.CMS.Shared;
using OttApiPlatform.CMS.Utilities;

namespace OttApiPlatform.CMS.Pages.ContentManagement.Categories;

public partial class CategoriesPage : ComponentBase, IAsyncDisposable
{
    #region Private Properties
    public int ActivePanelIndex { get; set; } = 0;
    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private IAccessTokenProvider? AccessTokenProvider { get; set; }
    [Inject] private IApiUrlProvider? ApiUrlProvider { get; set; }
    [Inject] private ICategoriesClient? CategoriesClient { get; set; }
    [Inject] private IBreadcrumbService? BreadcrumbService { get; set; }
    [Inject] private IDialogService? DialogService { get; set; }
    [Inject] private IJSRuntime? Js { get; set; }
    [Inject] private ILocalStorageService? LocalStorage { get; set; }
    [Inject] private ISnackbar? Snackbar { get; set; }
    [Inject] private AuthenticationStateProvider? AuthenticationStateProvider { get; set; }
    [Inject] private NavigationManager? NavigationManager { get; set; }
    private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }

    private string SearchString { get; set; } = string.Empty;
    private CategoriesResponse? CategoriesResponse { get; set; }
    private GetCategoriesQuery GetCategoriesQuery { get; set; } = new();
    private HubConnection? HubConnection { get; set; }
    private MudTable<CategoryItem>? Table { get; set; }
    #endregion Private Properties

    #region Public Methods
    public async ValueTask DisposeAsync()
    {
        if (HubConnection is not null && HubConnection.State == HubConnectionState.Connected)
        {
            try
            {
                await HubConnection.StopAsync();
            }
            finally
            {
                await HubConnection.DisposeAsync();
                // Snackbar?.Add("Reporting Hub is closed.", Severity.Error);
            }
        }
    }
    #endregion Public Methods

    #region Protected Methods
    protected override async Task OnInitializedAsync()
    {
        //await ServerReload(new TableState
        //{
        //    Page = 1,
        //    PageSize = 10,
        //    SortLabel = "Title",
        //    SortDirection = SortDirection.Ascending,
        //});

        BreadcrumbService?.SetBreadcrumbItems(new List<BreadcrumbItem>
        {
            new(Resource.Home, "/"),
            new(Resource.Categories, "#", true)
        });

        var userIdentity = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Identity;

        if (userIdentity is { IsAuthenticated: true })
        {
            //await StartHubConnection();
            //HubConnection?.On("NotifyReportIssuer", (Func<FileMetaData, ReportStatus, Task>)(async
            //(fileMetaData, reportStatus) =>
            //{
            //    switch (reportStatus)
            //    {
            //        case ReportStatus.Pending:
            //            Snackbar?.Add(Resource.Your_report_is_being_initiated, Severity.Info);
            //            break;

            //        case ReportStatus.InProgress:
            //            Snackbar?.Add(Resource.Your_report_is_being_generated,
            //        Severity.Warning);
            //            break;

            //        case ReportStatus.Completed:
            //            Snackbar?.Add(
            //        string.Format(Resource.Your_report_0_is_ready_to_download, fileMetaData.FileName),
            //        Severity.Success);
            //            await ShowDownloadFileDialogue(fileMetaData, reportStatus);
            //            break;

            //        case ReportStatus.Failed:
            //            Snackbar?.Add(Resource.Your_report_generation_has_failed,
            //        Severity.Error);
            //            break;

            //        default:
            //            throw new ArgumentOutOfRangeException(nameof(reportStatus), reportStatus, null);
            //    }
            //}));
        }
    }
    #endregion Protected Methods

    #region Private Methods
    private void AddCategory()
    {
        NavigationManager?.NavigateTo("/content/add-category");
    }

    private void EditCategory(Guid id)
    {
        NavigationManager?.NavigateTo($"/content/category/{id}");
    }

    private async Task DeleteCategory(Guid id)
    {
        var parameters = new DialogParameters
        {
            {"ContentText", Resource.Do_you_really_want_to_delete_this_record},
            {"ButtonText", Resource.Delete},
            {"Color", Color.Error}
        };

        var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

        var dialog = DialogService?.Show<DialogModal>(Resource.Delete, parameters, options);

        var result = await dialog.Result;

        if (!result.Canceled)
        {
            var responseWrapper = await CategoriesClient.DeleteCategory(id);

            //if (ApiResponseWrapper.Success)
            //{
            //    var successResult = ApiResponseWrapper.Response as SuccessResult<string>;
            //    Snackbar?.Add(successResult?.Result, Severity.Success);
            //    await Table.ReloadServerData();
            //}
            //else
            //{
            //    var exceptionResult = ApiResponseWrapper.Response as ExceptionResult;
            //    ServerSideValidator?.Validate(exceptionResult);
            //}

            if (responseWrapper.IsSuccessStatusCode)
            {
                Snackbar.Add(responseWrapper.Payload, Severity.Success);
                await Table.ReloadServerData();
            }
            else
            {
                EditContextApiExceptionFallback.PopulateFormErrors(responseWrapper.ApiErrorResponse);
                SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
            }
        }
    }

    private void FilterCategories(string searchString)
    {
        SearchString = searchString;
        Table?.ReloadServerData();
    }

    private async Task<TableData<CategoryItem>> ServerReload(TableState state)
    {
        // TODO guard clause

        GetCategoriesQuery.SearchText = SearchString;

        GetCategoriesQuery.PageNumber = state.Page + 1;

        GetCategoriesQuery.RowsPerPage = state.PageSize;

        GetCategoriesQuery.SortBy = state.SortDirection == SortDirection.None ? string.Empty : $"{state.SortLabel} {state.SortDirection}";

        var responseWrapper = await CategoriesClient.GetCategories(GetCategoriesQuery);

        var tableData = new TableData<CategoryItem>();

        if (responseWrapper.IsSuccessStatusCode)
        {

            CategoriesResponse.Categories = responseWrapper.Payload.Categories;

            tableData = new TableData<CategoryItem>()
            {
                TotalItems = CategoriesResponse.Categories.TotalRows, 
                Items = CategoriesResponse.Categories.Items
            };
        }
        else
        {
            EditContextApiExceptionFallback.PopulateFormErrors(responseWrapper.ApiErrorResponse);
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }

        return tableData;
    }

    private async Task ShowDownloadFileDialogue(FileMetaData fileMetaData, ReportStatus reportStatus)
    {
        var parameters = new DialogParameters
        {
            {"ContentText", Resource.Your_report_is_ready_to_download},
            {"ButtonText", Resource.Download},
            {"Color", Color.Error}
        };

        var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

        var dialog = DialogService.Show<DialogModal>(Resource.Export, parameters, options);

        var result = await dialog.Result;

        if (!result.Canceled)
            await Js.InvokeVoidAsync("triggerFileDownload", fileMetaData.FileName, fileMetaData.FileUri);
    }

    private async Task StartHubConnection()
    {
        if (HubConnection is null || HubConnection.State == HubConnectionState.Disconnected)
        {
            Snackbar.Add("Reporting Hub is being initialed.", Severity.Info);

            var tenantId = await LocalStorage.GetItemAsync<string>(Constants.TenantIdStorageKey);
            var culture = await LocalStorage.GetItemAsync<string>("Culture");

            HubConnection = new HubConnectionBuilder()
                .WithUrl($"{ApiUrlProvider.BaseHubUrl}/Hubs/DataExportHub?X-Tenant={tenantId}&Accept-Language={culture}",
                    options =>
                    {
                        //options.Headers.Add("X-Tenant", subDomain); //Doesn't Work
                        //options.Headers.Add("Accept-Language", culture); //Doesn't Work
                        options.AccessTokenProvider = () => AccessTokenProvider.TryGetAccessToken();
                    }).Build();

            await HubConnection.StartAsync();
            Snackbar.Add("Reporting Hub is now connected.", Severity.Success);
        }
    }
    #endregion Private Methods
}
