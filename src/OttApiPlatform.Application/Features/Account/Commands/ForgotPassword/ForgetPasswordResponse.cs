namespace OttApiPlatform.Application.Features.Account.Commands.ForgotPassword;

public class ForgetPasswordResponse
{
    #region Public Properties

    public string Code { get; set; }
    public string SuccessMessage { get; set; }
    public bool DisplayConfirmPasswordLink { get; set; }

    #endregion Public Properties
}