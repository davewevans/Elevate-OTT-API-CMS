namespace OttApiPlatform.Application.Features.ContentManagement.Assets.Commands.DeleteAsset;

public class DeleteAssetCommand : IRequest<Envelope<string>>
{
    #region Public Properties

    public Guid Id { get; set; }

    #endregion

    public class DeleteAssetCommandHandler : IRequestHandler<DeleteAssetCommand, Envelope<string>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ILogger<DeleteAssetCommandHandler> _logger;
        public DeleteAssetCommandHandler(IApplicationDbContext dbContext, ILogger<DeleteAssetCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public async Task<Envelope<string>> Handle(DeleteAssetCommand request, CancellationToken cancellationToken)
        {
            var asset = await _dbContext.Assets.FindAsync(request.Id);

            try
            {
                _dbContext.Assets.Remove(asset);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return Envelope<string>.Result.Ok("Asset deleted successfully.");
            }
            catch(Exception ex)
            {
                //_logger.LogError(ex, $"Error deleting asset. Asset ID: {request.Id}");
                return Envelope<string>.Result.ServerError($"Failed to delete asset. Asset ID: {request.Id}");
            }
        }
    }
}
