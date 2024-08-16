namespace OttApiPlatform.CMS.Features.AppSettings.Commands.UpdateSettings.UpdateIdentitySettings.IdentitySettingsCommand;

public class SignInSettingsCommand
{
    #region Public Properties

    public string Id { get; set; }

    //public bool RequireConfirmedEmail { get; set; }
    //public bool RequireConfirmedPhoneNumber { get; set; }
    public bool RequireConfirmedAccount { get; set; }

    #endregion Public Properties
}