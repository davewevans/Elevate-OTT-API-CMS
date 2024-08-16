namespace OttApiPlatform.Application.Features.AppSettings.Commands.UpdateSettings.UpdateTokenSettings;

public class UpdateTokenSettingsCommand : IRequest<Envelope<TokenSettingsResponse>>
{
    #region Public Properties

    public string Id { get; set; }
    public int AccessTokenUoT { get; set; }
    public double? AccessTokenTimeSpan { get; set; }
    public int RefreshTokenUoT { get; set; }
    public double? RefreshTokenTimeSpan { get; set; }

    #endregion Public Properties

    #region Public Methods

    public void MapToEntity(TokenSettings tokenSettings)
    {
        tokenSettings.Id = Guid.Parse(Id);
        tokenSettings.AccessTokenUoT = AccessTokenUoT;
        tokenSettings.AccessTokenTimeSpan = AccessTokenTimeSpan;
        tokenSettings.RefreshTokenUoT = RefreshTokenUoT;
        tokenSettings.RefreshTokenTimeSpan = RefreshTokenTimeSpan;
    }

    #endregion Public Methods

    #region Public Classes

    public class UpdateTokenSettingsCommandHandler : IRequestHandler<UpdateTokenSettingsCommand, Envelope<TokenSettingsResponse>>
    {
        #region Private Fields

        private readonly IApplicationDbContext _dbContext;
        private readonly IConfigReaderService _configReaderService;

        #endregion Private Fields

        #region Public Constructors

        public UpdateTokenSettingsCommandHandler(IApplicationDbContext dbContext, IConfigReaderService configReaderService)
        {
            _dbContext = dbContext;
            _configReaderService = configReaderService;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<TokenSettingsResponse>> Handle(UpdateTokenSettingsCommand request, CancellationToken cancellationToken)
        {
            // Check if the provided token settings id is valid.
            if (!Guid.TryParse(request.Id, out var tokenSettingsId))
                return Envelope<TokenSettingsResponse>.Result.BadRequest(Resource.Invalid_token_settings_Id);

            // Retrieve the token settings with the given id from the database, or fall back to the
            // default settings if not found.
            var tokenSettings = await _dbContext.TokenSettings.FirstOrDefaultAsync(fs => fs.Id == tokenSettingsId, cancellationToken: cancellationToken)
                                ?? _configReaderService.GetAppTokenOptions().MapToEntity();

            // Map the properties of the command onto the token settings entity.
            request.MapToEntity(tokenSettings);

            // Update the token settings entity in the database.
            _dbContext.TokenSettings.Update(tokenSettings);

            // Save changes to the database.
            await _dbContext.SaveChangesAsync(cancellationToken);

            // Create a response with the updated token settings id and a success message.
            var tokenSettingsResponse = new TokenSettingsResponse
            {
                Id = tokenSettings.Id,
                SuccessMessage = Resource.Token_settings_have_been_updated_successfully
            };

            // Return the response envelope containing the token settings response.
            return Envelope<TokenSettingsResponse>.Result.Ok(tokenSettingsResponse);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}