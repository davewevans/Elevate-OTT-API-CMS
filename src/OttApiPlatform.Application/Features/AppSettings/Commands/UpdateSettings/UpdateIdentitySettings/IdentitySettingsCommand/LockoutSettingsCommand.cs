namespace OttApiPlatform.Application.Features.AppSettings.Commands.UpdateSettings.UpdateIdentitySettings.IdentitySettingsCommand;

public class LockoutSettingsCommand
{
    #region Public Properties

    public string Id { get; set; }
    public bool AllowedForNewUsers { get; set; }
    public int? MaxFailedAccessAttempts { get; set; }
    public int? DefaultLockoutTimeSpan { get; set; }

    #endregion Public Properties
}