using OttApiPlatform.Application.Common.Contracts.UseCases.Content;

namespace OttApiPlatform.Application.Features.ContentManagement.Videos.Queries.ExportVideos;

public class ExportVideosQuery : IRequest<Envelope<ExportVideosResponse>>
{
    #region Public Classes

    public string SearchText { get; set; }
    public string SortBy { get; set; }

    public class CreateVideoQueryHandler : IRequestHandler<ExportVideosQuery, Envelope<ExportVideosResponse>>
    {
        #region Private Fields

        private readonly IVideoUseCase _videoUseCase;

        #endregion Private Fields

        #region Public Constructors

        public CreateVideoQueryHandler(IVideoUseCase videoUseCase)
        {
            _videoUseCase = videoUseCase;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<ExportVideosResponse>> Handle(ExportVideosQuery request, CancellationToken cancellationToken)
        {
            return await _videoUseCase.ExportAsPdf(request);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}
