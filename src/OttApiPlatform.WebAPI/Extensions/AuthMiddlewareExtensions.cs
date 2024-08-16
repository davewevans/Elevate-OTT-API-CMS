namespace OttApiPlatform.WebAPI.Extensions;

public static class AuthMiddlewareExtensions
{
    #region Public Methods

    /// <summary>
    /// Adds authentication and authorization middleware to the application pipeline.
    /// </summary>
    /// <param name="app">The <see cref="IApplicationBuilder"/> instance.</param>
    /// <returns>The <see cref="IApplicationBuilder"/> instance.</returns>
    public static IApplicationBuilder UseAuth(this IApplicationBuilder app)
    {
        app.UseAuthentication().UseChallenge();

        app.UseAuthorization();

        return app;
    }

    #endregion Public Methods
}