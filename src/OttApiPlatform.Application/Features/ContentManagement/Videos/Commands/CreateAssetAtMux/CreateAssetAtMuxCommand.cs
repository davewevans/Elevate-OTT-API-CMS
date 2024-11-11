using OttApiPlatform.Application.Common.Contracts.Mux;

namespace OttApiPlatform.Application.Features.ContentManagement.Videos.Commands.CreateAssetAtMux;

public class CreateAssetAtMuxCommand : IRequest<Envelope<CreateAssetAtMuxResponse>>
{
    #region Public Properties

    public string BlobUrl { get; set; } = string.Empty;
    public string LanguageCode { get; set; } = string.Empty;
    public bool ClosedCaption { get; set; } 
    public bool IsTestAsset { get; set; } 
    public bool Mp4Support { get; set; } 
    public string Passthrough { get; set; } = string.Empty;

    #endregion Public Properties


    #region Public Classes

    public class CreateAssetAtMuxCommandHandler : IRequestHandler<CreateAssetAtMuxCommand, Envelope<CreateAssetAtMuxResponse>>
    {
        #region Private Fields
        private readonly IMuxAssetService _muxAssetService;

        public CreateAssetAtMuxCommandHandler(IMuxAssetService muxAssetService)
        {
            _muxAssetService = muxAssetService;
        }

        #endregion Private Fields

        public async Task<Envelope<CreateAssetAtMuxResponse>> Handle(CreateAssetAtMuxCommand request, CancellationToken cancellationToken)
        {
            var response = await _muxAssetService.CreateAssetAtMuxAsync(request);
            return Envelope<CreateAssetAtMuxResponse>.Result.Ok(response);
        }
    }

    #endregion Public Classes

}
