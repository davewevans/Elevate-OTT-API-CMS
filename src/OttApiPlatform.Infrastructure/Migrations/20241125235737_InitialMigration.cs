using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OttApiPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.EnsureSchema(
            //    name: "Settings");

            //migrationBuilder.CreateTable(
            //    name: "Applicants",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Ssn = table.Column<int>(type: "int", nullable: false),
            //        FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        Height = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
            //        Weight = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
            //        CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Applicants", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AspNetPermissions",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        TenantVisibility = table.Column<bool>(type: "bit", nullable: false),
            //        HostVisibility = table.Column<bool>(type: "bit", nullable: false),
            //        IsCustomPermission = table.Column<bool>(type: "bit", nullable: false),
            //        ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AspNetPermissions", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_AspNetPermissions_AspNetPermissions_ParentId",
            //            column: x => x.ParentId,
            //            principalTable: "AspNetPermissions",
            //            principalColumn: "Id");
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AspNetRoles",
            //    columns: table => new
            //    {
            //        Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        IsStatic = table.Column<bool>(type: "bit", nullable: false),
            //        IsDefault = table.Column<bool>(type: "bit", nullable: false),
            //        CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
            //        NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
            //        ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AspNetRoles", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "ContentSettings",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        AllowUsersToCreatePlaylist = table.Column<bool>(type: "bit", nullable: false),
            //        RequireUsersSignInToAccessFreeContent = table.Column<bool>(type: "bit", nullable: false),
            //        AllowMaturityRatings = table.Column<bool>(type: "bit", nullable: false),
            //        AllowRatings = table.Column<bool>(type: "bit", nullable: false),
            //        AllowComments = table.Column<bool>(type: "bit", nullable: false),
            //        AllowReviews = table.Column<bool>(type: "bit", nullable: false),
            //        AllowDownload = table.Column<bool>(type: "bit", nullable: false),
            //        AllowSharing = table.Column<bool>(type: "bit", nullable: false),
            //        AllowEmbedding = table.Column<bool>(type: "bit", nullable: false),
            //        AllowLikes = table.Column<bool>(type: "bit", nullable: false),
            //        LikesCount = table.Column<bool>(type: "bit", nullable: false),
            //        AllowDislikes = table.Column<bool>(type: "bit", nullable: false),
            //        DislikesCount = table.Column<bool>(type: "bit", nullable: false),
            //        AllowFavorites = table.Column<bool>(type: "bit", nullable: false),
            //        IsGeoRestricted = table.Column<bool>(type: "bit", nullable: false),
            //        CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ContentSettings", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "FileStorageSettings",
            //    schema: "Settings",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        StorageType = table.Column<int>(type: "int", nullable: false),
            //        TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_FileStorageSettings", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Languages",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        RegionCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Languages", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "LockoutSettings",
            //    schema: "Settings",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        AllowedForNewUsers = table.Column<bool>(type: "bit", nullable: false),
            //        MaxFailedAccessAttempts = table.Column<int>(type: "int", nullable: false),
            //        DefaultLockoutTimeSpan = table.Column<int>(type: "int", nullable: false),
            //        TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_LockoutSettings", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "PasswordSettings",
            //    schema: "Settings",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        RequiredLength = table.Column<int>(type: "int", nullable: false),
            //        RequiredUniqueChars = table.Column<int>(type: "int", nullable: false),
            //        RequireNonAlphanumeric = table.Column<bool>(type: "bit", nullable: false),
            //        RequireLowercase = table.Column<bool>(type: "bit", nullable: false),
            //        RequireUppercase = table.Column<bool>(type: "bit", nullable: false),
            //        RequireDigit = table.Column<bool>(type: "bit", nullable: false),
            //        TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_PasswordSettings", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "People",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            //        Bio = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
            //        ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        SeoTitle = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
            //        SeoDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
            //        Slug = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_People", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Reports",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        QueryString = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        FileUri = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ContentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Status = table.Column<int>(type: "int", nullable: false),
            //        CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Reports", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "SignInSettings",
            //    schema: "Settings",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        RequireConfirmedAccount = table.Column<bool>(type: "bit", nullable: false),
            //        TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_SignInSettings", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "StorageLocations",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_StorageLocations", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "SwimLanes",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
            //        Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
            //        ContentType = table.Column<int>(type: "int", nullable: false),
            //        SwimLaneType = table.Column<int>(type: "int", nullable: false),
            //        SwimLaneCriteria = table.Column<int>(type: "int", nullable: false),
            //        ContentLimit = table.Column<int>(type: "int", nullable: false),
            //        CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_SwimLanes", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Tags",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            //        CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Tags", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Tenants",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Name = table.Column<string>(type: "nvarchar(450)", nullable: true),
            //        CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Tenants", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "TokenSettings",
            //    schema: "Settings",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        AccessTokenUoT = table.Column<int>(type: "int", nullable: false),
            //        AccessTokenTimeSpan = table.Column<double>(type: "float", nullable: true),
            //        RefreshTokenUoT = table.Column<int>(type: "int", nullable: false),
            //        RefreshTokenTimeSpan = table.Column<double>(type: "float", nullable: true),
            //        TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_TokenSettings", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "UserSettings",
            //    schema: "Settings",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        AllowedUserNameCharacters = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        NewUsersActiveByDefault = table.Column<bool>(type: "bit", nullable: false),
            //        TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_UserSettings", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "References",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ApplicantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_References", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_References_Applicants_ApplicantId",
            //            column: x => x.ApplicantId,
            //            principalTable: "Applicants",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AspNetRoleClaims",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
            //            column: x => x.RoleId,
            //            principalTable: "AspNetRoles",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Assets",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Type = table.Column<int>(type: "int", nullable: false),
            //        Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        Passthrough = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ClosedCaptions = table.Column<bool>(type: "bit", nullable: false),
            //        PublicPlaybackId = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        SignedPlaybackId = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        IsTestAsset = table.Column<bool>(type: "bit", nullable: false),
            //        CreationStatus = table.Column<int>(type: "int", nullable: false),
            //        LanguageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Assets", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Assets_Languages_LanguageId",
            //            column: x => x.LanguageId,
            //            principalTable: "Languages",
            //            principalColumn: "Id");
            //    });

            //migrationBuilder.CreateTable(
            //    name: "LiveStreams",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Sku = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        StreamUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        StreamKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        StreamType = table.Column<int>(type: "int", nullable: false),
            //        RtmpUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        RtmpsUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Status = table.Column<int>(type: "int", nullable: false),
            //        VideoResolutionType = table.Column<int>(type: "int", nullable: false),
            //        LatencyMode = table.Column<int>(type: "int", nullable: false),
            //        ReconnectWindow = table.Column<float>(type: "real", nullable: false),
            //        MuxLiveStreamId = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        IsMuxLiveStream = table.Column<bool>(type: "bit", nullable: false),
            //        MaxContinuousDuration = table.Column<int>(type: "int", nullable: false),
            //        LiveStreamCreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        LiveStreamEndedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        PreRegistrationText = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
            //        Rating = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ButtonPurchaseText = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
            //        TestLiveStreamPasscode = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
            //        PersonModelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_LiveStreams", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_LiveStreams_People_PersonModelId",
            //            column: x => x.PersonModelId,
            //            principalTable: "People",
            //            principalColumn: "Id");
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AspNetUsers",
            //    columns: table => new
            //    {
            //        Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Surname = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        AvatarUri = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        IsSuspended = table.Column<bool>(type: "bit", nullable: false),
            //        IsStatic = table.Column<bool>(type: "bit", nullable: false),
            //        IsSuperAdmin = table.Column<bool>(type: "bit", nullable: false),
            //        RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        RefreshTokenTimeSpan = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
            //        NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
            //        Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
            //        NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
            //        EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
            //        PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
            //        TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
            //        LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //        LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
            //        AccessFailedCount = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AspNetUsers", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_AspNetUsers_Tenants_TenantId",
            //            column: x => x.TenantId,
            //            principalTable: "Tenants",
            //            principalColumn: "Id");
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AssetStorage",
            //    columns: table => new
            //    {
            //        AssetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        StorageLocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Order = table.Column<int>(type: "int", nullable: false),
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AssetStorage", x => new { x.AssetId, x.StorageLocationId });
            //        table.ForeignKey(
            //            name: "FK_AssetStorage_Assets_AssetId",
            //            column: x => x.AssetId,
            //            principalTable: "Assets",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_AssetStorage_StorageLocations_StorageLocationId",
            //            column: x => x.StorageLocationId,
            //            principalTable: "StorageLocations",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Audios",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Bitrate = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Duration = table.Column<int>(type: "int", nullable: false),
            //        FileSize = table.Column<double>(type: "float", nullable: false),
            //        Encoded = table.Column<bool>(type: "bit", nullable: false),
            //        AssetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        PersonModelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Audios", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Audios_Assets_AssetId",
            //            column: x => x.AssetId,
            //            principalTable: "Assets",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_Audios_People_PersonModelId",
            //            column: x => x.PersonModelId,
            //            principalTable: "People",
            //            principalColumn: "Id");
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Categories",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
            //        Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
            //        Order = table.Column<int>(type: "int", nullable: false),
            //        SeoTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        SeoDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
            //        Slug = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        ImageAssetId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Categories", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Categories_Assets_ImageAssetId",
            //            column: x => x.ImageAssetId,
            //            principalTable: "Assets",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Countries",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ISO2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ISO3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        NumericCode = table.Column<int>(type: "int", nullable: false),
            //        DialCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Region = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        SubRegion = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        IsActive = table.Column<bool>(type: "bit", nullable: false),
            //        FlagAssetId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Countries", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Countries_Assets_FlagAssetId",
            //            column: x => x.FlagAssetId,
            //            principalTable: "Assets",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Documents",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ContentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        FileSize = table.Column<double>(type: "float", nullable: false),
            //        AssetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Documents", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Documents_Assets_AssetId",
            //            column: x => x.AssetId,
            //            principalTable: "Assets",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Images",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Type = table.Column<int>(type: "int", nullable: false),
            //        Width = table.Column<int>(type: "int", nullable: false),
            //        Height = table.Column<int>(type: "int", nullable: false),
            //        ImageSize = table.Column<double>(type: "float", nullable: false),
            //        AssetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Images", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Images_Assets_AssetId",
            //            column: x => x.AssetId,
            //            principalTable: "Assets",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Playlists",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
            //        Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
            //        SeoTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        SeoDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
            //        Slug = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        Type = table.Column<int>(type: "int", nullable: false),
            //        Status = table.Column<int>(type: "int", nullable: false),
            //        Permalink = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Language = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Rating = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ImageAssetId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        BannerWebsiteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        BannerMobileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        BannerTvAppsId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        PosterWebId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        PosterMobileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        PosterTvAppsId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Playlists", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Playlists_Assets_BannerMobileId",
            //            column: x => x.BannerMobileId,
            //            principalTable: "Assets",
            //            principalColumn: "Id");
            //        table.ForeignKey(
            //            name: "FK_Playlists_Assets_BannerTvAppsId",
            //            column: x => x.BannerTvAppsId,
            //            principalTable: "Assets",
            //            principalColumn: "Id");
            //        table.ForeignKey(
            //            name: "FK_Playlists_Assets_BannerWebsiteId",
            //            column: x => x.BannerWebsiteId,
            //            principalTable: "Assets",
            //            principalColumn: "Id");
            //        table.ForeignKey(
            //            name: "FK_Playlists_Assets_ImageAssetId",
            //            column: x => x.ImageAssetId,
            //            principalTable: "Assets",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Playlists_Assets_PosterMobileId",
            //            column: x => x.PosterMobileId,
            //            principalTable: "Assets",
            //            principalColumn: "Id");
            //        table.ForeignKey(
            //            name: "FK_Playlists_Assets_PosterTvAppsId",
            //            column: x => x.PosterTvAppsId,
            //            principalTable: "Assets",
            //            principalColumn: "Id");
            //        table.ForeignKey(
            //            name: "FK_Playlists_Assets_PosterWebId",
            //            column: x => x.PosterWebId,
            //            principalTable: "Assets",
            //            principalColumn: "Id");
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Videos",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Resolution = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        VideoCodec = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        VideoBitrate = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        AudioBitrate = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Bitrate = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        FrameRate = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Duration = table.Column<TimeSpan>(type: "time", nullable: false),
            //        FileSize = table.Column<double>(type: "float", nullable: false),
            //        Encoded = table.Column<bool>(type: "bit", nullable: false),
            //        AssetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        PersonModelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Videos", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Videos_Assets_AssetId",
            //            column: x => x.AssetId,
            //            principalTable: "Assets",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_Videos_People_PersonModelId",
            //            column: x => x.PersonModelId,
            //            principalTable: "People",
            //            principalColumn: "Id");
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AspNetUserAttachments",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        FileUri = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AspNetUserAttachments", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_AspNetUserAttachments_AspNetUsers_UserId",
            //            column: x => x.UserId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AspNetUserClaims",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        IsExcluded = table.Column<bool>(type: "bit", nullable: false),
            //        UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_AspNetUserClaims_AspNetUsers_UserId",
            //            column: x => x.UserId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AspNetUserLogins",
            //    columns: table => new
            //    {
            //        LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
            //        table.ForeignKey(
            //            name: "FK_AspNetUserLogins_AspNetUsers_UserId",
            //            column: x => x.UserId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AspNetUserRoles",
            //    columns: table => new
            //    {
            //        UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
            //        table.ForeignKey(
            //            name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
            //            column: x => x.RoleId,
            //            principalTable: "AspNetRoles",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_AspNetUserRoles_AspNetUsers_UserId",
            //            column: x => x.UserId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AspNetUserTokens",
            //    columns: table => new
            //    {
            //        UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
            //        table.ForeignKey(
            //            name: "FK_AspNetUserTokens_AspNetUsers_UserId",
            //            column: x => x.UserId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AccountInfo",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        ChannelName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        LicenseKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        SubDomain = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CustomDomain = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        StorageFileNamePrefix = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AccountInfo", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_AccountInfo_Countries_CountryId",
            //            column: x => x.CountryId,
            //            principalTable: "Countries",
            //            principalColumn: "Id");
            //    });

            //migrationBuilder.CreateTable(
            //    name: "ContentSettingsCountries",
            //    columns: table => new
            //    {
            //        ContentSettingsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ContentSettingsCountries", x => new { x.ContentSettingsId, x.CountryId });
            //        table.ForeignKey(
            //            name: "FK_ContentSettingsCountries_ContentSettings_ContentSettingsId",
            //            column: x => x.ContentSettingsId,
            //            principalTable: "ContentSettings",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_ContentSettingsCountries_Countries_CountryId",
            //            column: x => x.CountryId,
            //            principalTable: "Countries",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "PlaylistAssets",
            //    columns: table => new
            //    {
            //        AssetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        PlaylistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Order = table.Column<int>(type: "int", nullable: false),
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_PlaylistAssets", x => new { x.AssetId, x.PlaylistId });
            //        table.ForeignKey(
            //            name: "FK_PlaylistAssets_Assets_AssetId",
            //            column: x => x.AssetId,
            //            principalTable: "Assets",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_PlaylistAssets_Playlists_PlaylistId",
            //            column: x => x.PlaylistId,
            //            principalTable: "Playlists",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Asset",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
            //        ShortDescription = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        FullDescription = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
            //        SeoTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        SeoDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
            //        Slug = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        Type = table.Column<int>(type: "int", nullable: false),
            //        Status = table.Column<int>(type: "int", nullable: false),
            //        AllowDownload = table.Column<bool>(type: "bit", nullable: false),
            //        Language = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        PrimaryMediaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        PreviewMediaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        BannerWebsiteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        BannerMobileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        BannerTvAppsId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        PosterWebId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        PosterMobileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        PosterTvAppsId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        SeriesId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Asset", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Asset_Assets_BannerMobileId",
            //            column: x => x.BannerMobileId,
            //            principalTable: "Assets",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Asset_Assets_BannerTvAppsId",
            //            column: x => x.BannerTvAppsId,
            //            principalTable: "Assets",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Asset_Assets_BannerWebsiteId",
            //            column: x => x.BannerWebsiteId,
            //            principalTable: "Assets",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Asset_Assets_PosterMobileId",
            //            column: x => x.PosterMobileId,
            //            principalTable: "Assets",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Asset_Assets_PosterTvAppsId",
            //            column: x => x.PosterTvAppsId,
            //            principalTable: "Assets",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Asset_Assets_PosterWebId",
            //            column: x => x.PosterWebId,
            //            principalTable: "Assets",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Asset_Assets_PreviewMediaId",
            //            column: x => x.PreviewMediaId,
            //            principalTable: "Assets",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Asset_Assets_PrimaryMediaId",
            //            column: x => x.PrimaryMediaId,
            //            principalTable: "Assets",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "ContentCategories",
            //    columns: table => new
            //    {
            //        ContentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Order = table.Column<int>(type: "int", nullable: false),
            //        SwimLaneModelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ContentCategories", x => new { x.ContentId, x.CategoryId });
            //        table.ForeignKey(
            //            name: "FK_ContentCategories_Asset_ContentId",
            //            column: x => x.ContentId,
            //            principalTable: "Asset",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_ContentCategories_Categories_CategoryId",
            //            column: x => x.CategoryId,
            //            principalTable: "Categories",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_ContentCategories_SwimLanes_SwimLaneModelId",
            //            column: x => x.SwimLaneModelId,
            //            principalTable: "SwimLanes",
            //            principalColumn: "Id");
            //    });

            //migrationBuilder.CreateTable(
            //    name: "ContentPeople",
            //    columns: table => new
            //    {
            //        ContentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Order = table.Column<int>(type: "int", nullable: false),
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ContentPeople", x => new { x.ContentId, x.PersonId });
            //        table.ForeignKey(
            //            name: "FK_ContentPeople_Asset_ContentId",
            //            column: x => x.ContentId,
            //            principalTable: "Asset",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_ContentPeople_People_PersonId",
            //            column: x => x.PersonId,
            //            principalTable: "People",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "ContentTags",
            //    columns: table => new
            //    {
            //        ContentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        TagId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Order = table.Column<int>(type: "int", nullable: false),
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ContentTags", x => new { x.ContentId, x.TagId });
            //        table.ForeignKey(
            //            name: "FK_ContentTags_Asset_ContentId",
            //            column: x => x.ContentId,
            //            principalTable: "Asset",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_ContentTags_Tags_TagId",
            //            column: x => x.TagId,
            //            principalTable: "Tags",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Series",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        ContentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Series", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Series_Asset_ContentId",
            //            column: x => x.ContentId,
            //            principalTable: "Asset",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Subtitles",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        ClosedCaption = table.Column<bool>(type: "bit", nullable: false),
            //        TrackId = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Passthrough = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        LanguageCodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        ContentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Subtitles", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Subtitles_Asset_ContentId",
            //            column: x => x.ContentId,
            //            principalTable: "Asset",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "SwimLaneContent",
            //    columns: table => new
            //    {
            //        ContentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        SwimLaneId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Order = table.Column<int>(type: "int", nullable: false),
            //        SwimLaneModelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_SwimLaneContent", x => new { x.ContentId, x.SwimLaneId });
            //        table.ForeignKey(
            //            name: "FK_SwimLaneContent_Asset_ContentId",
            //            column: x => x.ContentId,
            //            principalTable: "Asset",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_SwimLaneContent_Categories_SwimLaneId",
            //            column: x => x.SwimLaneId,
            //            principalTable: "Categories",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_SwimLaneContent_SwimLanes_SwimLaneModelId",
            //            column: x => x.SwimLaneModelId,
            //            principalTable: "SwimLanes",
            //            principalColumn: "Id");
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Seasons",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        SeasonNumber = table.Column<int>(type: "int", nullable: false),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        SeriesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        ImageAssetId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Seasons", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Seasons_Assets_ImageAssetId",
            //            column: x => x.ImageAssetId,
            //            principalTable: "Assets",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Seasons_Series_SeriesId",
            //            column: x => x.SeriesId,
            //            principalTable: "Series",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "SeriesAssets",
            //    columns: table => new
            //    {
            //        AssetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        SeriesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Order = table.Column<int>(type: "int", nullable: false),
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_SeriesAssets", x => new { x.AssetId, x.SeriesId });
            //        table.ForeignKey(
            //            name: "FK_SeriesAssets_Assets_AssetId",
            //            column: x => x.AssetId,
            //            principalTable: "Assets",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_SeriesAssets_Series_SeriesId",
            //            column: x => x.SeriesId,
            //            principalTable: "Series",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "SeasonAssets",
            //    columns: table => new
            //    {
            //        AssetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        SeasonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        EpisodeNumber = table.Column<int>(type: "int", nullable: false),
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_SeasonAssets", x => new { x.AssetId, x.SeasonId });
            //        table.ForeignKey(
            //            name: "FK_SeasonAssets_Assets_AssetId",
            //            column: x => x.AssetId,
            //            principalTable: "Assets",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_SeasonAssets_Seasons_SeasonId",
            //            column: x => x.SeasonId,
            //            principalTable: "Seasons",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_AccountInfo_CountryId",
            //    table: "AccountInfo",
            //    column: "CountryId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_AspNetPermissions_ParentId",
            //    table: "AspNetPermissions",
            //    column: "ParentId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_AspNetRoleClaims_RoleId",
            //    table: "AspNetRoleClaims",
            //    column: "RoleId");

            //migrationBuilder.CreateIndex(
            //    name: "RoleNameIndex",
            //    table: "AspNetRoles",
            //    column: "NormalizedName");

            //migrationBuilder.CreateIndex(
            //    name: "IX_AspNetUserAttachments_UserId",
            //    table: "AspNetUserAttachments",
            //    column: "UserId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_AspNetUserClaims_UserId",
            //    table: "AspNetUserClaims",
            //    column: "UserId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_AspNetUserLogins_UserId",
            //    table: "AspNetUserLogins",
            //    column: "UserId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_AspNetUserRoles_RoleId",
            //    table: "AspNetUserRoles",
            //    column: "RoleId");

            //migrationBuilder.CreateIndex(
            //    name: "EmailIndex",
            //    table: "AspNetUsers",
            //    column: "NormalizedEmail");

            //migrationBuilder.CreateIndex(
            //    name: "IX_AspNetUsers_TenantId",
            //    table: "AspNetUsers",
            //    column: "TenantId");

            //migrationBuilder.CreateIndex(
            //    name: "UserNameIndex",
            //    table: "AspNetUsers",
            //    column: "NormalizedUserName");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Asset_BannerMobileId",
            //    table: "Asset",
            //    column: "BannerMobileId",
            //    unique: true,
            //    filter: "[BannerMobileId] IS NOT NULL");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Asset_BannerTvAppsId",
            //    table: "Asset",
            //    column: "BannerTvAppsId",
            //    unique: true,
            //    filter: "[BannerTvAppsId] IS NOT NULL");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Asset_BannerWebsiteId",
            //    table: "Asset",
            //    column: "BannerWebsiteId",
            //    unique: true,
            //    filter: "[BannerWebsiteId] IS NOT NULL");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Asset_PosterMobileId",
            //    table: "Asset",
            //    column: "PosterMobileId",
            //    unique: true,
            //    filter: "[PosterMobileId] IS NOT NULL");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Asset_PosterTvAppsId",
            //    table: "Asset",
            //    column: "PosterTvAppsId",
            //    unique: true,
            //    filter: "[PosterTvAppsId] IS NOT NULL");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Asset_PosterWebId",
            //    table: "Asset",
            //    column: "PosterWebId",
            //    unique: true,
            //    filter: "[PosterWebId] IS NOT NULL");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Asset_PreviewMediaId",
            //    table: "Asset",
            //    column: "PreviewMediaId",
            //    unique: true,
            //    filter: "[PreviewMediaId] IS NOT NULL");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Asset_PrimaryMediaId",
            //    table: "Asset",
            //    column: "PrimaryMediaId",
            //    unique: true,
            //    filter: "[PrimaryMediaId] IS NOT NULL");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Asset_SeriesId",
            //    table: "Asset",
            //    column: "SeriesId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Assets_LanguageId",
            //    table: "Assets",
            //    column: "LanguageId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_AssetStorage_Order",
            //    table: "AssetStorage",
            //    column: "Order",
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_AssetStorage_StorageLocationId",
            //    table: "AssetStorage",
            //    column: "StorageLocationId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Audios_AssetId",
            //    table: "Audios",
            //    column: "AssetId",
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Audios_PersonModelId",
            //    table: "Audios",
            //    column: "PersonModelId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Categories_ImageAssetId",
            //    table: "Categories",
            //    column: "ImageAssetId",
            //    unique: true,
            //    filter: "[ImageAssetId] IS NOT NULL");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Categories_Order",
            //    table: "Categories",
            //    column: "Order",
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_ContentCategories_CategoryId",
            //    table: "ContentCategories",
            //    column: "CategoryId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ContentCategories_Order",
            //    table: "ContentCategories",
            //    column: "Order",
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_ContentCategories_SwimLaneModelId",
            //    table: "ContentCategories",
            //    column: "SwimLaneModelId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ContentPeople_Order",
            //    table: "ContentPeople",
            //    column: "Order",
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_ContentPeople_PersonId",
            //    table: "ContentPeople",
            //    column: "PersonId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ContentSettingsCountries_CountryId",
            //    table: "ContentSettingsCountries",
            //    column: "CountryId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ContentTags_Order",
            //    table: "ContentTags",
            //    column: "Order",
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_ContentTags_TagId",
            //    table: "ContentTags",
            //    column: "TagId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Countries_FlagAssetId",
            //    table: "Countries",
            //    column: "FlagAssetId",
            //    unique: true,
            //    filter: "[FlagAssetId] IS NOT NULL");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Documents_AssetId",
            //    table: "Documents",
            //    column: "AssetId",
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Images_AssetId",
            //    table: "Images",
            //    column: "AssetId",
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_LiveStreams_PersonModelId",
            //    table: "LiveStreams",
            //    column: "PersonModelId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_PlaylistAssets_Order",
            //    table: "PlaylistAssets",
            //    column: "Order",
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_PlaylistAssets_PlaylistId",
            //    table: "PlaylistAssets",
            //    column: "PlaylistId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Playlists_BannerMobileId",
            //    table: "Playlists",
            //    column: "BannerMobileId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Playlists_BannerTvAppsId",
            //    table: "Playlists",
            //    column: "BannerTvAppsId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Playlists_BannerWebsiteId",
            //    table: "Playlists",
            //    column: "BannerWebsiteId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Playlists_ImageAssetId",
            //    table: "Playlists",
            //    column: "ImageAssetId",
            //    unique: true,
            //    filter: "[ImageAssetId] IS NOT NULL");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Playlists_PosterMobileId",
            //    table: "Playlists",
            //    column: "PosterMobileId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Playlists_PosterTvAppsId",
            //    table: "Playlists",
            //    column: "PosterTvAppsId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Playlists_PosterWebId",
            //    table: "Playlists",
            //    column: "PosterWebId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_References_ApplicantId",
            //    table: "References",
            //    column: "ApplicantId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_SeasonAssets_EpisodeNumber",
            //    table: "SeasonAssets",
            //    column: "EpisodeNumber",
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_SeasonAssets_SeasonId",
            //    table: "SeasonAssets",
            //    column: "SeasonId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Seasons_ImageAssetId",
            //    table: "Seasons",
            //    column: "ImageAssetId",
            //    unique: true,
            //    filter: "[ImageAssetId] IS NOT NULL");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Seasons_SeasonNumber",
            //    table: "Seasons",
            //    column: "SeasonNumber",
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Seasons_SeriesId",
            //    table: "Seasons",
            //    column: "SeriesId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Series_ContentId",
            //    table: "Series",
            //    column: "ContentId",
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_SeriesAssets_Order",
            //    table: "SeriesAssets",
            //    column: "Order",
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_SeriesAssets_SeriesId",
            //    table: "SeriesAssets",
            //    column: "SeriesId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Subtitles_ContentId",
            //    table: "Subtitles",
            //    column: "ContentId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_SwimLaneContent_Order",
            //    table: "SwimLaneContent",
            //    column: "Order",
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_SwimLaneContent_SwimLaneId",
            //    table: "SwimLaneContent",
            //    column: "SwimLaneId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_SwimLaneContent_SwimLaneModelId",
            //    table: "SwimLaneContent",
            //    column: "SwimLaneModelId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Tenants_Name",
            //    table: "Tenants",
            //    column: "Name",
            //    unique: true,
            //    filter: "[Name] IS NOT NULL");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Videos_AssetId",
            //    table: "Videos",
            //    column: "AssetId",
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Videos_PersonModelId",
            //    table: "Videos",
            //    column: "PersonModelId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Asset_Series_SeriesId",
            //    table: "Asset",
            //    column: "SeriesId",
            //    principalTable: "Series",
            //    principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Asset_Assets_BannerMobileId",
                table: "Asset");

            migrationBuilder.DropForeignKey(
                name: "FK_Asset_Assets_BannerTvAppsId",
                table: "Asset");

            migrationBuilder.DropForeignKey(
                name: "FK_Asset_Assets_BannerWebsiteId",
                table: "Asset");

            migrationBuilder.DropForeignKey(
                name: "FK_Asset_Assets_PosterMobileId",
                table: "Asset");

            migrationBuilder.DropForeignKey(
                name: "FK_Asset_Assets_PosterTvAppsId",
                table: "Asset");

            migrationBuilder.DropForeignKey(
                name: "FK_Asset_Assets_PosterWebId",
                table: "Asset");

            migrationBuilder.DropForeignKey(
                name: "FK_Asset_Assets_PreviewMediaId",
                table: "Asset");

            migrationBuilder.DropForeignKey(
                name: "FK_Asset_Assets_PrimaryMediaId",
                table: "Asset");

            migrationBuilder.DropForeignKey(
                name: "FK_Asset_Series_SeriesId",
                table: "Asset");

            migrationBuilder.DropTable(
                name: "AccountInfo");

            migrationBuilder.DropTable(
                name: "AspNetPermissions");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserAttachments");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AssetStorage");

            migrationBuilder.DropTable(
                name: "Audios");

            migrationBuilder.DropTable(
                name: "ContentCategories");

            migrationBuilder.DropTable(
                name: "ContentPeople");

            migrationBuilder.DropTable(
                name: "ContentSettingsCountries");

            migrationBuilder.DropTable(
                name: "ContentTags");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "FileStorageSettings",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "LiveStreams");

            migrationBuilder.DropTable(
                name: "LockoutSettings",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "PasswordSettings",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "PlaylistAssets");

            migrationBuilder.DropTable(
                name: "References");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "SeasonAssets");

            migrationBuilder.DropTable(
                name: "SeriesAssets");

            migrationBuilder.DropTable(
                name: "SignInSettings",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "Subtitles");

            migrationBuilder.DropTable(
                name: "SwimLaneContent");

            migrationBuilder.DropTable(
                name: "TokenSettings",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "UserSettings",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "Videos");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "StorageLocations");

            migrationBuilder.DropTable(
                name: "ContentSettings");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Playlists");

            migrationBuilder.DropTable(
                name: "Applicants");

            migrationBuilder.DropTable(
                name: "Seasons");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "SwimLanes");

            migrationBuilder.DropTable(
                name: "People");

            migrationBuilder.DropTable(
                name: "Tenants");

            migrationBuilder.DropTable(
                name: "Assets");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "Series");

            migrationBuilder.DropTable(
                name: "Asset");
        }
    }
}
