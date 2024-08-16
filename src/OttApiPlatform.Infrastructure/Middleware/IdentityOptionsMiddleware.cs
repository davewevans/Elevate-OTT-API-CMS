namespace OttApiPlatform.Infrastructure.Middleware;

/// <summary>
/// Middleware to apply identity options based on the current request path.
/// </summary>
public class IdentityOptionsMiddleware
{
    #region Private Fields

    // The following constants represent the paths that should be affected by the new identity options.
    private const string Login = "account/Login";

    private const string Register = "account/Register";
    private const string CreateUser = "identity/users/CreateUser";
    private const string UpdateUser = "identity/users/UpdateUser";
    private const string ChangeEmail = "manage/ChangeEmail";
    private const string ResetPassword = "account/ResetPassword";
    private const string ChangePassword = "manage/ChangePassword";
    private const string UpdateUserProfile = "manage/UpdateUserProfile";

    private readonly RequestDelegate _next;

    #endregion Private Fields

    #region Public Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="IdentityOptionsMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware in the pipeline.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="next"/> is null.</exception>
    public IdentityOptionsMiddleware(RequestDelegate next)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
    }

    #endregion Public Constructors

    #region Public Methods

    /// <summary>
    /// Applies the new identity options on the specified paths.
    /// </summary>
    /// <param name="httpContext">The current HTTP context.</param>
    /// <param name="dbContext">The database context.</param>
    /// <param name="configReaderService">The configuration reader.</param>
    /// <param name="tenantResolver">The tenant resolver.</param>
    /// <param name="identityOptions">The identity options.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="httpContext"/> is null or when <paramref name="path"/> is null.
    /// </exception>
    public async Task InvokeAsync(HttpContext httpContext,
                                  IApplicationDbContext dbContext,
                                  IConfigReaderService configReaderService,
                                  ITenantResolver tenantResolver,
                                  IOptions<IdentityOptions> identityOptions)
    {
        // Validate input parameters.
        if (httpContext == null)
            throw new ArgumentNullException(nameof(httpContext));

        var path = httpContext.Request.Path.Value;

        if (path == null)
            throw new ArgumentNullException(nameof(path));

        // Apply new identity options depending on the current path.
        if (path.Contains(Login))
            await ApplyNewIdentityOptionsOnLogin(dbContext, configReaderService, identityOptions);
        else
            await ApplyNewIdentityOptionsOnSpecifiedPaths(dbContext, configReaderService, tenantResolver, identityOptions, path);

        await _next(httpContext);
    }

    #endregion Public Methods

    #region Private Methods

    private static async Task ApplyNewIdentityOptionsOnSpecifiedPaths(IApplicationDbContext dbContext,
                                                                      IConfigReaderService configReaderService,
                                                                      ITenantResolver tenantResolver,
                                                                      IOptions<IdentityOptions> identityOptions,
                                                                      string path)
    {
        var pathAllowed = CheckPathAllowed(path);

        if (pathAllowed)
        {
            dynamic appSettings = new object();

            if (tenantResolver.TenantMode == TenantMode.MultiTenant)
                appSettings = await (from u in dbContext.UserSettings
                                     join p in dbContext.PasswordSettings
                                         on u.TenantId ?? Guid.Empty equals p.TenantId ?? Guid.Empty
                                     join l in dbContext.LockoutSettings
                                         on u.TenantId ?? Guid.Empty equals l.TenantId ?? Guid.Empty
                                     join s in dbContext.SignInSettings
                                         on u.TenantId ?? Guid.Empty equals s.TenantId ?? Guid.Empty
                                     select new { UserSettings = u, PasswordSettings = p, LockoutSettings = l, SignInSettings = s }).FirstOrDefaultAsync();

            if (tenantResolver.TenantMode == TenantMode.SingleTenant)
                appSettings = await (from u in dbContext.UserSettings
                                     join p in dbContext.PasswordSettings
                                         on true equals true
                                     join l in dbContext.LockoutSettings
                                         on true equals true
                                     join s in dbContext.SignInSettings
                                         on true equals true
                                     select new { UserSettings = u, PasswordSettings = p, LockoutSettings = l, SignInSettings = s }).FirstOrDefaultAsync();

            var userSettings = appSettings?.UserSettings ?? configReaderService.GetAppUserOptions().MapToEntity();
            var passwordSettings = appSettings?.PasswordSettings ?? configReaderService.GetAppPasswordOptions().MapToEntity();
            var lockoutSettings = appSettings?.LockoutSettings ?? configReaderService.GetAppLockoutOptions().MapToEntity();
            var signInSettings = appSettings?.SignInSettings ?? configReaderService.GetAppSignInOptions().MapToEntity();

            ApplyNewIdentityOptions(lockoutSettings, userSettings, passwordSettings, signInSettings, identityOptions.Value);
        }
    }

    /// <summary>
    /// Applies new identity options on login.
    /// </summary>
    /// <param name="dbContext">The database context</param>
    /// <param name="configReaderService">The configuration reader</param>
    /// <param name="identityOptions">The identity options</param>
    private static async Task ApplyNewIdentityOptionsOnLogin(IApplicationDbContext dbContext,
                                                             IConfigReaderService configReaderService,
                                                             IOptions<IdentityOptions> identityOptions)
    {
        // Retrieve lockout settings from the database or configuration file.
        var lockoutSettings = await dbContext.LockoutSettings.FirstOrDefaultAsync() ?? configReaderService.GetAppLockoutOptions().MapToEntity();

        // Apply new identity options using retrieved lockout settings.
        ApplyNewIdentityOptions(lockoutSettings, identityOptions.Value);
    }

    /// <summary>
    /// Checks if the given path is allowed to have identity options applied to it.
    /// </summary>
    /// <param name="path">The path to check.</param>
    /// <returns>Returns true if the path is allowed; false otherwise.</returns>
    /// <exception cref="ArgumentNullException">Thrown when path is null.</exception>
    private static bool CheckPathAllowed(string path)
    {
        // Throw an exception if the path is null.
        if (path == null)
            throw new ArgumentNullException(nameof(path));

        // Check if the path is one of the allowed paths.
        var pathAllowed = path.Contains(Register) ||
                          path.Contains(CreateUser) ||
                          path.Contains(UpdateUser) ||
                          path.Contains(ChangeEmail) ||
                          path.Contains(ResetPassword) ||
                          path.Contains(ChangePassword) ||
                          path.Contains(UpdateUserProfile);

        // Return the result.
        return pathAllowed;
    }

    /// <summary>
    /// Applies new lockout settings from database to IdentityOptions object.
    /// </summary>
    /// <param name="lockoutSettings">The lockout settings to apply</param>
    /// <param name="identityOptions">The IdentityOptions object to update</param>
    private static void ApplyNewIdentityOptions(LockoutSettings lockoutSettings, IdentityOptions identityOptions)
    {
        // update lockout settings in IdentityOptions object.
        identityOptions.Lockout.AllowedForNewUsers = lockoutSettings.AllowedForNewUsers;
        identityOptions.Lockout.MaxFailedAccessAttempts = lockoutSettings.MaxFailedAccessAttempts;
        identityOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(lockoutSettings.DefaultLockoutTimeSpan);
    }

    /// <summary>
    /// Updates the provided IdentityOptions object with the provided settings.
    /// </summary>
    /// <param name="lockoutSettings">
    /// The lockout settings to use for updating the IdentityOptions object.
    /// </param>
    /// <param name="userSettings">The user settings to use for updating the IdentityOptions object.</param>
    /// <param name="passwordSettings">
    /// The password settings to use for updating the IdentityOptions object.
    /// </param>
    /// <param name="signInSettings">
    /// The sign-in settings to use for updating the IdentityOptions object.
    /// </param>
    /// <param name="identityOptions">The IdentityOptions object to update.</param>
    private static void ApplyNewIdentityOptions(LockoutSettings lockoutSettings, UserSettings userSettings, PasswordSettings passwordSettings, SignInSettings signInSettings, IdentityOptions identityOptions)
    {
        // Set the allowed user name characters and require unique email.
        identityOptions.User.AllowedUserNameCharacters = userSettings.AllowedUserNameCharacters;
        identityOptions.User.RequireUniqueEmail = true;

        // Set the password requirements.
        identityOptions.Password.RequiredLength = passwordSettings.RequiredLength;
        identityOptions.Password.RequiredUniqueChars = passwordSettings.RequiredUniqueChars;
        identityOptions.Password.RequireNonAlphanumeric = passwordSettings.RequireNonAlphanumeric;
        identityOptions.Password.RequireLowercase = passwordSettings.RequireLowercase;
        identityOptions.Password.RequireUppercase = passwordSettings.RequireUppercase;
        identityOptions.Password.RequireDigit = passwordSettings.RequireDigit;

        // Set the lockout settings.
        identityOptions.Lockout.AllowedForNewUsers = lockoutSettings.AllowedForNewUsers;
        identityOptions.Lockout.MaxFailedAccessAttempts = lockoutSettings.MaxFailedAccessAttempts;
        identityOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(lockoutSettings.DefaultLockoutTimeSpan);

        // Set the sign-in settings.
        identityOptions.SignIn.RequireConfirmedAccount = signInSettings.RequireConfirmedAccount;
    }

    #endregion Private Methods
}