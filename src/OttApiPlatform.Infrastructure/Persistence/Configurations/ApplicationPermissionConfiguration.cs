namespace OttApiPlatform.Infrastructure.Persistence.Configurations;

public class ApplicationPermissionConfiguration : IEntityTypeConfiguration<ApplicationPermission>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<ApplicationPermission> builder)
    {
        builder.ToTable("AspNetPermissions");

        //builder.HasMany(p => p.Permissions)
        //    .WithOne(p => p.Parent)
        //    .HasForeignKey(pp => pp.ParentId)
        //    .OnDelete(DeleteBehavior.NoAction);
    }

    #endregion Public Methods
}