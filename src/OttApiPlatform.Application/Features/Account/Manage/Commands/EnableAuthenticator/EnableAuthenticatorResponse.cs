namespace OttApiPlatform.Application.Features.Account.Manage.Commands.EnableAuthenticator;

public class EnableAuthenticatorResponse
{
    #region Public Properties

    public string[] RecoveryCodes { get; set; }
    public string SuccessMessage { get; set; }
    public bool ShowRecoveryCodes { get; set; }

    #endregion Public Properties
}