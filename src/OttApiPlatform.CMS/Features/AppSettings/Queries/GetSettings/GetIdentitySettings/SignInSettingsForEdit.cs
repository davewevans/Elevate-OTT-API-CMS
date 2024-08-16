namespace OttApiPlatform.CMS.Features.AppSettings.Queries.GetSettings.GetIdentitySettings;

public class SignInSettingsForEdit
{
    #region Public Properties

    public string Id { get; set; }

    //public bool RequireConfirmedEmail { get; set; }
    //public bool RequireConfirmedPhoneNumber { get; set; }
    public bool RequireConfirmedAccount { get; set; }

    #endregion Public Properties

    #region Public Methods

    public SignInSettingsCommand MapToCommand()
    {
        return new SignInSettingsCommand
        {
            Id = Id,
            RequireConfirmedAccount = RequireConfirmedAccount
        };
    }

    #endregion Public Methods
}