namespace OttApiPlatform.CMS.Pages.Settings;

public partial class IdentitySettings
{
    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private IDialogService DialogService { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private IBreadcrumbService BreadcrumbService { get; set; }
    [Inject] private IAppSettingsClient AppSettingsClient { get; set; }

    private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }
    private IdentitySettingsForEdit IdentitySettingsForEditVm { get; set; } = new();
    private UpdateIdentitySettingsCommand UpdateIdentitySettingsCommand { get; set; }

    #endregion Private Properties

    #region Protected Methods

    protected override async Task OnInitializedAsync()
    {
        BreadcrumbService.SetBreadcrumbItems(new List<BreadcrumbItem>
        {
            new(Resource.Home, "/"),
            new(Resource.Settings, "#",true),
            new(Resource.Identity_Settings, "#",true)
        });

        var responseWrapper = await AppSettingsClient.GetIdentitySettings();

        if (responseWrapper.IsSuccessStatusCode)
        {
            var identitySettingsForEdit = responseWrapper.Payload;
            IdentitySettingsForEditVm = identitySettingsForEdit;
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
            UpdateIdentitySettingsCommand = new UpdateIdentitySettingsCommand
            {
                UserSettingsCommand = IdentitySettingsForEditVm.UserSettingsForEdit.MapToCommand(),
                LockoutSettingsCommand = IdentitySettingsForEditVm.LockoutSettingsForEdit.MapToCommand(),
                PasswordSettingsCommand = IdentitySettingsForEditVm.PasswordSettingsForEdit.MapToCommand(),
                SignInSettingsCommand = IdentitySettingsForEditVm.SignInSettingsForEdit.MapToCommand(),
            };

            var responseWrapper = await AppSettingsClient.UpdateIdentitySettings(UpdateIdentitySettingsCommand);

            if (responseWrapper.IsSuccessStatusCode)
            {
                IdentitySettingsForEditVm.UserSettingsForEdit.Id = responseWrapper.Payload.UserSettingsId;
                IdentitySettingsForEditVm.LockoutSettingsForEdit.Id = responseWrapper.Payload.LockoutSettingsId;
                IdentitySettingsForEditVm.PasswordSettingsForEdit.Id = responseWrapper.Payload.PasswordSettingsId;
                IdentitySettingsForEditVm.SignInSettingsForEdit.Id = responseWrapper.Payload.SignInSettingsId;
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