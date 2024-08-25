namespace OttApiPlatform.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
    #region Public Methods

    /// <summary>
    /// Adds application settings options to the specified <see cref="IServiceCollection"/> instance.
    /// </summary>
    /// <param name="services">The service collection to add the options to.</param>
    /// <param name="configuration">
    /// The <see cref="IConfiguration"/> instance to retrieve the settings from.
    /// </param>
    /// <returns>The modified <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddAppSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AppOptions>(configuration.GetSection(AppOptions.Section));
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.Section));
        services.Configure<SmtpOption>(configuration.GetSection(SmtpOption.Section));
        services.Configure<ClientAppOptions>(configuration.GetSection(ClientAppOptions.Section));
        return services;
    }

    /// <summary>
    /// Adds application localization services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddAppLocalization(this IServiceCollection services)
    {
        // Configure localization options.
        services.AddLocalization(options => options.ResourcesPath = "Resource");

        // Add the localization service implementation.
        services.AddSingleton<ILocalizationService, LocalizationService>();

        return services;
    }

    /// <summary>
    /// Configures authentication services for the application.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="configuration">
    /// The <see cref="IConfiguration"/> instance containing the JWT configuration.
    /// </param>
    /// <returns>The <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        // Get JWT settings from configuration.
        var jwtSettings = configuration.GetSection(JwtOptions.Section).Get<JwtOptions>();

        // Configure authentication options.
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                LifetimeValidator = LifetimeValidator, // Custom lifetime validator.
                ClockSkew = TimeSpan.Zero,
                ValidateIssuerSigningKey = true,
                RequireExpirationTime = true,
                ValidIssuer = jwtSettings?.Issuer,
                ValidAudience = jwtSettings?.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings?.SecurityKey ?? throw new InvalidOperationException($"{nameof(jwtSettings.SecurityKey)} cannot be Null."))),
            };
            options.Events = new JwtBearerEvents
            {
                // Event to handle message received.
                OnMessageReceived = context =>
                {
                    // Get the access token from the request header.
                    var accessToken = context.Request.Query["access_token"];

                    // Get the request path.
                    var path = context.HttpContext.Request.Path;

                    // Check if the request is coming from SignalR Hub...
                    if (!string.IsNullOrEmpty(accessToken) &&
                        (path.StartsWithSegments("/Hubs/DashboardHub") ||
                         path.StartsWithSegments("/Hubs/DataExportHub")))
                        // Read the token out of the query string.
                        context.Token = accessToken;

                    // Get a completed task.
                    return Task.CompletedTask;
                }
            };
        });

        // Add authorization services.
        services.AddAuthorization();

        return services;
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Custom lifetime validator for JWT tokens.
    /// </summary>
    /// <param name="notBefore">The 'not before' value of the token.</param>
    /// <param name="expires">The 'expires' value of the token.</param>
    /// <param name="tokenToValidate">The token to validate.</param>
    /// <param name="param">The <see cref="TokenValidationParameters"/> instance.</param>
    /// <returns>True if the token is valid, false otherwise.</returns>
    private static bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken tokenToValidate, TokenValidationParameters param)
    {
        if (expires != null)
            return expires > DateTime.UtcNow;

        return false;
    }


    #endregion Private Methods
}