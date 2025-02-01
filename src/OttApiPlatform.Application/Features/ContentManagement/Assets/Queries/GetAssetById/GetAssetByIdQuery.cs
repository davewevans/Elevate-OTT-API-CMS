
using AutoMapper;

namespace OttApiPlatform.Application.Features.ContentManagement.Assets.Queries.GetAssetById;
public class GetAssetByIdQuery : IRequest<Envelope<GetAssetByIdResponse>>
{
    #region Public Properties

    public Guid Id { get; set; }

    #endregion Public Properties

    public class GetAssetByIdQueryHandler : IRequestHandler<GetAssetByIdQuery, Envelope<GetAssetByIdResponse>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAssetByIdQueryHandler> _logger;

        public GetAssetByIdQueryHandler(IApplicationDbContext dbContext, IMapper mapper, ILogger<GetAssetByIdQueryHandler> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<Envelope<GetAssetByIdResponse>> Handle(GetAssetByIdQuery request, CancellationToken cancellationToken)
        {
            var asset = await _dbContext.Assets.FindAsync(request.Id);

            if (asset == null)
            {
                //_logger.LogWarning($"Asset with ID {request.Id} not found.");
                return Envelope<GetAssetByIdResponse>.Result.NotFound($"Asset with ID {request.Id} not found.");
            }
            var response = _mapper.Map<GetAssetByIdResponse>(asset);
            return Envelope<GetAssetByIdResponse>.Result.Ok(response);
        }
    }
}
