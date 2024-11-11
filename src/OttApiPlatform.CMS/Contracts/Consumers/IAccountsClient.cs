namespace OttApiPlatform.CMS.Contracts.Consumers;

/// <summary>
/// Provides methods for managing user accounts.
/// </summary>
public interface IAccountsClient
{
    #region Public Methods

    /// <summary>
    /// Registers a new user account.
    /// </summary>
    /// <param name="request">The registration details.</param>
    /// <returns>A <see cref="RegisterResponse"/>.</returns>
    Task<ApiResponseWrapper<RegisterResponse>> Register(RegisterCommand request);

    /// <summary>
    /// Logs in a user with the provided credentials.
    /// </summary>
    /// <param name="request">The login details.</param>
    /// <returns>A <see cref="LoginResponse"/>.</returns>
    Task<ApiResponseWrapper<LoginResponse>> Login(LoginCommand request);

    /// <summary>
    /// Logs in a user using two-factor authentication.
    /// </summary>
    /// <param name="request">The login details.</param>
    /// <returns>A <see cref="LoginWith2FaResponse"/>.</returns>
    Task<ApiResponseWrapper<LoginWith2FaResponse>> LoginWith2Fa(LoginWith2FaCommand request);

    /// <summary>
    /// Logs in a user using a recovery code.
    /// </summary>
    /// <param name="request">The login details.</param>
    /// <returns>A <see cref="LoginWithRecoveryCodeResponse"/>.</returns>
    Task<ApiResponseWrapper<LoginWithRecoveryCodeResponse>> LoginWithRecoveryCode(LoginWithRecoveryCodeCommand request);

    /// <summary>
    /// Sends an email to reset the user's password.
    /// </summary>
    /// <param name="request">The password reset details.</param>
    /// <returns>A <see cref="ForgetPasswordResponse"/>.</returns>
    Task<ApiResponseWrapper<ForgetPasswordResponse>> ForgetPassword(ForgetPasswordCommand request);

    /// <summary>
    /// Resets the user's password using a token and the new password.
    /// </summary>
    /// <param name="request">The password reset details.</param>
    /// <returns>A success message.</returns>
    Task<ApiResponseWrapper<string>> ResetPassword(ResetPasswordCommand request);

    /// <summary>
    /// Confirms the user's email using a token.
    /// </summary>
    /// <param name="request">The email confirmation details.</param>
    /// <returns>A success message.</returns>
    Task<ApiResponseWrapper<ConfirmEmailResponse>> ConfirmEmail(ConfirmEmailCommand request);

    /// <summary>
    /// Resends the email confirmation email to the user.
    /// </summary>
    /// <param name="request">The resend email confirmation details.</param>
    /// <returns>A <see cref="ResendEmailConfirmationResponse"/>.</returns>
    Task<ApiResponseWrapper<ResendEmailConfirmationResponse>> ResendEmailConfirmation(ResendEmailConfirmationCommand request);

    /// <summary>
    /// Refreshes the user's JWT token using the refresh token.
    /// </summary>
    /// <param name="request">The refresh token details.</param>
    /// <returns>A <see cref="AuthResponse"/>.</returns>
    Task<ApiResponseWrapper<AuthResponse>> RefreshToken(RefreshTokenCommand request);

    #endregion Public Methods
}