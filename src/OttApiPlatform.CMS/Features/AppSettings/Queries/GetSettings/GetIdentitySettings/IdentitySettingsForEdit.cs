namespace OttApiPlatform.CMS.Features.AppSettings.Queries.GetSettings.GetIdentitySettings;

public class IdentitySettingsForEdit
{
    #region Public Constructors

    public IdentitySettingsForEdit()
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