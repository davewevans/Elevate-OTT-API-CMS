using AutoMapper;

namespace OttApiPlatform.Application.Features.ContentManagement.Assets.Queries.GetAssets;

public class GetAssetsQuery : FilterableQuery, IRequest<Envelope<AssetsResponse>>
{
    public string Search { get; set; }
    public AssetType Type { get; set; }

    public class GetAssetsQueryHandler : IRequestHandler<GetAssetsQuery, Envelope<AssetsResponse>>
    {
        #region Private Fields

        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        #endregion Private Fields


        #region Public Constructors

        public GetAssetsQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        #endregion Public Constructors

        public async Task<Envelope<AssetsResponse>> Handle(GetAssetsQuery request,
            CancellationToken cancellationToken)
        {
            var query = _dbContext.Assets.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(x => x.FileName.Contains(request.Search));
            }

            var assets = await query.Select(x => _mapper.Map<AssetItem>(x))
                .AsNoTracking()
                .ToPagedListAsync(request.PageNumber, request.RowsPerPage);

            var response = new AssetsResponse { Assets = assets };

            return Envelope<AssetsResponse>.Result.Ok(response);
        }
    }
}
