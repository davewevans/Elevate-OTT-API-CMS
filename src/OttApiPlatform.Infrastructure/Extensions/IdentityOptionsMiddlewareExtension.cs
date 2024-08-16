namespace OttApiPlatform.Infrastructure.Extensions;

public static class IdentityOptionsMiddlewareExtension
{
    #region Public Methods

    /// <summary>
    /// Adds middleware that applies identity options to the application.
    /// </summary>
    /// <param name="builder">The <see cref="IApplicationBuilder"/> instance.</param>
    /// <returns>The <see cref="IApplicationBuilder"/> instance.</returns>
    public static IApplicationBuilder UseIdentityOptions(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<IdentityOptionsMiddleware>();
    }

    #endregion Public Methods
}