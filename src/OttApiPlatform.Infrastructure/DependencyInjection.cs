using System.Configuration;
using Microsoft.Extensions.Logging;
using OttApiPlatform.Application.Common.Contracts;
using OttApiPlatform.Application.Common.Contracts.Mux;
using OttApiPlatform.Application.Common.Contracts.Reports;
using OttApiPlatform.Infrastructure.Identity.Stores;
using OttApiPlatform.Infrastructure.Services;
using OttApiPlatform.Infrastructure.Services.Mux;
using OttApiPlatform.Infrastructure.Services.Security;

namespace OttApiPlatform.Infrastructure;

public static class DependencyInjection
{
    #region Public Methods

    /// <summary>
    /// Configures the infrastructure services for the application.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="configuration">
    /// The <see cref="IConfiguration"/> instance representing the appsettings.json file.
    /// </param>
    /// <param name="environment">
    /// The <see cref="IHostEnvironment"/> instance representing the current hosting environment.
    /// </param>
    /// <returns>The <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        // Add DbContext.
        services.AddDbContext<ApplicationDbContext>();

        // Use ONLY for logging sql queries
        //services.AddDbContext<ApplicationDbContext>(options =>
        //    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
        //        .EnableSensitiveDataLogging()
        //        .LogTo(Console.WriteLine, LogLevel.Information));

        // Add Hangfire.
        services.AddHangfire(globalConfiguration => globalConfiguration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true
            }));

        services.AddHangfireServer();

        // Add scoped DbContext.
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

        // Add Identity.
        services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddErrorDescriber<LocalizedIdentityErrorDescriber>()
            .AddDefaultTokenProviders()
            .AddPasswordValidator<CustomPasswordValidator<ApplicationUser>>()
            .AddUserManager<ApplicationUserManager>()
            .AddRoleManager<ApplicationRoleManager>();
            //.AddUserStore<CustomUserStore>()
            //.AddRoleStore<CustomRoleStore>();

        // Replace default validators with multi-tenant validators.
        services.Replace(ServiceDescriptor.Scoped<IUserValidator<ApplicationUser>, MultiTenantUserValidator>());
        services.Replace(ServiceDescriptor.Scoped<IRoleValidator<ApplicationRole>, MultiTenantRoleValidator>());

        // TODO: Uncomment and configure the password hashing options if needed.
        //services.Configure<PasswordHasherOptions>(options =>
        //{
        // options.IterationCount = 10000;
        //});

        // TODO: Uncomment and configure the password hashing options if using BCryptPasswordHasher.
        //services.AddScoped<IPasswordHasher<ApplicationUser>, BCryptPasswordHasher<ApplicationUser>>();
        //services.Configure<BCryptPasswordHasherOptions>(options =>
        //{
        // options.WorkFactor = 10;
        // options.EnhancedEntropy = false;
        //});

        // X-CSRF-Token.
        services.AddAntiforgery(options =>
        {
            options.HeaderName = "X-XSRF-Token";
            options.SuppressXFrameOptionsHeader = false;
        });

        // Add HttpContextAccessor.
        services.AddHttpContextAccessor();

        // Add Cache Services.
        services.AddMemoryCache();
        services.AddSingleton<ICacheService, MemoryCacheService>();

        // Add Application settings options.
        services.AddAppSettings(configuration);

        // Add identity managers.
        services.AddScoped<UserManager<ApplicationUser>, ApplicationUserManager>();
        services.AddScoped<RoleManager<ApplicationRole>, ApplicationRoleManager>();

        // Add singleton services.
        services.AddSingleton<UtcTimeService>();

        // Add scoped services.

        // Add application services.
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IApplicantsReaderService, ApplicantsReaderService>();
        services.AddScoped<IAppSettingsReaderService, AppSettingsReaderService>();

        // Add HttpClient for services.
        services.AddHttpClient<IMuxApiService, MuxApiService>();

        // Add infrastructure services.
        services.AddScoped<IUtcTimeService>(provider => new FrozenUtcTimeService(provider.GetRequiredService<UtcTimeService>()));
        services.AddScoped<IStorageProvider, StorageProvider>();
        services.AddScoped<IStorageFactory, StorageFactory>();
        services.AddScoped<ITenantResolver, TenantResolver>();
        services.AddScoped<IConfigReaderService, ConfigReaderService>();
        services.AddScoped<IPermissionScanner, PermissionScanner>();
        services.AddScoped<IdentityErrorDescriber, LocalizedIdentityErrorDescriber>();
        services.AddScoped<IFileStorageService, AzureBlobStorageService>();
        services.AddScoped<IFileStorageService, OnPremisesStorageService>();
        services.AddScoped<ITokenGeneratorService, TokenGeneratorService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IAppSeederService, AppSeederService>();
        services.AddScoped<IBackgroundReportingService, BackgroundReportingService>();
        services.AddScoped<IOnDemandReportingService, OnDemandReportingService>();
        services.AddScoped<IHtmlReportBuilderService, HtmlReportBuilderService>();
        services.AddScoped<ILicenseService, LicenseService>();
        services.AddScoped<ICryptoService, EncryptionService>();
        services.AddScoped<IMuxConfigurationFactory, MuxConfigurationFactory>();
        services.AddScoped<IMuxApiService, MuxApiService>();
        services.AddScoped<IMuxWebHookHandler, MuxWebHookHandler>();
        services.AddScoped<IAzureBlobStorageService, AzureBlobStorageService>();


        //services.AddScoped<ICategoryUseCase, CategoryUseCase>();
        //services.AddScoped<IAuthorUseCase, AuthorUseCase>();

        //IRepositoryManager 
        //services.AddScoped<IRepositoryManager, RepositoryManager>();

        return services;
    }

    #endregion Public Methods
}