namespace OttApiPlatform.Application.Common.Contracts.Infrastructure.Identity;

/// <summary>
/// Generates application access and refresh tokens.
/// </summary>
public interface ITokenGeneratorService
{
    #region Public Methods

    /// <summary>
    /// Generates an access token for the given user.
    /// </summary>
    /// <param name="user">The user to generate the access token for.</param>
    /// <returns>A string containing the generated access token.</returns>
    Task<string> GenerateAccessTokenAsync(ApplicationUser user);

    /// <summary>
    /// Generates a refresh token.
    /// </summary>
    /// <returns>A string containing the generated refresh token.</returns>
    string GenerateRefreshToken();

    /// <summary>
    /// Gets the claims principal from an expired token.
    /// </summary>
    /// <param name="token">The expired token.</param>
    /// <returns>A ClaimsPrincipal containing the claims from the expired token.</returns>
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);

    #endregion Public Methods
}