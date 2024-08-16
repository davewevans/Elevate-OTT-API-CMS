namespace OttApiPlatform.Application.Features.AppSettings.Commands.UpdateSettings.UpdateIdentitySettings;

public class UpdateIdentitySettingsCommand : IRequest<Envelope<IdentitySettingsResponse>>
{
    #region Public Properties

    public UserSettingsCommand UserSettingsCommand { get; set; }
    public PasswordSettingsCommand PasswordSettingsCommand { get; set; }
    public LockoutSettingsCommand LockoutSettingsCommand { get; set; }
    public SignInSettingsCommand SignInSettingsCommand { get; set; }

    #endregion Public Properties

    #region Public Methods

    public void MapToEntity(UserSettings userSettings, PasswordSettings passwordSettings, LockoutSettings lockoutSettings, SignInSettings signInSettings)
    {
        userSettings.Id = Guid.Parse(UserSettingsCommand.Id);
        userSettings.AllowedUserNameCharacters = UserSettingsCommand.AllowedUserNameCharacters;
        userSettings.NewUsersActiveByDefault = UserSettingsCommand.NewUsersActiveByDefault;

        passwordSettings.Id = Guid.Parse(PasswordSettingsCommand.Id);
        passwordSettings.RequiredLength = PasswordSettingsCommand.RequiredLength ?? throw new ArgumentNullException(nameof(PasswordSettingsCommand.RequiredLength));
        passwordSettings.RequiredUniqueChars = PasswordSettingsCommand.RequiredUniqueChars ?? throw new ArgumentNullException(nameof(PasswordSettingsCommand.RequiredUniqueChars));
        passwordSettings.RequireNonAlphanumeric = PasswordSettingsCommand.RequireNonAlphanumeric;
        passwordSettings.RequireLowercase = PasswordSettingsCommand.RequireLowercase;
        passwordSettings.RequireUppercase = PasswordSettingsCommand.RequireUppercase;
        passwordSettings.RequireDigit = PasswordSettingsCommand.RequireDigit;

        lockoutSettings.Id = Guid.Parse(LockoutSettingsCommand.Id);
        lockoutSettings.AllowedForNewUsers = LockoutSettingsCommand.AllowedForNewUsers;
        lockoutSettings.MaxFailedAccessAttempts = LockoutSettingsCommand.MaxFailedAccessAttempts ?? throw new ArgumentNullException(nameof(LockoutSettingsCommand.MaxFailedAccessAttempts));
        lockoutSettings.DefaultLockoutTimeSpan = LockoutSettingsCommand.DefaultLockoutTimeSpan ?? throw new ArgumentNullException(nameof(LockoutSettingsCommand.DefaultLockoutTimeSpan));

        signInSettings.Id = Guid.Parse(SignInSettingsCommand.Id);
        //signInSettings.RequireConfirmedEmail =SignInSettings.RequireConfirmedEmail;
        //signInSettings.RequireConfirmedPhoneNumber =SignInSettings.RequireConfirmedPhoneNumber;
        signInSettings.RequireConfirmedAccount = SignInSettingsCommand.RequireConfirmedAccount;
    }

    #endregion Public Methods

    #region Public Classes

    public class UpdateIdentitySettingsCommandHandler : IRequestHandler<UpdateIdentitySettingsCommand, Envelope<IdentitySettingsResponse>>
    {
        #region Private Fields

        private readonly IApplicationDbContext _dbContext;
        private readonly IConfigReaderService _configReaderService;

        #endregion Private Fields

        #region Public Constructors

