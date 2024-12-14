namespace OttApiPlatform.Application.Features.ContentManagement.Assets.Queries.GetSasToken;

public class GetSasTokenQuery : IRequest<Envelope<SasTokenResponse>>
{
    #region Public Properties

    public string FileName { get; set; }

    #endregion


    #region Public Classes

    public class GetSasTokenQueryHandler : IRequestHandler<GetSasTokenQuery, Envelope<SasTokenResponse>>
    {

        #region Private Fields

        private readonly IAzureBlobStorageService _azureBlobStorageService;

        #endregion Private Fields

        #region Public Constructors

        public GetSasTokenQueryHandler(IAzureBlobStorageService azureBlobStorageService)
        {
           _azureBlobStorageService = azureBlobStorageService;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<SasTokenResponse>> Handle(GetSasTokenQuery request, CancellationToken cancellationToken)
        {
            var sasTokenResponse = _azureBlobStorageService.GetSasTokenForVideoContainer(request.FileName);

            return Envelope<SasTokenResponse>.Result.Ok(sasTokenResponse);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}
