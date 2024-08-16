namespace OttApiPlatform.Application.Features.MyTenant.Commands.UpdateMyTenant;

public class UpdateMyTenantCommand : IRequest<Envelope<string>>
{
    #region Public Properties

    public string Id { get; set; }
    public string Name { get; set; }
    public string Subdomain { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidUntil { get; set; }

    #endregion Public Properties

    #region Public Methods

    public void MapToEntity(Tenant myTenant)
    {
        if (myTenant == null)
            throw new ArgumentNullException(nameof(myTenant));

        myTenant.Name = Name;
    }

    #endregion Public Methods

    #region Public Classes

    public class UpdateMyTenantCommandHandler : IRequestHandler<UpdateMyTenantCommand, Envelope<string>>
    {
        #region Private Fields

        private readonly IApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion Private Fields

        #region Public Constructors

        public UpdateMyTenantCommandHandler(IApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<string>> Handle(UpdateMyTenantCommand request, CancellationToken cancellationToken)
        {
            // Check if the ID is null or empty.
            if (string.IsNullOrEmpty(request.Id))
                return Envelope<string>.Result.BadRequest(Resource.Invalid_tenant_Id);

            // Check if the ID is a valid GUID.
            if (!Guid.TryParse(request.Id, out var tenantId))
                return Envelope<string>.Result.BadRequest(Resource.Invalid_tenant_Id);

            // Check if any tenant with the same name already exists in the database, excluding the
            // current tenant
            var tenantExist = await _dbContext.Tenants.AnyAsync(t => t.Id != tenantId && t.Name == request.Name, cancellationToken: cancellationToken);

            // Check if the tenant exists. If so, throw an exception.
            if (tenantExist)
                return Envelope<string>.Result.ServerError(Resource.A_tenant_with_the_same_name_already_exists__Please_choose_a_different_name);

            // Get the current user's tenant ID based on their user ID retrieved from the HTTP context
            var currentUserTenantId = await _dbContext.Users.Where(u => u.Id == _httpContextAccessor.GetUserId())
                                                            .Select(u => u.TenantId)
                                                            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            // Get the tenant from the database.
            var myTenant = await _dbContext.Tenants.Where(t => t.Id == tenantId && t.Id == currentUserTenantId)
                                                   .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            // Check if the tenant is null.
            if (myTenant == null)
                return Envelope<string>.Result.NotFound(Resource.Unable_to_load_tenant);

            // Map the update request to the tenant entity.
            request.MapToEntity(myTenant);

            // Update the tenant in the database.
            _dbContext.Tenants.Update(myTenant);

            // Save the changes to the database.
            await _dbContext.SaveChangesAsync(cancellationToken);

            // Return a success message.
            return Envelope<string>.Result.Ok(Resource.Tenant_has_been_updated_successfully);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}