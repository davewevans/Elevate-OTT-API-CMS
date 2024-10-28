namespace OttApiPlatform.Application.Features.Tenants.Commands.CreateTenant;

public class CreateTenantCommand : IRequest<Envelope<CreateTenantResponse>>
{
    #region Public Properties

    public Guid Id { get; set; }

    public string Name { get; set; }

    public string LicenseKey { get; set; }

    public string SubDomain { get; set; }

    public string CustomDomain { get; set; }

    public string StorageFileNamePrefix { get; set; }

    #endregion Public Properties

    #region Public Methods

    public Tenant MapToEntity()
    {
        return new()
        {
            Id = Id == Guid.Empty ? Guid.NewGuid() : Id,
            Name = Name,
            LicenseKey = LicenseKey,
            SubDomain = SubDomain,
            CustomDomain = CustomDomain,
            StorageFileNamePrefix = StorageFileNamePrefix,
        };
    }

    #endregion Public Methods

    public class CreateTenantCommandHandler : IRequestHandler<CreateTenantCommand, Envelope<CreateTenantResponse>>
    {
        #region Private Fields

        private readonly ITenantResolver _tenantResolver;
        private readonly IApplicationDbContext _dbContext;
        private readonly IAppSeederService _appSeederService;
        private readonly IOptions<IdentityOptions> _identityOptions;

        #endregion Private Fields

        #region Public Constructors

        public CreateTenantCommandHandler(ITenantResolver tenantResolver,
                                          IApplicationDbContext dbContext,
                                          IAppSeederService appSeederService,
                                          IOptions<IdentityOptions> identityOptions)
        {
            _tenantResolver = tenantResolver;
            _dbContext = dbContext;
            _appSeederService = appSeederService;
            _identityOptions = identityOptions;
            DisablePasswordComplexity();
        }

        #region Public Methods

        public async Task<Envelope<CreateTenantResponse>> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
        {
            // Determine the data isolation strategy based on the tenant resolver
            switch (_tenantResolver.DataIsolationStrategy)
            {
                // Shared database for all tenants
                case DataIsolationStrategy.SharedDatabaseForAllTenants:
                    {
                        // Check if any tenant with the same name already exists in the database,
                        // excluding the current tenant
                        var tenantExist = await _dbContext.Tenants.AnyAsync(t => t.Name == request.Name, cancellationToken: cancellationToken);

                        // Check if the tenant exists. If so, throw an exception.
                        if (tenantExist)
                            return Envelope<CreateTenantResponse>.Result.ServerError(Resource.A_tenant_with_the_same_name_already_exists__Please_choose_a_different_name);

                        break;
                    }

                // Separate database per tenant
                case DataIsolationStrategy.SeparateDatabasePerTenant:
                    {
                        // Ensure that the database exists.
                        var databaseCreated = await _dbContext.EnsureTenantDatabaseCreated();

                        if (databaseCreated)
                            await _appSeederService.SeedTenantWithSeparateDatabaseStrategy();

                        break;
                    }
            }

            // Map request to tenant entity.
            var tenant = request.MapToEntity();

            // Set the tenant ID in the tenant resolver.
            _tenantResolver.SetTenantId(tenant.Id);

            // Add the new tenant to the database.
            await _dbContext.Tenants.AddAsync(tenant, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            // Create a response object with the new tenant's ID and a success message.
            var createTenantResponse = new CreateTenantResponse
            {
                Id = tenant.Id,
                SuccessMessage = Resource.Tenant_has_been_created_successfully
            };

            var succeeded = await _appSeederService.SeedTenantWithSharedDatabaseStrategy();

            // Check if the identity result was successful.
            if (!succeeded)
                return Envelope<CreateTenantResponse>.Result.ServerError("Something went wrong upon seeding tenant's data.");

            // Return the create tenant response.
            return Envelope<CreateTenantResponse>.Result.Ok(createTenantResponse);
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