        public UpdateIdentitySettingsCommandHandler(IApplicationDbContext dbContext, IConfigReaderService configReaderService)
        {
            _dbContext = dbContext;
            _configReaderService = configReaderService;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<IdentitySettingsResponse>> Handle(UpdateIdentitySettingsCommand request, CancellationToken cancellationToken)
        {
            // Check whether the user settings ID is valid, and parse it into userSettingsId.
            if (!Guid.TryParse(request.UserSettingsCommand.Id, out var userSettingsId))
                return Envelope<IdentitySettingsResponse>.Result.BadRequest(Resource.Invalid_user_settings_Id);

            // Check whether the password settings ID is valid, and parse it into passwordSettingsId.
            if (!Guid.TryParse(request.PasswordSettingsCommand.Id, out var passwordSettingsId))
                return Envelope<IdentitySettingsResponse>.Result.BadRequest(Resource.Invalid_password_settings_Id);

            // Check whether the lockout settings ID is valid, and parse it into lockoutSettingsId.
            if (!Guid.TryParse(request.LockoutSettingsCommand.Id, out var lockoutSettingsId))
                return Envelope<IdentitySettingsResponse>.Result.BadRequest(Resource.Invalid_lockout_settings_Id);

            // Check whether the sign in settings ID is valid, and parse it into signInSettingsId.
            if (!Guid.TryParse(request.SignInSettingsCommand.Id, out var signInSettingsId))
                return Envelope<IdentitySettingsResponse>.Result.BadRequest(Resource.Invalid_sign_in_settings_Id);

            // Retrieve the user settings that match userSettingsId from the database, or get the
            // default AppUserOptions.
            var userSettings = await _dbContext.UserSettings.FirstOrDefaultAsync(us => us.Id == userSettingsId, cancellationToken: cancellationToken)
                               ?? _configReaderService.GetAppUserOptions().MapToEntity();

            // Retrieve the password settings that match passwordSettingsId from the database, or
            // get the default AppPasswordOptions.
            var passwordSettings = await _dbContext.PasswordSettings.FirstOrDefaultAsync(ps => ps.Id == passwordSettingsId, cancellationToken: cancellationToken)
                                   ?? _configReaderService.GetAppPasswordOptions().MapToEntity();

            // Retrieve the lockout settings that match lockoutSettingsId from the database, or get
            // the default AppLockoutOptions.
            var lockoutSettings = await _dbContext.LockoutSettings.FirstOrDefaultAsync(ls => ls.Id == lockoutSettingsId, cancellationToken: cancellationToken)
                                  ?? _configReaderService.GetAppLockoutOptions().MapToEntity();

            // Retrieve the sign in settings that match signInSettingsId from the database, or get
            // the default AppSignInOptions.
            var signInSettings = await _dbContext.SignInSettings.FirstOrDefaultAsync(ss => ss.Id == signInSettingsId, cancellationToken: cancellationToken)
                                 ?? _configReaderService.GetAppSignInOptions().MapToEntity();

            // Map properties from the request to the retrieved or default settings entities
            request.MapToEntity(userSettings, passwordSettings, lockoutSettings, signInSettings);

            // Update the UserSettings table with userSettings.
            _dbContext.UserSettings.Update(userSettings);

            // Update the PasswordSettings table with passwordSettings.
            _dbContext.PasswordSettings.Update(passwordSettings);

            // Update the LockoutSettings table with lockoutSettings.
            _dbContext.LockoutSettings.Update(lockoutSettings);

            // Update the SignInSettings table with signInSettings.
            _dbContext.SignInSettings.Update(signInSettings);

            // Save changes to the database.
            await _dbContext.SaveChangesAsync(cancellationToken);

            // Create a new IdentitySettingsResponse object with the updated settings and success message.
            var identitySettingsResponse = new IdentitySettingsResponse
            {
                LockoutSettingsId = lockoutSettings.Id,
                PasswordSettingsId = passwordSettings.Id,
                SignInSettingsId = signInSettings.Id,
                UserSettingsId = userSettings.Id,
                SuccessMessage = Resource.Identity_settings_have_been_updated_successfully
            };

            // Return an envelope with the updated IdentitySettingsResponse object.
            return Envelope<IdentitySettingsResponse>.Result.Ok(identitySettingsResponse);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}