using AutoMapper;

namespace OttApiPlatform.Application.Features.ContentManagement.Assets.Commands.UpdateAsset;
public class UpdateAssetCommand : IRequest<Envelope<string>>
{
    #region Public Properties

    public Guid Id { get; set; }

    public AssetType Type { get; set; }

    public AssetCreationStatus CreationStatus { get; set; }

    public string Url { get; set; }

    public string DownloadUrl { get; set; }

    public string FileName { get; set; }

    public bool IsTemporary { get; set; } = true;

    public bool ClosedCaptions { get; set; }

    public bool IsTestAsset { get; set; }

    public Guid? LanguageId { get; set; }

    public Guid? ImageId { get; set; }

    public Guid? DocumentId { get; set; }

    public Guid? MuxAssetId { get; set; }

    #endregion

    public class UpdateAssetCommandHandler : IRequestHandler<UpdateAssetCommand, Envelope<string>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateAssetCommandHandler> _logger;

        public UpdateAssetCommandHandler(IApplicationDbContext dbContext, IMapper mapper, ILogger<UpdateAssetCommandHandler> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Envelope<string>> Handle(UpdateAssetCommand request, CancellationToken cancellationToken)
        {
            var asset = await _dbContext.Assets.FindAsync(request.Id);
            //asset.Type = request.Type;
            //asset.CreationStatus = request.CreationStatus;
            //asset.Url = request.Url;
            //asset.DownloadUrl = request.DownloadUrl;
            //asset.FileName = request.FileName;
            //asset.IsTemporary = request.IsTemporary;
            //asset.ClosedCaptions = request.ClosedCaptions;
            //asset.IsTestAsset = request.IsTestAsset;
            //asset.LanguageId = request.LanguageId;
            //asset.ImageId = request.ImageId;
            //asset.DocumentId = request.DocumentId;
            //asset.MuxAssetId = request.MuxAssetId;


            _mapper.Map(request, asset);

            try
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
                return Envelope<string>.Result.Ok("Asset updated successfully");
            }
            catch (Exception ex)
            {
                // Log the exception (assuming you have a logger, otherwise you can use Console.WriteLine)
                //_logger.LogError(ex, "Error updating asset with ID {AssetId}", request.Id);

                // Return a server error response
                return Envelope<string>.Result.ServerError("An error occurred while updating the asset.");
            }
        }
    }
}
