namespace OttApiPlatform.CMS.Features.Identity.Manage.Commands.ChangeEmail;

public class ChangeEmailResponse
{
    #region Public Properties

    public bool RequireConfirmedAccount { get; set; } = false;
    public string SuccessMessage { get; set; }
    public string EmailConfirmationUrl { get; set; }
    public bool DisplayConfirmAccountLink { get; set; } = false;
    public bool EmailUnchanged { get; set; } = false;
    public AuthResponse AuthResponse { get; set; }

    #endregion Public Properties
}