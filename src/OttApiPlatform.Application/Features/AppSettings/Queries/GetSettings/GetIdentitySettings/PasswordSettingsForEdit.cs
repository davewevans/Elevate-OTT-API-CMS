namespace OttApiPlatform.Application.Features.AppSettings.Queries.GetSettings.GetIdentitySettings;

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

    public static PasswordSettingsForEdit MapFromEntity(PasswordSettings passwordSettings)
    {
        return new PasswordSettingsForEdit
        {
            Id = passwordSettings.Id.ToString(),
            RequiredLength = passwordSettings.RequiredLength,
            RequiredUniqueChars = passwordSettings.RequiredUniqueChars,
            RequireNonAlphanumeric = passwordSettings.RequireNonAlphanumeric,
            RequireLowercase = passwordSettings.RequireLowercase,
            RequireUppercase = passwordSettings.RequireUppercase,
            RequireDigit = passwordSettings.RequireDigit,
        };
    }

    #endregion Public Methods
}