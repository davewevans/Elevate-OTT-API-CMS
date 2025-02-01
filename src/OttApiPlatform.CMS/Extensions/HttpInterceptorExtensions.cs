namespace OttApiPlatform.CMS.Extensions;

public static class HttpInterceptorExtensions
{
    #region Public Methods

    public static void AddHttpInterceptor(this IServiceCollection services, WebAssemblyHostBuilder builder)
    {
        services.AddScoped<HttpInterceptor>();
        services.AddHttpClient("DefaultClient", client =>
        {
            client.BaseAddress = new Uri(builder.Configuration.GetSection("BaseApiUrl").Value ?? throw new InvalidOperationException("Invalid BaseApiUrl."));
        }).AddHttpMessageHandler<HttpInterceptor>();

        // Client without a Base URL
        services.AddHttpClient("NoBaseUrlClient");
    }

    #endregion Public Methods
}