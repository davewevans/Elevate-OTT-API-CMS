namespace OttApiPlatform.Infrastructure.Persistence.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        // Each User can have many UserClaims.
        builder.HasMany(user => user.Claims)
               .WithOne(userClaim => userClaim.User)
               .HasForeignKey(userClaim => userClaim.UserId)
               .IsRequired();

        // Each User can have many UserLogins.
        builder.HasMany(user => user.Logins)
               .WithOne(userLogin => userLogin.User)
               .HasForeignKey(userLogin => userLogin.UserId)
               .IsRequired();

        // Each User can have many UserTokens.
        builder.HasMany(user => user.Tokens)
               .WithOne(userToken => userToken.User)
               .HasForeignKey(userToken => userToken.UserId)
               .IsRequired();

        // Each User can have many entries in the UserRole join table.
        builder.HasMany(user => user.UserRoles)
               .WithOne(userRole => userRole.User)
               .HasForeignKey(userRole => userRole.UserId)
               .IsRequired();

        builder.HasIndex(user => user.NormalizedUserName)
               .IsUnique(false);

        builder.Property(user => user.NormalizedUserName)
               .IsRequired();

        builder.HasIndex(user => user.NormalizedEmail)
               .IsUnique(false);

        builder.Property(user => user.NormalizedEmail)
               .IsRequired();

        builder.Ignore(user => user.FullName);
    }

    #endregion Public Methods
}