namespace OttApiPlatform.Application.Features.MyTenant.Commands.DeleteMyTenant;

public class DeleteMyTenantCommand : IRequest<Envelope<string>>
{
    #region Public Properties

    public string Id { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class DeleteMyTenantCommandHandler : IRequestHandler<DeleteMyTenantCommand, Envelope<string>>
    {
        #region Private Fields

        private readonly IApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion Private Fields

        #region Public Constructors

        public DeleteMyTenantCommandHandler(IApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<string>> Handle(DeleteMyTenantCommand request, CancellationToken cancellationToken)
        {
            // Check if the provided Id is null or empty.
            if (string.IsNullOrEmpty(request.Id))
                return Envelope<string>.Result.BadRequest(Resource.Invalid_tenant_Id);

            // Check if the provided Id is not a valid GUID.
            if (!Guid.TryParse(request.Id, out var tenantId))
                return Envelope<string>.Result.BadRequest(Resource.Invalid_tenant_Id);

            // Get the current user's tenant ID based on their user ID retrieved from the HTTP context
            var currentUserTenantId = await _dbContext.Users.Where(u => u.Id == _httpContextAccessor.GetUserId())
                                                            .Select(u => u.TenantId)
                                                            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            // Find the tenant with the provided Id.
            var tenant = await _dbContext.Tenants.Where(t => t.Id == tenantId && t.Id == currentUserTenantId)
                                                 .FirstOrDefaultAsync(cancellationToken: cancellationToken);

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
    }

    #endregion Public Classes
}