using OttApiPlatform.Domain.Entities.ContentManagement;
using OttApiPlatform.Domain.Entities.Mux;

namespace OttApiPlatform.Infrastructure.Persistence;

public sealed class ApplicationDbContext : IdentityDbContext<ApplicationUser,
                                                      ApplicationRole,
                                                      string,
                                                      ApplicationUserClaim,
                                                      ApplicationUserRole,
                                                      ApplicationUserLogin,
                                                      ApplicationRoleClaim,
                                                      ApplicationUserToken>, IApplicationDbContext
{
    #region Private Fields

    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITenantResolver _tenantResolver;
    private readonly IUtcTimeService _utcTimeService;

    #endregion Private Fields

    #region Public Constructors

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
                                IHttpContextAccessor httpContextAccessor,
                                ITenantResolver tenantResolver,
                                IUtcTimeService utcTimeService) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
        _tenantResolver = tenantResolver;
        Current = this;
        _utcTimeService = utcTimeService;
    }

    #endregion Public Constructors

    #region Public Properties

    // Application configuration entities.
    public override DbSet<ApplicationUserRole> UserRoles { get; set; }
    public override DbSet<ApplicationUserClaim> UserClaims { get; set; }
    public override DbSet<ApplicationUserLogin> UserLogins { get; set; }
    public override DbSet<ApplicationRoleClaim> RoleClaims { get; set; }
    public override DbSet<ApplicationUserToken> UserTokens { get; set; }
    public DbSet<ApplicationUserAttachment> ApplicationUserAttachments { get; set; }
    public DbSet<ApplicationPermission> ApplicationPermissions { get; set; }

    // Application configuration entities.
    public DbSet<UserSettings> UserSettings { get; set; }

    public DbSet<PasswordSettings> PasswordSettings { get; set; }
    public DbSet<LockoutSettings> LockoutSettings { get; set; }
    public DbSet<SignInSettings> SignInSettings { get; set; }
    public DbSet<TokenSettings> TokenSettings { get; set; }
    public DbSet<FileStorageSettings> FileStorageSettings { get; set; }

    // Application-specific entities.
    public DbSet<Applicant> Applicants { get; set; }

    public DbSet<Reference> References { get; set; }

    // Application-generic entities.
    public DbSet<Report> Reports { get; set; }
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<Domain.Entities.AccountInfoModel> AccountInfo { get; set; }

    public DbSet<AssetModel> Assets { get; set; }
    public DbSet<AssetStorageModel> AssetStorages { get; set; }
    public DbSet<AudioModel> Audios { get; set; }
    public DbSet<CategoryModel> Categories { get; set; }
    public DbSet<ContentCategoryModel> ContentCategories { get; set; }
    public DbSet<ContentSettingsCountryModel> ContentCountryRestrictions { get; set; }
    public DbSet<ContentModel> Contents { get; set; }
    public DbSet<ContentPersonModel> ContentPeople { get; set; }
    public DbSet<CollectionsAssetModel> ContentCollections { get; set; }
    public DbSet<SeriesAssetModel> ContentSeries { get; set; }
    public DbSet<ContentSettingsModel> ContentSettings { get; set; }
    public DbSet<ContentTagModel> ContentTags { get; set; }
    public DbSet<CountryModel> Countries { get; set; }
    public DbSet<DocumentModel> Documents { get; set; }
    public DbSet<ImageModel> Images { get; set; }
    public DbSet<LanguageModel> Languages { get; set; }
    public DbSet<LiveStreamModel> LiveStreams { get; set; }
    public DbSet<MuxAssetModel> MuxAssets { get; set; }
    public DbSet<MuxAssetTrackModel> MuxAssetTracks { get; set; }
    public DbSet<MuxPlaybackIdModel> MuxPlaybackIds { get; set; }
    public DbSet<MuxSettingsModel> MuxSettings { get; set; }
    public DbSet<PersonModel> People { get; set; }
    public DbSet<CollectionModel> Collections { get; set; }
    public DbSet<SeasonModel> Seasons { get; set; }
    public DbSet<SeriesModel> Series { get; set; }
    public DbSet<StorageLocationModel> StorageLocations { get; set; }
    public DbSet<SubtitleModel> Subtitles { get; set; }
    public DbSet<SwimLaneModel> SwimLanes { get; set; }
    public DbSet<SwimLaneContentModel> SwimLaneContent { get; set; }
    public DbSet<TagModel> Tags { get; set; }


    // DbContext-related properties.
    public DbContext Current { get; }

    #endregion Public Properties

    #region Private Properties

    private Guid? TargetTenantIdProvidedByHost { get; set; }

    #endregion Private Properties

    #region Public Methods

    public async Task<bool> EnsureTenantDatabaseCreated()
    {
        // If the tenant mode is multi-tenant and separate database per tenant is used.
        if (!_tenantResolver.IsHost &&
            _tenantResolver.TenantMode == TenantMode.MultiTenant &&
            _tenantResolver.DataIsolationStrategy == DataIsolationStrategy.SeparateDatabasePerTenant)
        {
            // Ensure that the database is created.
            return await Database.EnsureCreatedAsync();
        }
        return false;
    }

    // Override the SaveChangesAsync method of DbContext.
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        // Iterate through all the entries in the ChangeTracker and validate them.
        foreach (var entry in ChangeTracker.Entries())
        {
            var validationContext = new ValidationContext(entry);
            Validator.ValidateObject(entry, validationContext);
        }

        // Get the current user ID and current UTC utcTime.
        var userId = _httpContextAccessor.GetUserId();
        var utcNow = _utcTimeService.GetUtcNow();

        // Iterate through all the auditable entries in the ChangeTracker and set the
        // created/modified/deleted properties accordingly.
        foreach (var entry in ChangeTracker.Entries<IAuditable>())
            switch (entry)
            {
                // For added entities, set the created properties.
                case { State: EntityState.Added }:
                    entry.Property("CreatedOn").CurrentValue = utcNow;
                    entry.Property("CreatedBy").CurrentValue = userId;
                    break;

                // For modified entities, set the modified properties.
                case { State: EntityState.Modified }:
                    entry.Property("ModifiedOn").CurrentValue = utcNow;
                    entry.Property("ModifiedBy").CurrentValue = userId;
                    break;

                // For deleted entities, if they are soft deletable, set the deleted properties.
                case { State: EntityState.Deleted }:
                    if (entry.Entity is ISoftDeletable)
                    {
                        entry.Property("DeletedOn").CurrentValue = utcNow;
                        entry.Property("DeletedBy").CurrentValue = userId;
                    }
                    break;
            }

        // Get the current tenant ID.
        var currentTenantId = _tenantResolver.GetTenantId();

        // If the application is running in multi-tenant mode, iterate through the entries and set
        // the tenant ID accordingly.
        if (_tenantResolver.TenantMode == TenantMode.MultiTenant)
        {
            // Iterate through all the entries that must have a tenant ID.
            foreach (var entry in ChangeTracker.Entries<IMustHaveTenant>())
                switch (entry)
                {
                    // For added entities, set the tenant ID to the current tenant ID.
                    case { State: EntityState.Added }:
                        entry.Property("TenantId").CurrentValue = ThrowExceptionIfTenantIsNull(currentTenantId);
                        break;

                    // For modified entities, if the tenant ID is null, set it to the current tenant ID.
                    case { State: EntityState.Modified }:
                        entry.Property("TenantId").CurrentValue ??= ThrowExceptionIfTenantIsNull(currentTenantId);
                        break;
                }

            // Iterate through all the entries that may have a tenant ID.
            foreach (var entry in ChangeTracker.Entries<IMayHaveTenant>())
                switch (entry.State)
                {
                    // For added entities, set the tenant ID to either the target tenant ID provided
                    // by the host, or the current tenant ID.
                    case EntityState.Added:
                        entry.Property("TenantId").CurrentValue = TargetTenantIdProvidedByHost ?? currentTenantId;
                        break;

                    // For modified entities, set the tenant ID to either the target tenant ID
                    // provided by the host, or the current tenant ID.
                    case EntityState.Modified:
                        entry.Property("TenantId").CurrentValue = TargetTenantIdProvidedByHost ?? currentTenantId;
                        break;
                }
        }

        // Iterate over all entities that implement the ISoftDeletable interface and have been
        // marked for deletion. For such entities, it sets their state to EntityState.Unchanged,
        // which ensures that only the IsDeleted flag will be updated and sent to the database
        // during the next call to SaveChangesAsync().
        foreach (var entry in ChangeTracker.Entries<ISoftDeletable>().Where(x => x.State == EntityState.Deleted))
        {
            entry.State = EntityState.Unchanged;
            entry.Property("IsDeleted").CurrentValue = true;
        }

        try
        {
            // Saves all changes made in this context to the database. Any validation errors will be
            // thrown as exceptions. If the operation is successful, it returns the number of
            // entities written to the database.
            var totalChanges = await base.SaveChangesAsync(cancellationToken);

            // Resets the TargetTenantIdProvidedByHost property to null. This property is used to
            // temporarily override the tenant ID for certain entities.
            TargetTenantIdProvidedByHost = null;

            // Returns the number of entities written to the database.
            return totalChanges;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            // If a concurrency error occurs during the call to SaveChangesAsync(), it throws a
            // DbUpdateConcurrencyException with a message that instructs the user to try submitting
            // the form again.
            throw new DbUpdateConcurrencyException("Data may have been modified or deleted since entities were loaded. Try submitting the form again.", ex);
        }
        catch (Exception ex)
        {
            // Otherwise, it throws a generic Exception with the inner exception's message or a
            // string representation of the exception.
            throw new Exception(ex.InnerException?.Message ?? ex.ToString());
        }
    }

    // Sets the tenant id provided by the host.
    public void SetTargetTenantId(Guid tenantId)
    {
        TargetTenantIdProvidedByHost = tenantId;
    }

    #endregion Public Methods

    #region Protected Methods

    // Configures the DbContext with the tenant-specific connection string and other options.
    protected override void OnConfiguring(DbContextOptionsBuilder contextOptionsBuilder)
    {
        // Sets the connection string for the current tenant.
        _tenantResolver.SetConnectionString(contextOptionsBuilder);

        // Calls the base method to configure other DbContext options.
        base.OnConfiguring(contextOptionsBuilder);
    }

    // Configures the entities in the DbContext.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configures entities based on multi-tenant mode.
        ConfigureEntitiesBasedOnMultiTenantMode(modelBuilder);

        // Configures entities to support soft deletion.
        DbContextHelper.ConfigureSoftDeletableEntities(modelBuilder);

        // Configures entities for storing settings in a separate schema.
        DbContextHelper.ConfigureSettingsSchemaEntities(modelBuilder);

        DbContextHelper.ConfigureUniqueConstraints(modelBuilder);

        DbContextHelper.ConfigureManyToManyRelationships(modelBuilder);

        DbContextHelper.ConfigureOneToManyRelationships(modelBuilder);

        DbContextHelper.ConfigureOneToOneRelationships(modelBuilder);

        DbContextHelper.ConfigurePrecisionForDecimal(modelBuilder);

        // Calls the base method to continue with further configuration.
        base.OnModelCreating(modelBuilder);

        // Applies entity configurations from the current assembly.
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    #endregion Protected Methods

    #region Private Methods

    /// <summary>
    /// Configures the entities based on the multi-tenant mode.
    /// </summary>
    /// <param name="modelBuilder">The model builder to be configured.</param>
    private void ConfigureEntitiesBasedOnMultiTenantMode(ModelBuilder modelBuilder)
    {
        // Depending on the tenant mode, configure the entities and set query filters.
        switch (_tenantResolver.TenantMode)
        {
            case TenantMode.MultiTenant:
                DbContextHelper.ConfigureMultiTenantsEntities(modelBuilder);
                SetQueryFilterOnMultiTenantsEntities(modelBuilder);
                break;

            case TenantMode.SingleTenant:
                DbContextHelper.IgnoreMappingIMustHaveTenant(modelBuilder);
                DbContextHelper.IgnoreMappingIMayHaveTenant(modelBuilder);
                break;
        }

        // Set query filter on permission entity regardless of the tenant mode.
        SetQueryFilterOnPermissionEntity(modelBuilder);
    }

    /// <summary>
    /// Sets a query filter on the multi-tenant entities based on the current tenant.
    /// </summary>
    /// <param name="builder">The model builder to be configured.</param>

    private void SetQueryFilterOnMultiTenantsEntities(ModelBuilder builder)
    {
        // Set the query filter for entities that may have a tenant ID.
        builder.SetQueryFilter<IMayHaveTenant>(p => p.TenantId == _tenantResolver.GetTenantId());

        // Set the query filter for entities that must have a tenant ID.
        builder.SetQueryFilter<IMustHaveTenant>(p => p.TenantId == _tenantResolver.GetTenantId());
    }

    /// <summary>
    /// Sets a query filter on the permission entity based on the current tenant.
    /// </summary>
    /// <param name="builder">The model builder to be configured.</param>
    private void SetQueryFilterOnPermissionEntity(ModelBuilder builder)
    {
        // Set the query filter for the ApplicationPermission entity, based on tenant or host visibility.
        builder.SetQueryFilter<ApplicationPermission>(p => p.TenantVisibility == !_tenantResolver.IsHost || p.HostVisibility == _tenantResolver.IsHost);
    }

    /// <summary>
    /// Throws an exception if the provided tenant ID or target tenant ID provided by the host is null.
    /// </summary>
    /// <param name="tenantId">The ID of the tenant to be checked.</param>
    /// <returns>The non-null tenant ID or target tenant ID provided by the host.</returns>
    private Guid? ThrowExceptionIfTenantIsNull(Guid? tenantId)
    {
        // If the tenant ID is not null, return it.
        if (tenantId != null)
            return tenantId;

        // If the tenant ID is null but was provided by the host, return it.
        if (TargetTenantIdProvidedByHost != null)
            return TargetTenantIdProvidedByHost;

        // If no tenant ID was provided, throw an exception.
        throw new Exception("IMustHaveTenant entity must have tenant Id.");
    }

    #endregion Private Methods
}