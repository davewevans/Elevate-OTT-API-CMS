namespace OttApiPlatform.Application.Features.Account.Manage.Queries.Get2faState;

public class TwoFactorAuthenticationStateResponse
{
    #region Public Properties

    public bool HasAuthenticator { get; set; }
    public int RecoveryCodesLeft { get; set; }
    public bool Is2FaEnabled { get; set; }
    public bool IsMachineRemembered { get; set; }

    #endregion Public Properties
}