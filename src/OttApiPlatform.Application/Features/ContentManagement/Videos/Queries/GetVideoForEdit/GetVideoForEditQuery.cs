using OttApiPlatform.Application.Common.Contracts.UseCases.Content;

namespace OttApiPlatform.Application.Features.ContentManagement.Videos.Queries.GetVideoForEdit;

public class GetVideoForEditQuery : IRequest<Envelope<VideoForEdit>>
{
    #region Public Properties

    public Guid? Id { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class GetVideoForEditQueryHandler : IRequestHandler<GetVideoForEditQuery, Envelope<VideoForEdit>>
    {
        #region Private Fields

        private readonly IVideoUseCase _videoUseCase;

        #endregion Private Fields

        #region Public Constructors

        public GetVideoForEditQueryHandler(IVideoUseCase videoUseCase)
        {
            _videoUseCase = videoUseCase;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<VideoForEdit>> Handle(GetVideoForEditQuery request, CancellationToken cancellationToken)
        {
            return await _videoUseCase.GetVideo(request);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}
