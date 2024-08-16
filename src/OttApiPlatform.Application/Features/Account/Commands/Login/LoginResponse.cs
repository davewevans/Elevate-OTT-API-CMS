namespace OttApiPlatform.Application.Features.Account.Commands.Login;

public class LoginResponse
{
    #region Public Properties

    public bool RequiresTwoFactor { get; set; }
    public AuthResponse AuthResponse { get; set; }

    #endregion Public Properties
}