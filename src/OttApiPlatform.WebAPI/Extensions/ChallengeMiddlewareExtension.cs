namespace OttApiPlatform.WebAPI.Extensions;

public static class ChallengeMiddlewareExtension
{
    /// <summary>
    /// This middleware intercepts unauthorized requests caused by the built-in [Authorize]
    /// attribute and sends HTTP 401 Unauthorized status codes.
    /// </summary>
    /// <param name="builder">The <see cref="IApplicationBuilder"/> instance.</param>
    /// <returns>
    /// The <see cref="IApplicationBuilder"/> instance with the ChallengeMiddleware added to the
    /// middleware pipeline.
    /// </returns>

    #region Public Methods

    public static IApplicationBuilder UseChallenge(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ChallengeMiddleware>();
    }

    #endregion Public Methods
}