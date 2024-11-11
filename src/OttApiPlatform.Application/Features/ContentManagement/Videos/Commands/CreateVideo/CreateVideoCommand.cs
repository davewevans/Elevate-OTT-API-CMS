using OttApiPlatform.Application.Common.Contracts.UseCases.Content;
using OttApiPlatform.Domain.DTOs;

namespace OttApiPlatform.Application.Features.ContentManagement.Videos.Commands.CreateVideo;

public class CreateVideoCommand : BaseAssetDto, IRequest<Envelope<CreateVideoResponse>>
{
    #region Public Properties

    public Guid TenantId { get; set; }  
    public bool Mp4Support { get; set; }

    #endregion Public Properties



    #region Public Classes
    public class CreateVideoCommandHandler : IRequestHandler<CreateVideoCommand, Envelope<CreateVideoResponse>>
    {
        #region Private Fields
        private readonly IVideoUseCase _videoUseCaseUseCase;
        #endregion Private Fields

        #region Public Constructors
        public CreateVideoCommandHandler(IVideoUseCase videoUseCase)
        {
            _videoUseCaseUseCase = videoUseCase;
        }
        #endregion Public Constructors

        #region Public Methods
        public async Task<Envelope<CreateVideoResponse>> Handle(CreateVideoCommand request, CancellationToken cancellationToken)
        {
            return await _videoUseCaseUseCase.AddVideo(request);
        }
        #endregion Public Methods
    }
    #endregion Public Classes
}
