namespace OttApiPlatform.CMS.Features.AppSettings.Queries.GetSettings.GetTokenSettings;

public class TokenSettingsForEdit
{
    #region Public Properties

    public string Id { get; set; }
    public int AccessTokenUoT { get; set; }
    public double? AccessTokenTimeSpan { get; set; }
    public int RefreshTokenUoT { get; set; }
    public double? RefreshTokenTimeSpan { get; set; }

    #endregion Public Properties
}