namespace OttApiPlatform.Application.Features.AppSettings.Queries.GetSettings.GetIdentitySettings;

public class GetIdentitySettingsQuery : IRequest<Envelope<IdentitySettingsForEditResponse>>
{
    #region Public Classes

    public class GetIdentitySettingsQueryHandler : IRequestHandler<GetIdentitySettingsQuery, Envelope<IdentitySettingsForEditResponse>>
    {
        #region Private Fields

        private readonly IAppSettingsReaderService _appSettingsReaderService;

        #endregion Private Fields

        #region Public Constructors

        public GetIdentitySettingsQueryHandler(IAppSettingsReaderService appSettingsReaderService)
        {
            _appSettingsReaderService = appSettingsReaderService;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<IdentitySettingsForEditResponse>> Handle(GetIdentitySettingsQuery request, CancellationToken cancellationToken)
        {
            return await _appSettingsReaderService.GetIdentitySettings();
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}