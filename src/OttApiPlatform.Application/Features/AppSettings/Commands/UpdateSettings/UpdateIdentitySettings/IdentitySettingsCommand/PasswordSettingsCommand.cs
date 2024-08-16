namespace OttApiPlatform.Application.Features.AppSettings.Commands.UpdateSettings.UpdateIdentitySettings.IdentitySettingsCommand;

public class PasswordSettingsCommand
{
    #region Public Properties

    public string Id { get; set; }
    public int? RequiredLength { get; set; }
    public int? RequiredUniqueChars { get; set; }
    public bool RequireNonAlphanumeric { get; set; }
    public bool RequireLowercase { get; set; }
    public bool RequireUppercase { get; set; }
    public bool RequireDigit { get; set; }

    #endregion Public Properties
}