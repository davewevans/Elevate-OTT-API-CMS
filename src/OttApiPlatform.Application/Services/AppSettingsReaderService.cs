namespace OttApiPlatform.Application.Services;

public class AppSettingsReaderService : IAppSettingsReaderService
{
    #region Private Fields

    private readonly IApplicationDbContext _dbContext;
    private readonly IConfigReaderService _configReaderService;

    #endregion Private Fields

    #region Public Constructors

    public AppSettingsReaderService(IApplicationDbContext dbContext, IConfigReaderService configReaderService)
    {
        _dbContext = dbContext;
        _configReaderService = configReaderService;
    }

    #endregion Public Constructors

    #region Public Methods

    public async Task<Envelope<IdentitySettingsForEditResponse>> GetIdentitySettings()
    {
        // Retrieve UserSettings entity from the database, or if not found, create a new one from
        // the configuration options.
        var userSettings = await _dbContext.UserSettings.FirstOrDefaultAsync()
                           ?? _configReaderService.GetAppUserOptions().MapToEntity();

        // Retrieve PasswordSettings entity from the database, or if not found, create a new one
        // from the configuration options.
        var passwordSettings = await _dbContext.PasswordSettings.FirstOrDefaultAsync()
                               ?? _configReaderService.GetAppPasswordOptions().MapToEntity();

        // Retrieve LockoutSettings entity from the database, or if not found, create a new one from
        // the configuration options.
        var lockoutSettings = await _dbContext.LockoutSettings.FirstOrDefaultAsync()
                              ?? _configReaderService.GetAppLockoutOptions().MapToEntity();

        // Retrieve SignInSettings entity from the database, or if not found, create a new one from
        // the configuration options.
        var signInSettings = await _dbContext.SignInSettings.FirstOrDefaultAsync()
                             ?? _configReaderService.GetAppSignInOptions().MapToEntity();

        // Map the retrieved entities to DTOs.
        var identitySettingsForEditResponse = new IdentitySettingsForEditResponse
        {
            UserSettingsForEdit = UserSettingsForEdit.MapFromEntity(userSettings),
            PasswordSettingsForEdit = PasswordSettingsForEdit.MapFromEntity(passwordSettings),
            LockoutSettingsForEdit = LockoutSettingsForEdit.MapFromEntity(lockoutSettings),
            SignInSettingsForEdit = SignInSettingsForEdit.MapFromEntity(signInSettings)
        };

        // Return the DTO wrapped in an Envelope object.
        return Envelope<IdentitySettingsForEditResponse>.Result.Ok(identitySettingsForEditResponse);
    }

    public async Task<Envelope<TokenSettingsForEditResponse>> GetTokenSettings()
    {
        // Retrieve the current token settings from the database, or fall back to the default
        // settings if not found.
        var tokenSettings = await _dbContext.TokenSettings.FirstOrDefaultAsync()
                            ?? _configReaderService.GetAppTokenOptions().MapToEntity();

        // Map the token settings entity onto a response DTO.
        var tokenSettingsForEditResponse = TokenSettingsForEditResponse.MapFromEntity(tokenSettings);

        // Return the response envelope containing the token settings DTO.
        return Envelope<TokenSettingsForEditResponse>.Result.Ok(tokenSettingsForEditResponse);
    }

    public async Task<Envelope<FileStorageSettingsForEditResponse>> GetFileStorageSettings()
    {
        // Get the file storage settings entity from the database, or create a new one from the app settings.
        var fileStorageSettings = await _dbContext.FileStorageSettings.FirstOrDefaultAsync()
                                  ?? _configReaderService.GetAppFileStorageOptions().MapToEntity();

        // Map the entity to a response DTO.
        var storageSettingsForEditResponse = FileStorageSettingsForEditResponse.MapFromEntity(fileStorageSettings);

        // Return the response in an envelope with a success status.
        return Envelope<FileStorageSettingsForEditResponse>.Result.Ok(storageSettingsForEditResponse);
    }

    #endregion Public Methods
}