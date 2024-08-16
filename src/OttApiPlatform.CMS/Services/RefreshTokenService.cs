namespace OttApiPlatform.CMS.Services;

public class RefreshTokenService : IRefreshTokenService
{
    #region Private Fields

    private readonly ILocalStorageService _localStorageService;
    private readonly IAuthenticationService _authenticationService;
    private readonly IAccountsClient _accountsClient;
    private readonly IHttpClientFactory _httpClientFactory;

    private readonly AuthStateProvider _authStateProvider;

    #endregion Private Fields

    #region Public Constructors

    public RefreshTokenService(ILocalStorageService localStorageService,
                               IAuthenticationService authenticationService,
                               IAccountsClient accountsClient,
                               AuthStateProvider authStateProvider,
                               IHttpClientFactory httpClientFactory)
    {
        _localStorageService = localStorageService;
        _authenticationService = authenticationService;
        _accountsClient = accountsClient;
        _authStateProvider = authStateProvider;
        _httpClientFactory = httpClientFactory;
    }

    #endregion Public Constructors

    #region Public Methods

    public async Task<string> TryRefreshToken()
    {
        var authState = await _authStateProvider.GetAuthenticationStateAsync();

        var user = authState.User;

        if (user.Identity is { IsAuthenticated: false })
            return string.Empty;

        var accessTokenExpiryDate = user.FindFirst(c => c.Type.Equals("exp"))?.Value;

        var refreshAt = user.FindFirst(c => c.Type.Equals("refreshAt"))?.Value;

        if (refreshAt == null)
            return string.Empty;

        var refreshTokenExpiryDate = long.Parse(refreshAt);

        var utcNowUnix = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
        if (accessTokenExpiryDate != null && long.Parse(accessTokenExpiryDate) <= utcNowUnix && refreshTokenExpiryDate > utcNowUnix)
            return await RefreshToken(utcNowUnix, refreshTokenExpiryDate);

        if (refreshTokenExpiryDate <= utcNowUnix)
            await _authenticationService.Logout();

        return string.Empty;
    }

    public async Task<string> RefreshToken(long utcNowUnix, long refreshTokenExpiryDate)
    {
        var token = await _localStorageService.GetItemAsync<string>(TokenType.AccessToken);

        var refreshToken = await _localStorageService.GetItemAsync<string>(TokenType.RefreshToken);

        var responseWrapper = await _accountsClient.RefreshToken(new RefreshTokenCommand { AccessToken = token, RefreshToken = refreshToken });

        if (responseWrapper.IsSuccessStatusCode)
        {
            await _localStorageService.SetItemAsync(TokenType.AccessToken, responseWrapper.Payload.AccessToken);
            await _localStorageService.SetItemAsync(TokenType.RefreshToken, responseWrapper.Payload.RefreshToken);
            using var httpClient = _httpClientFactory.CreateClient("DefaultClient");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", responseWrapper.Payload.AccessToken);
            return responseWrapper.Payload.AccessToken;
        }

        if (refreshTokenExpiryDate > utcNowUnix)
            return token;

        await _authenticationService.Logout();

        return string.Empty;
    }

    #endregion Public Methods
}