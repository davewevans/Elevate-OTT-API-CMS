namespace OttApiPlatform.Application.Features.Tenants.Queries.GetTenants;

public class GetTenantsQuery : FilterableQuery, IRequest<Envelope<TenantsResponse>>
{
    #region Public Classes

    public class GetTenantsQueryHandler : IRequestHandler<GetTenantsQuery, Envelope<TenantsResponse>>
    {
        #region Private Fields

        private readonly IApplicationDbContext _dbContext;

        #endregion Private Fields

        #region Public Constructors

        public GetTenantsQueryHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<TenantsResponse>> Handle(GetTenantsQuery request, CancellationToken cancellationToken)
        {
            // Create a query that searches for tenants whose name contains the search text or
            // returns all tenants if no search text is specified.
            var query = _dbContext.Tenants.Where(t => t.Name.Contains(request.SearchText) || string.IsNullOrWhiteSpace(request.SearchText)).AsQueryable();

            // If a sort column is specified, sort the results by that column; otherwise, sort by
            // tenant name.
            query = !string.IsNullOrWhiteSpace(request.SortBy) ? query.SortBy(request.SortBy) : query.OrderBy(r => r.Name);

            // Execute the query and return a paged list of tenant items.
            var tenantItems = await query.Select(q => TenantItem.MapFromEntity(q))
                                         .AsNoTracking()
                                         .ToPagedListAsync(request.PageNumber, request.RowsPerPage);

            // Create a response object containing the paged list of tenant items.
            var tenantsResponse = new TenantsResponse
            {
                Tenants = tenantItems
            };

            // Return a successful response with the tenants response object.
            return Envelope<TenantsResponse>.Result.Ok(tenantsResponse);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}