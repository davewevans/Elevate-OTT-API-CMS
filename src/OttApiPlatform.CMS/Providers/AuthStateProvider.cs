namespace OttApiPlatform.CMS.Providers;

public class AuthStateProvider : AuthenticationStateProvider
{
    #region Private Fields

    private readonly AuthenticationState _anonymous;
    private readonly IAccessTokenProvider _accessTokenProvider;

    #endregion Private Fields

    #region Public Constructors

    public AuthStateProvider(IAccessTokenProvider accessTokenProvider)
    {
        _accessTokenProvider = accessTokenProvider;
        _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    #endregion Public Constructors

    #region Public Methods

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _accessTokenProvider.TryGetAccessToken();

        if (string.IsNullOrWhiteSpace(token))
            return _anonymous;

        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwtAuthType")));
    }

    public void NotifyUserAuthentication(string token)
    {
        var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwtAuthType"));

        var authState = Task.FromResult(new AuthenticationState(authenticatedUser));

        NotifyAuthenticationStateChanged(authState);
    }

    public void NotifyUserLogout()
    {
        var authState = Task.FromResult(_anonymous);

        NotifyAuthenticationStateChanged(authState);
    }

    #endregion Public Methods
}