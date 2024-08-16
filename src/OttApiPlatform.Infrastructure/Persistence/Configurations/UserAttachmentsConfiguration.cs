namespace OttApiPlatform.Infrastructure.Persistence.Configurations;

public class UserAttachmentsConfiguration : IEntityTypeConfiguration<ApplicationUserAttachment>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<ApplicationUserAttachment> builder)
    {
        builder.ToTable("AspNetUserAttachments");

        builder.Property(userAttachment => userAttachment.FileUri)
               .IsRequired();

        builder.Property(userAttachment => userAttachment.UserId)
               .IsRequired();
    }

    #endregion Public Methods
}