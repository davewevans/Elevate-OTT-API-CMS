namespace OttApiPlatform.CMS.Features.AppSettings.Queries.GetSettings.GetIdentitySettings;

public class PasswordSettingsForEdit
{
    #region Public Properties

    public string Id { get; set; }
    public int RequiredLength { get; set; }
    public int RequiredUniqueChars { get; set; }
    public bool RequireNonAlphanumeric { get; set; }
    public bool RequireLowercase { get; set; }
    public bool RequireUppercase { get; set; }
    public bool RequireDigit { get; set; }

    #endregion Public Properties

    #region Public Methods

    public PasswordSettingsCommand MapToCommand()
    {
        return new PasswordSettingsCommand
        {
            Id = Id,
            RequiredLength = RequiredLength,
            RequiredUniqueChars = RequiredUniqueChars,
            RequireNonAlphanumeric = RequireNonAlphanumeric,
            RequireLowercase = RequireLowercase,
            RequireUppercase = RequireUppercase,
            RequireDigit = RequireDigit,
        };
    }

    #endregion Public Methods
}