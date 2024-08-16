namespace OttApiPlatform.Application.Features.Tenants.Commands.DeleteTenant;

public class DeleteTenantCommand : IRequest<Envelope<string>>
{
    #region Public Properties

    public string Id { get; set; }

    #endregion Public Properties

    public class DeleteTenantCommandHandler : IRequestHandler<DeleteTenantCommand, Envelope<string>>
    {
        #region Private Fields

        private readonly IApplicationDbContext _dbContext;
        private readonly IOptions<IdentityOptions> _identityOptions;

        #endregion Private Fields

        #region Public Constructors

        public DeleteTenantCommandHandler(IApplicationDbContext dbContext, IOptions<IdentityOptions> identityOptions)
        {
            _dbContext = dbContext;
            _identityOptions = identityOptions;
            DisablePasswordComplexity();
        }

        #region Public Methods

        public async Task<Envelope<string>> Handle(DeleteTenantCommand request, CancellationToken cancellationToken)
        {
            // Check if the provided Id is null or empty.
            if (string.IsNullOrEmpty(request.Id))
                return Envelope<string>.Result.BadRequest(Resource.Invalid_tenant_Id);

            // Check if the provided Id is not a valid GUID.
            if (!Guid.TryParse(request.Id, out var tenantId))
                return Envelope<string>.Result.BadRequest(Resource.Invalid_tenant_Id);

            // Find the tenant with the provided Id.
            var tenant = await _dbContext.Tenants.Where(t => t.Id == tenantId).FirstOrDefaultAsync(cancellationToken: cancellationToken);

            // If no tenant with the provided Id is found, return a Not Found response.
            if (tenant == null)
                return Envelope<string>.Result.NotFound(Resource.The_tenant_is_not_found);

            // Remove the tenant from the database.
            _dbContext.Tenants.Remove(tenant);

            // Save the changes to the database.
            await _dbContext.SaveChangesAsync(cancellationToken);

            // Return a successful response message.
            return Envelope<string>.Result.Ok(Resource.Tenant_has_been_deleted_successfully);
        }

        #endregion Public Methods

        private void DisablePasswordComplexity()
        {
            // Disable the password complexity requirements.
            _identityOptions.Value.Password.RequireDigit = false;
            _identityOptions.Value.Password.RequireLowercase = false;
            _identityOptions.Value.Password.RequireNonAlphanumeric = false;
            _identityOptions.Value.Password.RequireUppercase = false;
        }
    }

    #endregion Public Constructors
}