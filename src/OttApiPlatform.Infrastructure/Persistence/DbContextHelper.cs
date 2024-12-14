using Mux.Csharp.Sdk.Model;
using OttApiPlatform.Domain.Entities.ContentAccessManagement;
using OttApiPlatform.Domain.Entities.ContentManagement;
using OttApiPlatform.Domain.Entities.Mux;
using OttApiPlatform.Domain.Entities.TransactionManagement;

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
        var entityTypes = builder.Model.GetEntityTypes()
            .Where(t => typeof(IMustHaveTenant).IsAssignableFrom(t.ClrType));

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
        foreach (var entityType in builder.Model.GetEntityTypes()
                     .Where(e => typeof(ISettingsSchema).IsAssignableFrom(e.ClrType)))
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
        foreach (var entityType in builder.Model.GetEntityTypes()
                     .Where(e => typeof(IMustHaveTenant).IsAssignableFrom(e.ClrType)))
            builder.Entity(entityType.ClrType).Property<Guid>("TenantId").IsRequired();

        // Set the "TenantId" property as optional for entities that implement the "IMayHaveTenant" interface.
        foreach (var entityType in builder.Model.GetEntityTypes()
                     .Where(e => typeof(IMayHaveTenant).IsAssignableFrom(e.ClrType)))
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
        foreach (var entityType in builder.Model.GetEntityTypes()
                     .Where(e => typeof(ISoftDeletable).IsAssignableFrom(e.ClrType)))
            builder.Entity(entityType.ClrType).Property<bool>("IsDeleted").IsRequired();

        //Filter out soft-deleted entities by default.
        builder.SetQueryFilter<ISoftDeletable>(p => EF.Property<bool>(p, "IsDeleted") == false);
    }

    /// <summary>
    /// Configures unique constraints for specific properties
    /// </summary>
    /// <param name="builder"></param>
    public static void ConfigureUniqueConstraints(ModelBuilder builder)
    {
        builder.Entity<AssetStorageModel>()
            .HasIndex(c => c.Order)
            .IsUnique();

        builder.Entity<CategoryModel>()
            .HasIndex(c => c.Order)
            .IsUnique();

        builder.Entity<ContentCategoryModel>()
            .HasIndex(c => c.Order)
            .IsUnique();

        builder.Entity<ContentPersonModel>()
            .HasIndex(c => c.Order)
            .IsUnique();

        builder.Entity<CollectionsAssetModel>()
            .HasIndex(c => c.Order)
            .IsUnique();

        builder.Entity<SeriesAssetModel>()
            .HasIndex(c => c.Order)
            .IsUnique();

        builder.Entity<ContentTagModel>()
            .HasIndex(c => c.Order)
            .IsUnique();

        builder.Entity<SwimLaneContentModel>()
            .HasIndex(c => c.Order)
            .IsUnique();

        //builder.Entity<UserSubscriptionModel>()
        //    .HasIndex(c => c.Order)
        //    .IsUnique();

        //builder.Entity<RentalContentModel>()
        //    .HasIndex(c => c.Order)
        //    .IsUnique();

        builder.Entity<SeasonModel>()
            .HasIndex(c => c.SeasonNumber)
            .IsUnique();

        builder.Entity<SeasonAssetModel>()
            .HasIndex(c => c.EpisodeNumber)
            .IsUnique();
    }

    /// <summary>
    /// Configures many-to-many relationships
    /// </summary>
    /// <param name="builder"></param>
    public static void ConfigureManyToManyRelationships(ModelBuilder builder)
    {
        builder.Entity<AssetStorageModel>()
            .HasKey(t => new { t.AssetId, t.StorageLocationId });

        builder.Entity<AssetStorageModel>()
            .HasOne(asl => asl.Asset)
            .WithMany(a => a.AssetStorages)
            .HasForeignKey(asl => asl.AssetId);

        builder.Entity<AssetStorageModel>()
            .HasOne(asl => asl.StorageLocation)
            .WithMany(sl => sl.AssetStorages)
            .HasForeignKey(asl => asl.StorageLocationId);

        builder.Entity<ContentCategoryModel>()
            .HasKey(t => new { t.ContentId, t.CategoryId });

        builder.Entity<ContentCategoryModel>()
            .HasOne(cc => cc.Content)
            .WithMany(c => c.ContentCategories)
            .HasForeignKey(cc => cc.ContentId);

        builder.Entity<ContentCategoryModel>()
            .HasOne(cc => cc.Category)
            .WithMany(c => c.ContentCategories)
            .HasForeignKey(cc => cc.CategoryId);

        builder.Entity<ContentPersonModel>()
            .HasKey(t => new { t.ContentId, t.PersonId });

        builder.Entity<ContentPersonModel>()
            .HasOne(cp => cp.Content)
            .WithMany(c => c.ContentPeople)
            .HasForeignKey(cp => cp.ContentId);

        builder.Entity<ContentPersonModel>()
            .HasOne(cp => cp.Person)
            .WithMany(p => p.ContentPeople)
            .HasForeignKey(cp => cp.PersonId);

        builder.Entity<CollectionsAssetModel>()
            .HasKey(pla => new { pla.AssetId, pla.CollectionId });

        builder.Entity<CollectionsAssetModel>()
            .HasOne(cp => cp.Asset)
            .WithMany(c => c.CollectionAssets)
            .HasForeignKey(cp => cp.AssetId);

        builder.Entity<CollectionsAssetModel>()
            .HasOne(cp => cp.Collection)
            .WithMany(p => p.CollectionAssets)
            .HasForeignKey(cp => cp.CollectionId);

        builder.Entity<SeriesAssetModel>()
            .HasKey(t => new { t.AssetId, t.SeriesId });

        builder.Entity<SeriesAssetModel>()
            .HasOne(cs => cs.Asset)
            .WithMany(c => c.SeriesAssets)
            .HasForeignKey(cs => cs.AssetId);

        builder.Entity<SeriesAssetModel>()
            .HasOne(cs => cs.Series)
            .WithMany(s => s.SeriesAssets)
            .HasForeignKey(cs => cs.SeriesId);

        builder.Entity<SeasonAssetModel>()
            .HasKey(t => new { t.AssetId, t.SeasonId });

        builder.Entity<SeasonAssetModel>()
            .HasOne(cs => cs.Asset)
            .WithMany(c => c.SeasonAssets)
            .HasForeignKey(cs => cs.AssetId);

        builder.Entity<SeasonAssetModel>()
            .HasOne(cs => cs.Season)
            .WithMany(s => s.SeasonAssets)
            .HasForeignKey(cs => cs.SeasonId);

        builder.Entity<ContentTagModel>()
            .HasKey(t => new { t.ContentId, t.TagId });

        builder.Entity<ContentTagModel>()
            .HasOne(ct => ct.Content)
            .WithMany(c => c.ContentTags)
            .HasForeignKey(ct => ct.ContentId);

        builder.Entity<ContentTagModel>()
            .HasOne(ct => ct.Tag)
            .WithMany(t => t.ContentTags)
            .HasForeignKey(ct => ct.TagId);

        builder.Entity<SwimLaneContentModel>()
            .HasKey(t => new { t.ContentId, t.SwimLaneId });

        builder.Entity<SwimLaneContentModel>()
            .HasOne(sc => sc.Content)
            .WithMany(c => c.SwimLaneContent)
            .HasForeignKey(sc => sc.ContentId);

        builder.Entity<SwimLaneContentModel>()
            .HasOne(sc => sc.SwimLane)
            .WithMany(sl => sl.SwimLaneContent)
            .HasForeignKey(sc => sc.SwimLaneId);

        builder.Entity<ContentSettingsCountryModel>()
            .HasKey(t => new { t.ContentSettingsId, t.CountryId });

        builder.Entity<ContentSettingsCountryModel>()
            .HasOne(ccr => ccr.ContentSettings)
            .WithMany(cs => cs.RestrictedCountries)
            .HasForeignKey(ccr => ccr.ContentSettingsId);

        builder.Entity<ContentSettingsCountryModel>()
            .HasOne(ccr => ccr.Country)
            .WithMany(c => c.RestrictedCountries)
            .HasForeignKey(ccr => ccr.CountryId);

        //builder.Entity<UserSubscriptionModel>()
        //    .HasKey(t => new { t.UserId, t.SubscriptionId });

        //builder.Entity<UserSubscriptionModel>()
        //    .HasOne(us => us.User)
        //    .WithMany(u => u.UserSubscriptions)
        //    .HasForeignKey(us => us.UserId)
        //    .IsRequired();

        //builder.Entity<UserSubscriptionModel>()
        //    .HasOne(us => us.Subscription)
        //    .WithMany(s => s.UserSubscriptions)
        //    .HasForeignKey(us => us.SubscriptionId)
        //    .IsRequired();

        //builder.Entity<RentalContentModel>()
        //    .HasKey(t => new { t.ContentId, t.RentalId });

        //builder.Entity<RentalContentModel>()
        //    .HasOne(rc => rc.Content)
        //    .WithMany(c => c.RentalContents)
        //    .HasForeignKey(rc => rc.ContentId);

        //builder.Entity<RentalContentModel>()
        //    .HasOne(rc => rc.Rental)
        //    .WithMany(r => r.RentalContents)
        //    .HasForeignKey(rc => rc.RentalId);
    }

    /// <summary>
    /// Configures one-to-many relationships
    /// </summary>
    /// <param name="builder"></param>
    public static void ConfigureOneToManyRelationships(ModelBuilder builder)
    {
        builder.Entity<AccountInfoModel>()
            .HasOne(a => a.Country)
            .WithMany()
            .HasForeignKey(a => a.CountryId);

        builder.Entity<AssetModel>()
            .HasOne(a => a.Language)
            .WithMany(l => l.Assets)
            .HasForeignKey(a => a.LanguageId);

        builder.Entity<ContentModel>()
            .HasOne(a => a.Language)
            .WithMany(l => l.Contents)
            .HasForeignKey(a => a.LanguageId);

        builder.Entity<MuxPlaybackIdModel>()
            .HasOne(plId => plId.MuxAsset)
            .WithMany(ma => ma.PlaybackIds)
            .HasForeignKey(plId => plId.MuxAssetId);

        builder.Entity<MuxAssetTrackModel>()
            .HasOne(tr => tr.MuxAsset)
            .WithMany(ma => ma.AssetTracks)
            .HasForeignKey(tr => tr.MuxAssetId);

        //builder.Entity<ContentModel>()
        //    .HasOne(c => c.PrimaryMedia)
        //    .WithMany()
        //    .HasForeignKey(c => c.PrimaryMediaId);

        //builder.Entity<ContentModel>()
        //    .HasOne(c => c.PreviewMedia)
        //    .WithMany()
        //    .HasForeignKey(c => c.PreviewMediaId);

        //builder.Entity<ContentModel>()
        //    .HasOne(c => c.BannerWebsite)
        //    .WithMany()
        //    .HasForeignKey(c => c.BannerWebsiteId);

        //builder.Entity<ContentModel>()
        //    .HasOne(c => c.BannerMobile)
        //    .WithMany()
        //    .HasForeignKey(c => c.BannerMobileId);

        //builder.Entity<ContentModel>()
        //    .HasOne(c => c.BannerTvApps)
        //    .WithMany()
        //    .HasForeignKey(c => c.BannerTvAppsId);

        //builder.Entity<ContentModel>()
        //    .HasOne(c => c.PosterWeb)
        //    .WithMany()
        //    .HasForeignKey(c => c.PosterWebId);

        //builder.Entity<ContentModel>()
        //    .HasOne(c => c.PosterMobile)
        //    .WithMany()
        //    .HasForeignKey(c => c.PosterMobileId);

        //builder.Entity<ContentModel>()
        //    .HasOne(c => c.PosterTvApps)
        //    .WithMany()
        //    .HasForeignKey(c => c.PosterTvAppsId);

        builder.Entity<SeasonModel>()
            .HasOne(c => c.Series)
            .WithMany(c => c.Seasons)
            .HasForeignKey(c => c.SeriesId);

        builder.Entity<SubtitleModel>()
            .HasOne(c => c.Content)
            .WithMany(c => c.Subtitles)
            .HasForeignKey(c => c.ContentId);

        //builder.Entity<TransactionModel>()
        //    .HasOne(t => t.Content)
        //    .WithMany(c => c.Transactions)
        //    .HasForeignKey(t => t.ContentId);
    }


    /// <summary>
    /// Configures one-to-one relationships
    /// </summary>
    /// <param name="builder"></param>
    public static void ConfigureOneToOneRelationships(ModelBuilder builder)
    {
        builder.Entity<ContentModel>()
            .HasOne(c => c.PrimaryMedia)
            .WithOne()
            .HasForeignKey<ContentModel>(c => c.PrimaryMediaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ContentModel>()
            .HasOne(c => c.PreviewMedia)
            .WithOne()
            .HasForeignKey<ContentModel>(c => c.PreviewMediaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ContentModel>()
            .HasOne(c => c.BannerWebsite)
            .WithOne()
            .HasForeignKey<ContentModel>(c => c.BannerWebsiteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ContentModel>()
            .HasOne(c => c.BannerMobile)
            .WithOne()
            .HasForeignKey<ContentModel>(c => c.BannerMobileId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ContentModel>()
            .HasOne(c => c.BannerTvApps)
            .WithOne()
            .HasForeignKey<ContentModel>(c => c.BannerTvAppsId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ContentModel>()
            .HasOne(c => c.PosterWeb)
            .WithOne()
            .HasForeignKey<ContentModel>(c => c.PosterWebId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ContentModel>()
            .HasOne(c => c.PosterMobile)
            .WithOne()
            .HasForeignKey<ContentModel>(c => c.PosterMobileId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ContentModel>()
            .HasOne(c => c.PosterTvApps)
            .WithOne()
            .HasForeignKey<ContentModel>(c => c.PosterTvAppsId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ContentModel>()
            .HasOne(c => c.PosterTvApps)
            .WithOne()
            .HasForeignKey<ContentModel>(c => c.PosterTvAppsId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<AssetModel>()
            .HasOne(a => a.Image)
            .WithOne(i => i.Asset)
            .HasForeignKey<ImageModel>(i => i.AssetId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<AssetModel>()
            .HasOne(a => a.Document)
            .WithOne(d => d.Asset)
            .HasForeignKey<DocumentModel>(a => a.AssetId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<CountryModel>()
            .HasOne(c => c.FlagAsset)
            .WithOne()
            .HasForeignKey<CountryModel>(c => c.FlagAssetId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<CategoryModel>()
            .HasOne(c => c.Image)
            .WithOne()
            .HasForeignKey<CategoryModel>(c => c.ImageAssetId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<CollectionModel>()
            .HasOne(c => c.Image)
            .WithOne()
            .HasForeignKey<CollectionModel>(c => c.ImageAssetId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<SeasonModel>()
            .HasOne(c => c.Image)
            .WithOne()
            .HasForeignKey<SeasonModel>(c => c.ImageAssetId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<SeriesModel>()
            .HasOne(c => c.Content)
            .WithOne()
            .HasForeignKey<SeriesModel>(c => c.ContentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<AssetModel>()
            .HasOne(c => c.MuxAsset)
            .WithOne()
            .HasForeignKey<AssetModel>(c => c.MuxAssetId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    /// <summary>
    /// Configures the precise SQL Server data type for decimal properties
    /// </summary>
    /// <param name="builder"></param>
    public static void ConfigurePrecisionForDecimal(ModelBuilder builder)
    {
        builder.Entity<Applicant>()
            .Property(a => a.Weight)
            .HasColumnType("decimal(10, 2)");

        builder.Entity<Applicant>()
            .Property(a => a.Height)
            .HasColumnType("decimal(10, 2)");

        //builder.Entity<RentalModel>()
        //    .Property(r => r.Price)
        //    .HasColumnType("decimal(10, 2)");

        //builder.Entity<TransactionModel>()
        //    .Property(t => t.Amount)
        //    .HasColumnType("decimal(10, 2)");

    }

    #endregion Public Methods
}