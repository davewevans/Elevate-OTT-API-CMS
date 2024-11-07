namespace OttApiPlatform.Application.Common.Contracts.Application;

/// <summary>
/// Represents a service for authentication-related operations.
/// </summary>
public interface IAuthenticationService
{
    #region Public Methods

    /// <summary>
    /// Performs a login operation based on the specified login command.
    /// </summary>
    /// <param name="request">The login command containing user credentials.</param>
    /// <returns>An envelope containing the login response.</returns>
    Task<Envelope<LoginResponse>> Login(LoginCommand request);

    /// <summary>
    /// Performs a login operation after email has been confirmed after registration.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    Task<AuthResponse> LoginUserAfterConfirmEmail(ApplicationUser user);

    /// <summary>
    /// Generates access and refresh tokens for the specified user.
    /// </summary>
    /// <param name="user">The user for whom to generate tokens.</param>
    /// <returns>A tuple containing the access token and refresh token.</returns>
    Task<(string accessToken, string refreshToken)> GenerateAccessAndRefreshTokens(ApplicationUser user);

    #endregion Public Methods
}