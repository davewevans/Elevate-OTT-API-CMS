namespace OttApiPlatform.CMS.Features.Identity.Manage.Commands.ConfirmEmailChange;

public class ConfirmEmailChangeCommand
{
    #region Public Properties

    public string UserId { get; set; }
    public string Email { get; set; }
    public string Code { get; set; }

    #endregion Public Properties
}