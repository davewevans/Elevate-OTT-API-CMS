namespace OttApiPlatform.CMS.Features.AppSettings.Commands.UpdateSettings.UpdateTokenSettings;

public class UpdateTokenSettingsCommand
{
    #region Public Properties

    public string Id { get; set; }
    public int AccessTokenUoT { get; set; }
    public double? AccessTokenTimeSpan { get; set; }
    public int RefreshTokenUoT { get; set; }
    public double? RefreshTokenTimeSpan { get; set; }

    #endregion Public Properties
}