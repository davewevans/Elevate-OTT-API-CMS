using Microsoft.Extensions.Options;

namespace OttApiPlatform.Infrastructure.Services;

public class ConfigReaderService : IConfigReaderService
{
    #region Private Fields

    private readonly AppOptions _appOptionsSnapshot;
    private readonly JwtOptions _jwtOptionsSnapshot;
    private readonly SmtpOption _smtpOptionSnapshot;
    private readonly ClientAppOptions _clientAppOptionsSnapshot;
    private readonly LicenseInfoOptions _licenseInfoOptionsSnapshot;
    private readonly BlobOptions _blobOptionsSnapshot;
    private readonly MuxOptions _muxOptionsSnapshot;
    private readonly IHttpContextAccessor _httpContextAccessor;

    #endregion Private Fields

    #region Public Constructors

    public ConfigReaderService(IOptionsSnapshot<AppOptions> appOptionsSnapshot,
                               IOptionsSnapshot<JwtOptions> jwtOptionsSnapshot,
                               IOptionsSnapshot<ClientAppOptions> clientAppOptionsSnapshot,
                               IOptionsSnapshot<SmtpOption> smtpOptionSnapshot,
                               IOptionsSnapshot<LicenseInfoOptions> licenseInfoOptionsSnapshot,
                               IHttpContextAccessor httpContextAccessor,
                               IOptionsSnapshot<BlobOptions> blobOptionsSnapshot,
                               IOptionsSnapshot<MuxOptions> muxOptionsSnapshot)
    {
        _appOptionsSnapshot = appOptionsSnapshot.Value;
        _jwtOptionsSnapshot = jwtOptionsSnapshot.Value;
        _clientAppOptionsSnapshot = clientAppOptionsSnapshot.Value;
        _smtpOptionSnapshot = smtpOptionSnapshot.Value;
        _licenseInfoOptionsSnapshot = licenseInfoOptionsSnapshot.Value;
        _httpContextAccessor = httpContextAccessor;
        _blobOptionsSnapshot = blobOptionsSnapshot.Value;
        _muxOptionsSnapshot = muxOptionsSnapshot.Value;

    }

    #endregion Public Constructors

    #region Public Methods

    public AppUserOptions GetAppUserOptions() => _appOptionsSnapshot.AppIdentityOptions.AppUserOptions;
    public AppPasswordOptions GetAppPasswordOptions() => _appOptionsSnapshot.AppIdentityOptions.AppPasswordOptions;
    public AppLockoutOptions GetAppLockoutOptions() => _appOptionsSnapshot.AppIdentityOptions.AppLockoutOptions;
    public AppSignInOptions GetAppSignInOptions() => _appOptionsSnapshot.AppIdentityOptions.AppSignInOptions;
    public AppTokenOptions GetAppTokenOptions() => _appOptionsSnapshot.AppTokenOptions;
    public AppFileStorageOptions GetAppFileStorageOptions() => _appOptionsSnapshot.AppFileStorageOptions;
    public JwtOptions GetJwtOptions() => _jwtOptionsSnapshot;
    public SmtpOption GetSmtpOption() => _smtpOptionSnapshot;
    public ClientAppOptions GetClientAppOptions() => _clientAppOptionsSnapshot;
    public AppTenantOptions GetAppTenantOptions() => _appOptionsSnapshot.AppTenantOptions;
    public string GetSubDomain() => _httpContextAccessor.GetTenantName();
    public LicenseInfoOptions GetLicenseInfoOptions() => _licenseInfoOptionsSnapshot;
    public BlobOptions GetBlobOptions() => _blobOptionsSnapshot;
    public MuxOptions GetMuxOptions() => _muxOptionsSnapshot;

    #endregion Public Methods
}