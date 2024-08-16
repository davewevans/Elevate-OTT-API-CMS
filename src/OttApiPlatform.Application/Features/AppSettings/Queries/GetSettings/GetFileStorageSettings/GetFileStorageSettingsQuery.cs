namespace OttApiPlatform.Application.Features.AppSettings.Queries.GetSettings.GetFileStorageSettings;

public class GetFileStorageSettingsQuery : IRequest<Envelope<FileStorageSettingsForEditResponse>>
{
    #region Public Classes

    public class GetFileStorageSettingsQueryHandler : IRequestHandler<GetFileStorageSettingsQuery, Envelope<FileStorageSettingsForEditResponse>>
    {
        #region Private Fields

        private readonly IAppSettingsReaderService _appSettingsReaderService;

        #endregion Private Fields

        #region Public Constructors

        public GetFileStorageSettingsQueryHandler(IAppSettingsReaderService appSettingsReaderService)
        {
            _appSettingsReaderService = appSettingsReaderService;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<FileStorageSettingsForEditResponse>> Handle(GetFileStorageSettingsQuery request, CancellationToken cancellationToken)
        {
            return await _appSettingsReaderService.GetFileStorageSettings();
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}