namespace OttApiPlatform.CMS.Features.Identity.Manage.Commands.ChangeEmail;

public class ChangeEmailCommand
{
    #region Public Properties

    public string NewEmail { get; set; }
    public bool DisplayConfirmAccountLink { get; set; } = true;

    #endregion Public Properties
}