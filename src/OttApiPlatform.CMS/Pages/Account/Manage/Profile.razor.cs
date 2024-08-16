namespace OttApiPlatform.CMS.Pages.Account.Manage;

public partial class Profile
{
    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private IManageClient ManageClient { get; set; }

    private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }
    private CurrentUserForEdit UserForEdit { get; set; }

    #endregion Private Properties

    #region Protected Methods

    protected override async Task OnInitializedAsync()
    {
        var responseWrapper = await ManageClient.GetUser();

        if (responseWrapper.IsSuccessStatusCode)
            UserForEdit = responseWrapper.Payload;
        else
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
    }

    #endregion Protected Methods

    #region Private Methods

    private async Task UpdateUserProfile()
    {
        var updateCurrentUserProfileCommand = new UpdateUserProfileCommand
        {
            Name = UserForEdit.Name,
            Surname = UserForEdit.Surname,
            JobTitle = UserForEdit.JobTitle,
            PhoneNumber = UserForEdit.PhoneNumber
        };

        var responseWrapper = await ManageClient.UpdateUserProfile(updateCurrentUserProfileCommand);

        if (responseWrapper.IsSuccessStatusCode)
        {
            Snackbar.Add(responseWrapper.Payload, Severity.Success);
        }
        else
        {
            EditContextApiExceptionFallback.PopulateFormErrors(responseWrapper.ApiErrorResponse);
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
    }

    #endregion Private Methods
}