namespace OttApiPlatform.Application.Features.AppSettings.Queries.GetSettings.GetIdentitySettings;

public class IdentitySettingsForEditResponse
{
    #region Public Constructors

    public IdentitySettingsForEditResponse()
    {
        UserSettingsForEdit = new UserSettingsForEdit();
        PasswordSettingsForEdit = new PasswordSettingsForEdit();
        LockoutSettingsForEdit = new LockoutSettingsForEdit();
        SignInSettingsForEdit = new SignInSettingsForEdit();
    }

    #endregion Public Constructors

    #region Public Properties

    public UserSettingsForEdit UserSettingsForEdit { get; set; }
    public PasswordSettingsForEdit PasswordSettingsForEdit { get; set; }
    public LockoutSettingsForEdit LockoutSettingsForEdit { get; set; }
    public SignInSettingsForEdit SignInSettingsForEdit { get; set; }

    #endregion Public Properties
}