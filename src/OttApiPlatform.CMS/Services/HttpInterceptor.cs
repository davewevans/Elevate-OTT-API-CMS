namespace OttApiPlatform.CMS.Services;

// ref: https://github.com/jsakamoto/Toolbelt.Blazor.HttpClientInterceptor

public class HttpInterceptor : DelegatingHandler
{
    #region Private Fields

    private readonly NavigationManager _navigationManager;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IAccessTokenProvider _accessTokenProvider;
    private readonly IAppStateManager _appStateManager;
    private readonly IReturnUrlProvider _returnUrlProvider;

    private CancellationTokenSource _tokenSource;

    #endregion Private Fields

    #region Public Constructors

    public HttpInterceptor(NavigationManager navigationManager,
                           IRefreshTokenService refreshTokenService,
                           IAccessTokenProvider accessTokenProvider,
                           IAppStateManager appStateManager,
                           IReturnUrlProvider returnUrlProvider)
    {
        _navigationManager = navigationManager;
        _refreshTokenService = refreshTokenService;
        _accessTokenProvider = accessTokenProvider;
        _appStateManager = appStateManager;

        _tokenSource = new CancellationTokenSource();

        _appStateManager.TokenSourceChanged += OnAppStateManagerOnTokenSourceChanged;
        _returnUrlProvider = returnUrlProvider;
    }

    private void OnAppStateManagerOnTokenSourceChanged(object obj, EventArgs args)
    {
        _tokenSource.Cancel();
        _tokenSource = new CancellationTokenSource();
    }

    #endregion Public Constructors

    #region Protected Methods

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        _appStateManager.OverlayVisible = true;

        await AddRequiredHeaders(request);

        var response = await base.SendAsync(request, _tokenSource.Token);

        if (response.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
        {
            await _returnUrlProvider.SetReturnUrl(_navigationManager.Uri);
            _navigationManager.NavigateTo("/pages/error/401");
        }

        _appStateManager.OverlayVisible = false;

        return response;
    }

    private async Task AddRequiredHeaders(HttpRequestMessage request)
    {
        var subDomain = _navigationManager.GetSubDomain();

        if (subDomain != null)
            request.Headers.Add("X-Tenant", subDomain);

        var accessToken = await _accessTokenProvider.TryGetAccessToken();

        request.Headers.Authorization = new AuthenticationHeaderValue("bearer", accessToken);

        if (!string.IsNullOrWhiteSpace(accessToken))
        {
            var absPath = request.RequestUri?.AbsolutePath;
            if (absPath != null && !absPath.Contains("/api/account/"))
            {
                accessToken = await _refreshTokenService.TryRefreshToken();
                if (!string.IsNullOrEmpty(accessToken))
                    request.Headers.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
            }
        }

        request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue(CultureInfo.CurrentCulture.ToString()));
    }
}

#endregion Protected Methods
