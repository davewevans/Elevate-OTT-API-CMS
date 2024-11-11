using OttApiPlatform.Application.Common.Contracts.UseCases.Content;

namespace OttApiPlatform.Application.Features.ContentManagement.Videos.Queries.GetVideos;

public class GetVideosQuery : FilterableQuery, IRequest<Envelope<VideosResponse>>
{
    #region Public Classes
    public class GetVideosQueryHandler : IRequestHandler<GetVideosQuery, Envelope<VideosResponse>>
    {
        #region Private Fields

        private readonly IVideoUseCase _videoUseCase;

        #endregion Private Fields

        #region Public Constructors

        public GetVideosQueryHandler(IVideoUseCase videoUseCase)
        {
            _videoUseCase = videoUseCase;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<VideosResponse>> Handle(GetVideosQuery request, CancellationToken cancellationToken)
        {
            return await _videoUseCase.GetVideos(request);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}
