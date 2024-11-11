using OttApiPlatform.Application.Common.Contracts.UseCases.Content;

namespace OttApiPlatform.Application.Features.ContentManagement.Videos.Queries.GetSasToken;

public class GetSasTokenQuery : IRequest<Envelope<SasTokenResponse>>
{
    #region Public Classes

    public class GetSasTokenQueryHandler : IRequestHandler<GetSasTokenQuery, Envelope<SasTokenResponse>>
    {
        #region Private Fields

        private readonly IVideoUseCase _videoUseCase;

        #endregion Private Fields

        #region Public Constructors

        public GetSasTokenQueryHandler(IVideoUseCase videoUseCase)
        {
            _videoUseCase = videoUseCase;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<SasTokenResponse>> Handle(GetSasTokenQuery request, CancellationToken cancellationToken)
        {
            var response = _videoUseCase.GetSasTokenFromAzure();
            return await Task.FromResult(response);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}
