namespace OttApiPlatform.Application.Features.AppSettings.Queries.GetSettings.GetTokenSettings;

public class GetTokenSettingsQuery : IRequest<Envelope<TokenSettingsForEditResponse>>
{
    public class GetTokenSettingsQueryHandler : IRequestHandler<GetTokenSettingsQuery, Envelope<TokenSettingsForEditResponse>>
    {
        #region Private Fields

        private readonly IAppSettingsReaderService _appSettingsReaderService;

        #endregion Private Fields

        #region Public Constructors

        #region Public Constructors

        public GetTokenSettingsQueryHandler(IAppSettingsReaderService appSettingsReaderService)
        {
            _appSettingsReaderService = appSettingsReaderService;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<TokenSettingsForEditResponse>> Handle(GetTokenSettingsQuery request, CancellationToken cancellationToken)
        {
            return await _appSettingsReaderService.GetTokenSettings();
        }

        #endregion Public Methods
    }

    #endregion Public Constructors
}