namespace OttApiPlatform.Application.Features.Tenants.Queries.GetTenantForEdit;

public class GetTenantForEditQuery : IRequest<Envelope<TenantForEditResponse>>
{
    #region Public Properties

    public string Id { get; set; }

    #endregion Public Properties

    public class GetTenantForEditQueryHandler : IRequestHandler<GetTenantForEditQuery, Envelope<TenantForEditResponse>>
    {
        #region Private Fields

        private readonly IApplicationDbContext _dbContext;
        private readonly IOptions<IdentityOptions> _identityOptions;

        #endregion Private Fields

        #region Public Constructors

        public GetTenantForEditQueryHandler(IApplicationDbContext dbContext, IOptions<IdentityOptions> identityOptions)
        {
            _dbContext = dbContext;
            _identityOptions = identityOptions;
            DisablePasswordComplexity();
        }

        #region Public Methods

        public async Task<Envelope<TenantForEditResponse>> Handle(GetTenantForEditQuery request, CancellationToken cancellationToken)
        {
            // Try to parse the tenant ID from the request.
            if (!Guid.TryParse(request.Id, out var tenantId))
                return Envelope<TenantForEditResponse>.Result.BadRequest(Resource.Invalid_tenant_Id);

            // Query the database for a tenant with the given ID.
            var tenant = await _dbContext.Tenants.Where(t => t.Id == tenantId).FirstOrDefaultAsync(cancellationToken: cancellationToken);

            // If no tenant is found with the given ID, return a not found response.
            if (tenant == null)
                return Envelope<TenantForEditResponse>.Result.NotFound(Resource.Unable_to_load_tenant);

            // Convert the tenant entity to a response DTO.
            var tenantForEditResponse = TenantForEditResponse.MapFromEntity(tenant);

            // Return a successful response with the converted tenant DTO.
            return Envelope<TenantForEditResponse>.Result.Ok(tenantForEditResponse);
        }

        private void DisablePasswordComplexity()
        {
            // Disable the password complexity requirements.
            _identityOptions.Value.Password.RequireDigit = false;
            _identityOptions.Value.Password.RequireLowercase = false;
            _identityOptions.Value.Password.RequireNonAlphanumeric = false;
            _identityOptions.Value.Password.RequireUppercase = false;
        }

        #endregion Public Methods
    }

    #endregion Public Constructors
}