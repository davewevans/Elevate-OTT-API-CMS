namespace OttApiPlatform.CMS.Features.Identity.Account.Commands.ResendEmailConfirmation;

public class ResendEmailConfirmationResponse
{
    #region Public Properties

    public bool RequireConfirmedAccount { get; set; }
    public bool DisplayConfirmAccountLink { get; set; }
    public string SuccessMessage { get; set; }
    public string EmailConfirmationUrl { get; set; }

    #endregion Public Properties
}