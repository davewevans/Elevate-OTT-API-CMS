namespace OttApiPlatform.Application.Features.MyTenant.Queries.GetMyTenant;

public class GetMyTenantForEditQuery : IRequest<Envelope<MyTenantForEditResponse>>
{
    #region Public Properties

    public string Name { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class GetMyTenantForEditQueryHandler : IRequestHandler<GetMyTenantForEditQuery, Envelope<MyTenantForEditResponse>>
    {
        #region Private Fields

        private readonly IApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion Private Fields

        #region Public Constructors

        public GetMyTenantForEditQueryHandler(IApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<MyTenantForEditResponse>> Handle(GetMyTenantForEditQuery request, CancellationToken cancellationToken)
        {
            // Get the current user's tenant ID based on their user ID retrieved from the HTTP context
            var currentUserTenantId = await _dbContext.Users.Where(u => u.Id == _httpContextAccessor.GetUserId())
                                                            .Select(u => u.TenantId)
                                                            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            // Query the database for a tenant with the given ID.
            var myTenant = await _dbContext.Tenants.Where(t => t.Name == request.Name && t.Id == currentUserTenantId)
                                                   .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            // If no tenant is found with the given ID, return a not found response.
            if (myTenant == null)
                return Envelope<MyTenantForEditResponse>.Result.NotFound(Resource.Unable_to_load_tenant);

            // Convert the tenant entity to a response DTO.
            var myTenantForEditResponse = MyTenantForEditResponse.MapFromEntity(myTenant);

            // Return a successful response with the converted myTenant DTO.
            return Envelope<MyTenantForEditResponse>.Result.Ok(myTenantForEditResponse);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}