namespace OttApiPlatform.CMS.Features.Identity.Account.Commands.ConfirmEmail;

public class ConfirmEmailCommand
{
    #region Public Properties

    public string UserId { get; set; }
    public string Code { get; set; }

    #endregion Public Properties
}