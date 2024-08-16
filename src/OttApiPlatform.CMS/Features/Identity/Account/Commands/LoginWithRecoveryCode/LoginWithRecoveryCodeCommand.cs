namespace OttApiPlatform.CMS.Features.Identity.Account.Commands.LoginWithRecoveryCode;

public class LoginWithRecoveryCodeCommand
{
    #region Public Properties

    public string RecoveryCode { get; set; }
    public string UserName { get; set; }

    #endregion Public Properties
}