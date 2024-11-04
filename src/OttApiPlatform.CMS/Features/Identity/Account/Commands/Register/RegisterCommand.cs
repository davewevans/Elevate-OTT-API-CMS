namespace OttApiPlatform.CMS.Features.Identity.Account.Commands.Register;

public class RegisterCommand
{
    #region Public Properties

    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public string ChannelName { get; set; }
    public string SubDomain { get; set; }
    public bool AcceptTerms { get; set; }

    #endregion Public Properties
}