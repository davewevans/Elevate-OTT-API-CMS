namespace OttApiPlatform.Infrastructure.Persistence.Configurations;

public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        // Each Role can have many entries in the UserRole join table.
        builder.HasMany(role => role.UserRoles)
               .WithOne(userRole => userRole.Role)
               .HasForeignKey(userRole => userRole.RoleId)
               .IsRequired();

        // Each Role can have many associated RoleClaims.
        builder.HasMany(role => role.RoleClaims)
               .WithOne(roleClaim => roleClaim.Role)
               .HasForeignKey(roleClaim => roleClaim.RoleId)
               .IsRequired();

        builder.HasIndex(role => role.NormalizedName)
               .IsUnique(false);

        builder.Property(role => role.NormalizedName)
               .IsRequired();
    }

    #endregion Public Methods
}