namespace OttApiPlatform.Application.Common.Contracts.Infrastructure.Persistence;

/// <summary>
/// A DbContext instance represents a session with the database and can be used to query and save
/// instances of your entities. DbContext is a combination of the Unit Of Work and Repository patterns.
/// </summary>
/// <remarks>
/// <para>
/// Entity Framework Core does not support multiple parallel operations being run on the same
/// DbContext instance. This includes both parallel execution of async queries and any explicit
/// concurrent use from multiple threads. Therefore, always await async calls onDemand, or use
/// separate DbContext instances for operations that execute in parallel. See <see
/// href="https://aka.ms/efcore-docs-threading">Avoiding DbContext threading issues</see> for more
/// information and examples.
/// </para>
/// <para>
/// Typically you create a class that derives from DbContext and contains <see
/// cref="DbSet{TEntity}"/> properties for each entity in the model. If the <see
/// cref="DbSet{TEntity}"/> properties have a public setter, they are automatically initialized when
/// the instance of the derived context is created.
/// </para>
/// <para>
/// Override the <see cref="OnConfiguring(DbContextOptionsBuilder)"/> method to configure the
/// database (and other options) to be used for the context. Alternatively, if you would rather
/// perform configuration externally instead of inline in your context, you can use <see
/// cref="DbContextOptionsBuilder{TContext}"/> (or <see cref="DbContextOptionsBuilder"/>) to
/// externally create an instance of <see cref="DbContextOptions{TContext}"/> (or <see
/// cref="DbContextOptions"/>) and pass it to a base constructor of <see cref="DbContext"/>.
/// </para>
/// <para>
/// The model is discovered by running a set of conventions over the entity classes found in the
/// <see cref="DbSet{TEntity}"/> properties on the derived context. To further configure the model
/// that is discovered by convention, you can override the <see
/// cref="OnModelCreating(ModelBuilder)"/> method.
/// </para>
/// <para>
/// See <see href="https://aka.ms/efcore-docs-dbcontext">DbContext lifetime, configuration, and
/// initialization</see>, <see href="https://aka.ms/efcore-docs-query">Querying data with EF
/// Core</see>, <see href="https://aka.ms/efcore-docs-change-tracking">Changing tracking</see>, and
/// <see href="https://aka.ms/efcore-docs-saving-data">Saving data with EF Core</see> for more
/// information and examples.
/// </para>
/// </remarks>
public interface IApplicationDbContext : IDisposable
{
    #region Public Properties

    // User-related entities.
    DbSet<ApplicationUser> Users { get; set; }

    DbSet<ApplicationRole> Roles { get; set; }
    DbSet<ApplicationUserRole> UserRoles { get; set; }
    DbSet<ApplicationUserClaim> UserClaims { get; set; }
    DbSet<ApplicationUserLogin> UserLogins { get; set; }
    DbSet<ApplicationRoleClaim> RoleClaims { get; set; }
    DbSet<ApplicationUserToken> UserTokens { get; set; }
    DbSet<ApplicationUserAttachment> ApplicationUserAttachments { get; set; }
    DbSet<ApplicationPermission> ApplicationPermissions { get; set; }

    // Application configuration entities.
    DbSet<UserSettings> UserSettings { get; set; }

    DbSet<PasswordSettings> PasswordSettings { get; set; }
    DbSet<LockoutSettings> LockoutSettings { get; set; }
    DbSet<SignInSettings> SignInSettings { get; set; }
    DbSet<TokenSettings> TokenSettings { get; set; }
    DbSet<FileStorageSettings> FileStorageSettings { get; set; }

    // Application-specific entities.
    DbSet<Applicant> Applicants { get; set; }

    DbSet<Reference> References { get; set; }

    // Application-generic entities.
    DbSet<Report> Reports { get; set; }

    DbSet<Tenant> Tenants { get; set; }

    public DbSet<Domain.Entities.AccountInfo> AccountInfo { get; set; }

    // DbContext-related properties.
    DbContext Current { get; }

    DatabaseFacade Database { get; }

    #endregion Public Properties

    /// <summary>
    /// Saves all changes made in this context to the database.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method will automatically call <see
    /// cref="Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.DetectChanges"/> to.
    /// discover any changes to entity instances before saving to the underlying database. This can.
    /// be disabled via <see cref="Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AutoDetectChangesEnabled"/>.
    /// </para>
    /// <para>
    /// Entity Framework Core does not support multiple parallel operations being run on the same.
    /// DbContext instance. This includes both parallel execution of async queries and any explicit.
    /// concurrent use from multiple threads. Therefore, always await async calls onDemand, or. use
    /// separate DbContext instances for operations that execute in parallel. See <see
    /// href="https://aka.ms/efcore-docs-threading">Avoiding DbContext threading issues</see> for.
    /// more information and examples.
    /// </para>
    /// <para>
    /// See <see href="https://aka.ms/efcore-docs-saving-data">Saving data in EF Core</see> for
    /// more. information and examples.
    /// </para>
    /// </remarks>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous save operation. The task result contains the number.
    /// of state entries written to the database.
    /// </returns>
    /// <exception cref="DbUpdateException">An error is encountered while saving to the database.</exception>
    /// <exception cref="DbUpdateConcurrencyException">
    /// A concurrency violation is encountered while saving to the database. A concurrency
    /// violation. occurs when an unexpected number of rows are affected during save. This is
    /// usually because. the data in the database has been modified since it was loaded into memory.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// If the <see cref="CancellationToken"/> is canceled.
    /// </exception>

    #region Public Methods

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets the target tenant ID for the current context instance.
    /// </summary>
    void SetTargetTenantId(Guid tenantId);

    /// <summary>
    /// Ensures the creation of the tenant database.
    /// </summary>
    /// <returns>A boolean value indicating whether the tenant database was created successfully.</returns>
    Task<bool> EnsureTenantDatabaseCreated();

    #endregion Public Methods
}