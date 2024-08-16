namespace OttApiPlatform.Infrastructure.Persistence.Configurations;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.HasIndex(tenant => tenant.Name)
               .IsUnique();

        builder.Property(tenant => tenant.Id)
               .ValueGeneratedNever();
    }

    #endregion Public Methods
}