namespace OttApiPlatform.CMS.Services;

public class AuthenticationService : IAuthenticationService
{
    #region Private Fields

    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly ILocalStorageService _localStorageService;

    #endregion Private Fields

    #region Public Constructors

    public AuthenticationService(AuthenticationStateProvider authStateProvider,
                                 ILocalStorageService localStorageService)
    {
        _authStateProvider = authStateProvider;
        _localStorageService = localStorageService;
    }

    #endregion Public Constructors

    #region Public Methods

    public async Task Login(AuthResponse authResponse)
    {
        await Logout();

        await SetTokens(authResponse);
        ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(authResponse.AccessToken);
    }

    public async Task ReAuthenticate(AuthResponse authResponse)
    {
        await SetTokens(authResponse);

        ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(authResponse.AccessToken);
    }

    public async Task Logout()
    {
        var authState = await ((AuthStateProvider)_authStateProvider).GetAuthenticationStateAsync();

        var user = authState.User;

        if (user.Identity is { IsAuthenticated: true })
        {
            await ClearTokens();
            ((AuthStateProvider)_authStateProvider).NotifyUserLogout();
        }
    }

    #endregion Public Methods

    #region Private Methods

    private async Task SetTokens(AuthResponse authResponse)
    {
        await _localStorageService.RemoveItemAsync(TokenType.AccessToken);

        await _localStorageService.RemoveItemAsync(TokenType.RefreshToken);

        await _localStorageService.SetItemAsync(TokenType.AccessToken, authResponse.AccessToken);

        await _localStorageService.SetItemAsync(TokenType.RefreshToken, authResponse.RefreshToken);
    }

    private async Task ClearTokens()
    {
        await _localStorageService.RemoveItemAsync(TokenType.AccessToken);

        await _localStorageService.RemoveItemAsync(TokenType.RefreshToken);
    }

    #endregion Private Methods
}