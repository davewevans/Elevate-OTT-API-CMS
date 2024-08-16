namespace OttApiPlatform.Application.Features.Tenants.Commands.UpdateTenant;

public class UpdateTenantCommand : IRequest<Envelope<string>>
{
    #region Public Properties

    public string Id { get; set; }
    public string Name { get; set; }
    public string Subdomain { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidUntil { get; set; }

    #endregion Public Properties

    #region Public Methods

    public void MapToEntity(Tenant tenant)
    {
        if (tenant == null)
            throw new ArgumentNullException(nameof(tenant));

        tenant.Name = Name;
    }

    #endregion Public Methods

    public class UpdateTenantCommandHandler : IRequestHandler<UpdateTenantCommand, Envelope<string>>
    {
        #region Private Fields

        private readonly IApplicationDbContext _dbContext;
        private readonly IOptions<IdentityOptions> _identityOptions;

        #endregion Private Fields

        #region Public Constructors

        public UpdateTenantCommandHandler(IApplicationDbContext dbContext, IOptions<IdentityOptions> identityOptions)
        {
            _dbContext = dbContext;
            _identityOptions = identityOptions;
            DisablePasswordComplexity();
        }

        #region Public Methods

        public async Task<Envelope<string>> Handle(UpdateTenantCommand request, CancellationToken cancellationToken)
        {
            // Check if the ID is null or empty.
            if (string.IsNullOrEmpty(request.Id))
                return Envelope<string>.Result.BadRequest(Resource.Invalid_tenant_Id);

            // Check if the ID is a valid GUID.
            if (!Guid.TryParse(request.Id, out var tenantId))
                return Envelope<string>.Result.BadRequest(Resource.Invalid_tenant_Id);

            // Check if any tenant with the same name already exists in the database, excluding the
            // current tenant
            var tenantExist = await _dbContext.Tenants.AnyAsync(t => t.Id != tenantId && t.Name == request.Name);

            // Check if the tenant exists. If so, throw an exception.
            if (tenantExist)
                return Envelope<string>.Result.ServerError(Resource.A_tenant_with_the_same_name_already_exists__Please_choose_a_different_name);

            // Get the tenant from the database.
            var tenant = await _dbContext.Tenants.Where(t => t.Id == tenantId).FirstOrDefaultAsync(cancellationToken: cancellationToken);

            // Check if the tenant is null.
            if (tenant == null)
                return Envelope<string>.Result.NotFound(Resource.Unable_to_load_tenant);

            // Map the update request to the tenant entity.
            request.MapToEntity(tenant);

            // Update the tenant in the database.
            _dbContext.Tenants.Update(tenant);
            await _dbContext.SaveChangesAsync(cancellationToken);

            // Return a success message.
            return Envelope<string>.Result.Ok(Resource.Tenant_has_been_updated_successfully);
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