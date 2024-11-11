using OttApiPlatform.Application.Common.Contracts.Mux;
using OttApiPlatform.Application.Common.Contracts.UseCases.Content;
using OttApiPlatform.Application.Features.ContentManagement.Videos.Queries.GetVideoForEdit;

namespace OttApiPlatform.Application.Features.ContentManagement.Videos.Commands.DeleteVideo;

public class DeleteVideoCommand : IRequest<Envelope<string>>
{
    #region Public Properties

    public Guid Id { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class DeleteVideoCommandHandler : IRequestHandler<DeleteVideoCommand, Envelope<string>>
    {
        #region Private Fields

        private readonly IVideoUseCase _videoUseCase;
        private readonly IMuxAssetService _muxAssetService;
        private readonly ILogger<DeleteVideoCommandHandler> _logger;

        #endregion Private Fields

        #region Public Constructors

        public DeleteVideoCommandHandler(IVideoUseCase videoUseCase, IMuxAssetService muxAssetService)
        {
            _videoUseCase = videoUseCase;
            _muxAssetService = muxAssetService;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<string>> Handle(DeleteVideoCommand request, CancellationToken cancellationToken)
        {
            // Delete asset at Mux
            var videoToDelete = await _videoUseCase.GetVideo(new GetVideoForEditQuery { Id = request.Id });

            if (videoToDelete.Payload?.AssetId is null)
            {
                return await _videoUseCase.DeleteVideo(request);
            }

            var assetExists = await _muxAssetService.AssetExistsAsync(videoToDelete.Payload.AssetId);
            if (assetExists)
            {
                await _muxAssetService.DeleteAssetFromMuxAsync(videoToDelete.Payload.AssetId);
            }

            return await _videoUseCase.DeleteVideo(request);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}
