namespace OttApiPlatform.CMS.Pages.Settings;

public partial class TokenSettings
{
    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private IDialogService DialogService { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private IBreadcrumbService BreadcrumbService { get; set; }
    [Inject] private IAppSettingsClient AppSettingsClient { get; set; }

    private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }
    private TokenSettingsForEdit TokenSettingsForEditVm { get; set; } = new();
    private UpdateTokenSettingsCommand UpdateTokenSettingsCommand { get; set; }

    #endregion Private Properties

    #region Protected Methods

    protected override async Task OnInitializedAsync()
    {
        BreadcrumbService.SetBreadcrumbItems(new List<BreadcrumbItem>
        {
            new(Resource.Home, "/"),
            new(Resource.Settings, "#",true),
            new(Resource.Token_Settings, "#",true)
        });

        var responseWrapper = await AppSettingsClient.GetTokenSettings();

        if (responseWrapper.IsSuccessStatusCode)
        {
            TokenSettingsForEditVm = responseWrapper.Payload;
            TokenSettingsForEditVm.AccessTokenUoT = 0;
            TokenSettingsForEditVm.RefreshTokenUoT = 0;
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
            if (TokenSettingsForEditVm.AccessTokenTimeSpan != null)
            {
                var time = TokenSettingsForEditVm.AccessTokenTimeSpan.Value;
                switch ((UnitOfTime)TokenSettingsForEditVm.AccessTokenUoT)
                {
                    case UnitOfTime.Hours:
                        TokenSettingsForEditVm.AccessTokenTimeSpan = TimeSpan.FromHours(time).TotalMinutes;
                        break;

                    case UnitOfTime.Days:
                        TokenSettingsForEditVm.AccessTokenTimeSpan = TimeSpan.FromDays(time).TotalMinutes;
                        break;

                    case UnitOfTime.Month:
                        time *= 30;
                        TokenSettingsForEditVm.AccessTokenTimeSpan = TimeSpan.FromDays(time).TotalMinutes;
                        break;

                    case UnitOfTime.Minutes:
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (TokenSettingsForEditVm.RefreshTokenTimeSpan != null)
            {
                var time = TokenSettingsForEditVm.RefreshTokenTimeSpan.Value;
                switch ((UnitOfTime)TokenSettingsForEditVm.RefreshTokenUoT)
                {
                    case UnitOfTime.Hours:
                        TokenSettingsForEditVm.RefreshTokenTimeSpan = TimeSpan.FromHours(time).TotalMinutes;
                        break;

                    case UnitOfTime.Days:
                        TokenSettingsForEditVm.RefreshTokenTimeSpan = TimeSpan.FromDays(time).TotalMinutes;
                        break;

                    case UnitOfTime.Month:
                        time *= 30;
                        TokenSettingsForEditVm.RefreshTokenTimeSpan = TimeSpan.FromDays(time).TotalMinutes;
                        break;

                    case UnitOfTime.Minutes:
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            UpdateTokenSettingsCommand = new UpdateTokenSettingsCommand
            {
                Id = TokenSettingsForEditVm.Id,
                AccessTokenTimeSpan = TokenSettingsForEditVm.AccessTokenTimeSpan,
                RefreshTokenUoT = TokenSettingsForEditVm.RefreshTokenUoT,
                AccessTokenUoT = TokenSettingsForEditVm.AccessTokenUoT,
                RefreshTokenTimeSpan = TokenSettingsForEditVm.RefreshTokenTimeSpan,
            };

            var responseWrapper = await AppSettingsClient.UpdateTokenSettings(UpdateTokenSettingsCommand);

            if (responseWrapper.IsSuccessStatusCode)
            {
                TokenSettingsForEditVm.AccessTokenUoT = 0;
                TokenSettingsForEditVm.RefreshTokenUoT = 0;

                TokenSettingsForEditVm.Id = responseWrapper.Payload.Id;
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