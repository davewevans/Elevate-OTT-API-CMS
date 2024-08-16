namespace OttApiPlatform.CMS.Features.Identity.Account.Commands.ForgotPassword;

public class ForgetPasswordResponse
{
    #region Public Properties

    public string Code { get; set; }
    public bool DisplayConfirmPasswordLink { get; set; }
    public string SuccessMessage { get; set; }

    #endregion Public Properties
}