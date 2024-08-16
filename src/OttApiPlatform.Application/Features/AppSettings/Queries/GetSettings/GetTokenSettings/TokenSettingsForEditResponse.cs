namespace OttApiPlatform.Application.Features.AppSettings.Queries.GetSettings.GetTokenSettings;

public class TokenSettingsForEditResponse
{
    #region Public Properties

    public string Id { get; set; }
    public int AccessTokenUoT { get; set; }
    public double? AccessTokenTimeSpan { get; set; }
    public int RefreshTokenUoT { get; set; }
    public double? RefreshTokenTimeSpan { get; set; }

    #endregion Public Properties

    #region Public Methods

    public static TokenSettingsForEditResponse MapFromEntity(TokenSettings tokenSettings)
    {
        return new()
        {
            Id = tokenSettings.Id.ToString(),
            AccessTokenUoT = tokenSettings.AccessTokenUoT,
            AccessTokenTimeSpan = tokenSettings.AccessTokenTimeSpan,
            RefreshTokenUoT = tokenSettings.RefreshTokenUoT,
            RefreshTokenTimeSpan = tokenSettings.RefreshTokenTimeSpan
        };
    }

    #endregion Public Methods
}