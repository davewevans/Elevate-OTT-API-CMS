namespace OttApiPlatform.Infrastructure.Extensions;

public static class LocalizationMiddlewareExtension
{
    #region Public Methods

    /// <summary>
    /// Configures the application to use localization middleware.
    /// </summary>
    /// <param name="app">The application builder.</param>
    /// <returns>The application builder.</returns>
    public static IApplicationBuilder UseAppLocalization(this IApplicationBuilder app)
    {
        // Create the supported culture.
        var supportedCultures = new[]
        {
            "en-US",
            "ar-SA",
            "de-DE",
            "ro-RO",
            "es-ES",
            "fr-FR",
            "hi-IN",
            "ja-JP",
            "pt-PT",
            "tr-TR",
            "zh-CN"
        };

        // Create localization options and set the default culture.
        var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
                                                                  .AddSupportedCultures(supportedCultures)
                                                                  .AddSupportedUICultures(supportedCultures);

        // Add a custom request culture provider to select the culture based on the Accept-Language header.
        localizationOptions.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(context =>
        {
            var userLanguages = context.Request.Headers["Accept-Language"].ToString();
            var firstLanguage = userLanguages.Split(',').FirstOrDefault();
            var defaultLanguage = string.IsNullOrEmpty(firstLanguage) ? supportedCultures[0] : firstLanguage;
            return Task.FromResult(new ProviderCultureResult(defaultLanguage, defaultLanguage));
        }));

        // Use the localization middleware.
        app.UseRequestLocalization(localizationOptions);

        return app;
    }

    #endregion Public Methods
}