namespace OttApiPlatform.CMS.Pages.Account;

public partial class Logout
{
    #region Private Properties

    [Inject] private IAuthenticationService AuthenticationService { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }

    #endregion Private Properties

    #region Protected Methods

    protected override async Task OnInitializedAsync()
    {
        await AuthenticationService.Logout();
        NavigationManager.NavigateTo("/account/login");
    }

    #endregion Protected Methods
}