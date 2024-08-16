namespace OttApiPlatform.Application.Features.AppSettings.Queries.GetSettings.GetIdentitySettings;

public class SignInSettingsForEdit
{
    #region Public Properties

    public string Id { get; set; }

    //public bool RequireConfirmedEmail { get; set; }
    //public bool RequireConfirmedPhoneNumber { get; set; }
    public bool RequireConfirmedAccount { get; set; }

    #endregion Public Properties

    #region Public Methods

    public static SignInSettingsForEdit MapFromEntity(SignInSettings userSettings)
    {
        return new SignInSettingsForEdit
        {
            Id = userSettings.Id.ToString(),
            RequireConfirmedAccount = userSettings.RequireConfirmedAccount,
        };
    }

    #endregion Public Methods
}