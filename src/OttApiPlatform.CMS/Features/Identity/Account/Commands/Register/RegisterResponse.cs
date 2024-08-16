namespace OttApiPlatform.CMS.Features.Identity.Account.Commands.Register;

public class RegisterResponse
{
    #region Public Properties

    public string Email { get; set; }
    public string EmailConfirmationUrl { get; set; }
    public bool DisplayConfirmAccountLink { get; set; }
    public bool RequireConfirmedAccount { get; set; }
    public string SuccessMessage { get; set; }
    public AuthResponse AuthResponse { get; set; }

    #endregion Public Properties
}