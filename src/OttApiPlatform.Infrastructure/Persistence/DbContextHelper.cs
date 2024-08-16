namespace OttApiPlatform.Infrastructure.Persistence;

/// <summary>
/// A static class with helper methods for configuring Entity Framework Core DbContext instances.
/// </summary>
public sealed class DbContextHelper
{
    #region Public Methods

    /// <summary>
    /// Configures the given <paramref name="builder"/> to ignore mapping of properties from. <see cref="IMayHaveTenant"/>.
    /// </summary>
    /// <param name="builder">The <see cref="ModelBuilder"/> instance to configure.</param>
    public static void IgnoreMappingIMayHaveTenant(ModelBuilder builder)
    {
        // Get the names of the properties in the IMayHaveTenant interface.
        var propertyNames = typeof(IMayHaveTenant).GetProperties().Select(p => p.Name).ToList();

        // Get the entity types that implement the IMayHaveTenant interface.
        var entityTypes = builder.Model.GetEntityTypes().Where(t => typeof(IMayHaveTenant).IsAssignableFrom(t.ClrType));

        foreach (var entityType in entityTypes)
        {
            var entityTypeBuilder = builder.Entity(entityType.ClrType);

            // Ignore the properties from the IMayHaveTenant interface.
            foreach (var propertyName in propertyNames)
                entityTypeBuilder.Ignore(propertyName);
        }
    }

    /// <summary>
    /// Configures the given <paramref name="builder"/> to ignore mapping of properties from. <see cref="IMustHaveTenant"/>.
    /// </summary>
    /// <param name="builder">The <see cref="ModelBuilder"/> instance to configure.</param>
    public static void IgnoreMappingIMustHaveTenant(ModelBuilder builder)
    {
        // Get the names of the properties in the IMustHaveTenant interface.
        var propertyNames = typeof(IMustHaveTenant).GetProperties().Select(p => p.Name).ToList();

        // Get the entity types that implement the IMustHaveTenant interface.
        var entityTypes = builder.Model.GetEntityTypes().Where(t => typeof(IMustHaveTenant).IsAssignableFrom(t.ClrType));

        foreach (var entityType in entityTypes)
        {
            var entityTypeBuilder = builder.Entity(entityType.ClrType);

            // Ignore the properties from the IMustHaveTenant interface.
            foreach (var propertyName in propertyNames)
                entityTypeBuilder.Ignore(propertyName);
        }
    }

    /// <summary>
    /// Configures the given <paramref name="builder"/> to use the "Settings" schema for. entities
    /// that implement the <see cref="ISettingsSchema"/> interface.
    /// </summary>
    /// <param name="builder">The <see cref="ModelBuilder"/> instance to configure.</param>
    public static void ConfigureSettingsSchemaEntities(ModelBuilder builder)
    {
        // Create SQL database schema for the settings tables.
        foreach (var entityType in builder.Model.GetEntityTypes().Where(e => typeof(ISettingsSchema).IsAssignableFrom(e.ClrType)))
            builder.Entity(entityType.ClrType).ToTable(entityType.ClrType.Name, "Settings");
    }

    /// <summary>
    /// Configures the given <paramref name="builder"/> use the multi-tenancy for entities. that
    /// implement the <see cref="IMustHaveTenant"/> and <see cref="IMayHaveTenant"/> interfaces. by
    /// setting the TenantId property as required or optional.
    /// </summary>
    /// <param name="builder">The model builder.</param>
    public static void ConfigureMultiTenantsEntities(ModelBuilder builder)
    {
        // Set the "TenantId" property as required for entities that implement the "IMustHaveTenant" interface.
        foreach (var entityType in builder.Model.GetEntityTypes().Where(e => typeof(IMustHaveTenant).IsAssignableFrom(e.ClrType)))
            builder.Entity(entityType.ClrType).Property<Guid>("TenantId").IsRequired();

        // Set the "TenantId" property as optional for entities that implement the "IMayHaveTenant" interface.
        foreach (var entityType in builder.Model.GetEntityTypes().Where(e => typeof(IMayHaveTenant).IsAssignableFrom(e.ClrType)))
            builder.Entity(entityType.ClrType).Property<Guid?>("TenantId").IsRequired(false);
    }

    /// <summary>
    /// Configures the given model builder to add soft delete functionality to all entities.
    /// implementing the <see cref="ISoftDeletable"/> interface.
    /// </summary>
    /// <param name="builder">The <see cref="ModelBuilder"/> instance to configure.</param>
    public static void ConfigureSoftDeletableEntities(ModelBuilder builder)
    {
        //Creating navigation or shadow properties for all entity.
        foreach (var entityType in builder.Model.GetEntityTypes().Where(e => typeof(ISoftDeletable).IsAssignableFrom(e.ClrType)))
            builder.Entity(entityType.ClrType).Property<bool>("IsDeleted").IsRequired();

        //Filter out soft-deleted entities by default.
        builder.SetQueryFilter<ISoftDeletable>(p => EF.Property<bool>(p, "IsDeleted") == false);
    }

    #endregion Public Methods
}