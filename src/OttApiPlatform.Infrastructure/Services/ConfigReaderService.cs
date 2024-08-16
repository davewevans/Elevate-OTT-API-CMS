namespace OttApiPlatform.Infrastructure.Services;

public class ConfigReaderService : IConfigReaderService
{
    #region Private Fields

    private readonly AppOptions _appOptionsSnapshot;
    private readonly JwtOptions _jwtOptionsSnapshot;
    private readonly SmtpOption _smtpOptionSnapshot;
    private readonly ClientAppOptions _clientAppOptionsSnapshot;
    private readonly IHttpContextAccessor _httpContextAccessor;

    #endregion Private Fields

    #region Public Constructors

    public ConfigReaderService(IOptionsSnapshot<AppOptions> appOptionsSnapshot,
                               IOptionsSnapshot<JwtOptions> jwtOptionsSnapshot,
                               IOptionsSnapshot<ClientAppOptions> clientAppOptionsSnapshot,
                               IOptionsSnapshot<SmtpOption> smtpOptionSnapshot,
                               IHttpContextAccessor httpContextAccessor)
    {
        _appOptionsSnapshot = appOptionsSnapshot.Value;
        _jwtOptionsSnapshot = jwtOptionsSnapshot.Value;
        _clientAppOptionsSnapshot = clientAppOptionsSnapshot.Value;
        _smtpOptionSnapshot = smtpOptionSnapshot.Value;
        _httpContextAccessor = httpContextAccessor;
    }

    #endregion Public Constructors

    #region Public Methods

    public AppUserOptions GetAppUserOptions()
    {
        return _appOptionsSnapshot.AppIdentityOptions.AppUserOptions;
    }

    public AppPasswordOptions GetAppPasswordOptions()
    {
        return _appOptionsSnapshot.AppIdentityOptions.AppPasswordOptions;
    }

    public AppLockoutOptions GetAppLockoutOptions()
    {
        return _appOptionsSnapshot.AppIdentityOptions.AppLockoutOptions;
    }

    public AppSignInOptions GetAppSignInOptions()
    {
        return _appOptionsSnapshot.AppIdentityOptions.AppSignInOptions;
    }

    public AppTokenOptions GetAppTokenOptions()
    {
        return _appOptionsSnapshot.AppTokenOptions;
    }

    public AppFileStorageOptions GetAppFileStorageOptions()
    {
        return _appOptionsSnapshot.AppFileStorageOptions;
    }

    public JwtOptions GetJwtOptions()
    {
        return _jwtOptionsSnapshot;
    }

    public SmtpOption GetSmtpOption()
    {
        return _smtpOptionSnapshot;
    }

    public ClientAppOptions GetClientAppOptions()
    {
        return _clientAppOptionsSnapshot;
    }

    public AppTenantOptions GetAppTenantOptions()
    {
        return _appOptionsSnapshot.AppTenantOptions;
    }

    public string GetSubDomain()
    {
        return _httpContextAccessor.GetTenantName();
    }

    #endregion Public Methods
}