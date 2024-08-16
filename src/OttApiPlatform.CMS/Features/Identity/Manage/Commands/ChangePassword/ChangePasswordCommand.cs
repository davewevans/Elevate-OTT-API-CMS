namespace OttApiPlatform.CMS.Features.Identity.Manage.Commands.ChangePassword;

public class ChangePasswordCommand
{
    #region Public Properties

    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
    public string ConfirmPassword { get; set; }

    #endregion Public Properties
}