namespace OttApiPlatform.CMS.Pages.Settings;

public partial class FileStorageSettings
{
    #region Public Properties

    public bool StorageType { get; set; }

    #endregion Public Properties

    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private IDialogService DialogService { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private IBreadcrumbService BreadcrumbService { get; set; }
    [Inject] private IAppSettingsClient AppSettingsClient { get; set; }

    private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }
    private UpdateFileStorageSettingsCommand UpdateFileStorageSettingsCommand { get; set; }
    private FileStorageSettingsForEdit FileStorageSettingsForEditVm { get; set; } = new();

    #endregion Private Properties

    #region Public Methods

    public void OnToggledChanged(bool toggled)
    {
        // Because variable is not two-way bound, we need to update it manually.
        StorageType = toggled;
        FileStorageSettingsForEditVm.StorageType = Convert.ToInt32(StorageType);
    }

    #endregion Public Methods

    #region Protected Methods

    protected override async Task OnInitializedAsync()
    {
        BreadcrumbService.SetBreadcrumbItems(new List<BreadcrumbItem>
        {
            new(Resource.Home, "/"),
            new(Resource.Settings, "#",true),
            new(Resource.File_Storage_Settings, "#",true)
        });

        var responseWrapper = await AppSettingsClient.GetFileStorageSettings();

        if (responseWrapper.IsSuccessStatusCode)
        {
            FileStorageSettingsForEditVm = responseWrapper.Payload;
            StorageType = Convert.ToBoolean(FileStorageSettingsForEditVm.StorageType);
        }
        else
        {
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
    }

    #endregion Protected Methods

    #region Private Methods

    private async Task SubmitForm()
    {
        var dialog = await DialogService.ShowAsync<SaveConfirmationDialog>(Resource.Confirm);

        var dialogResult = await dialog.Result;

        if (!dialogResult.Canceled)
        {
            UpdateFileStorageSettingsCommand = new UpdateFileStorageSettingsCommand
            {
                Id = FileStorageSettingsForEditVm.Id,
                StorageType = FileStorageSettingsForEditVm.StorageType
            };

            var responseWrapper = await AppSettingsClient.UpdateFileStorageSettings(UpdateFileStorageSettingsCommand);

            if (responseWrapper.IsSuccessStatusCode)
            {
                FileStorageSettingsForEditVm.Id = responseWrapper.Payload.Id;
                Snackbar.Add(responseWrapper.Payload.SuccessMessage, Severity.Success);
            }
            else
            {
                EditContextApiExceptionFallback.PopulateFormErrors(responseWrapper.ApiErrorResponse);
                SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
            }
        }
    }

    #endregion Private Methods
}