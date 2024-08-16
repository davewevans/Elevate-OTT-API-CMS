namespace OttApiPlatform.CMS.Features.AppSettings.Queries.GetSettings.GetIdentitySettings;

public class LockoutSettingsForEdit
{
    #region Public Properties

    public string Id { get; set; }
    public bool AllowedForNewUsers { get; set; }
    public int MaxFailedAccessAttempts { get; set; }
    public int DefaultLockoutTimeSpan { get; set; }

    #endregion Public Properties

    #region Public Methods

    public LockoutSettingsCommand MapToCommand()
    {
        return new LockoutSettingsCommand
        {
            Id = Id,
            AllowedForNewUsers = AllowedForNewUsers,
            MaxFailedAccessAttempts = MaxFailedAccessAttempts,
            DefaultLockoutTimeSpan = DefaultLockoutTimeSpan
        };
    }

    #endregion Public Methods
}