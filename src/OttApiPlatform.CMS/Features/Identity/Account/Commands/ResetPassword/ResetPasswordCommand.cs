namespace OttApiPlatform.CMS.Features.Identity.Account.Commands.ResetPassword;

public class ResetPasswordCommand
{
    #region Public Properties

    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public string Code { get; set; }

    #endregion Public Properties
}