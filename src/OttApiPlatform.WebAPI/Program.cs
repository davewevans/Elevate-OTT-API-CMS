// Create a new instance of the WebApplication builder.

var builder = WebApplication.CreateBuilder(args);
                            
// Add application services to the service collection.
builder.Services.AddApplication();

// Add infrastructure services to the service collection.
builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);

// Add health checks services to the service collection.
builder.Services.AddHealthChecks();

// Add localization services to the service collection.
builder.Services.AddAppLocalization();

// Add authentication services to the service collection.
builder.Services.AddAuth(builder.Configuration);

// Add controllers services to the service collection, and configure JSON options.
builder.Services.AddControllers(options =>
{
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

// Add validation services to the service collection, and configure validation options.
builder.Services.AddFluentValidationAutoValidation()
.AddFluentValidationClientsideAdapters()
.AddValidatorsFromAssemblyContaining<IApplicationDbContext>();

// Configure API behavior options.
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Configure form options.
builder.Services.Configure<FormOptions>(x =>
{
    x.ValueLengthLimit = 1073741824;
    x.MultipartBodyLengthLimit = 1073741824;
});

// Add Swagger API services to the service collection.
builder.Services.AddSwaggerApi();

// Add CORS services to the service collection.
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policyBuilder => policyBuilder.AllowAnyOrigin()
                                                                  .AllowAnyMethod()
                                                                  .AllowAnyHeader());
});

// Add SignalR services to the service collection.
builder.Services.AddSignalR();

// Add a singleton TimerManager instance to the service collection.
builder.Services.AddSingleton<TimerManager>();

// Add scoped services for HubNotificationService and SignalRContextProvider to the service collection.
builder.Services.AddScoped<IHubNotificationService, HubNotificationService>();
builder.Services.AddScoped<IAssetHubNotificationService, AssetHubNotificationService>();
builder.Services.AddScoped<ISignalRContextProvider, SignalRContextProvider>();

// Configure Identity options.
builder.Services.Configure<IdentityOptions>(options =>
{
    // Set user account options.
    options.User.AllowedUserNameCharacters = builder.Configuration.GetValue<string>($"{AppUserOptions.Section}:allowedUserNameCharacters") ?? "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;

    // Set password requirements.
    options.Password.RequiredLength = builder.Configuration.GetValue<int>($"{AppPasswordOptions.Section}:RequiredLength");
    options.Password.RequiredUniqueChars = builder.Configuration.GetValue<int>($"{AppPasswordOptions.Section}:RequiredUniqueChars");
    options.Password.RequireNonAlphanumeric = builder.Configuration.GetValue<bool>($"{AppPasswordOptions.Section}:RequireNonAlphanumeric");
    options.Password.RequireLowercase = builder.Configuration.GetValue<bool>($"{AppPasswordOptions.Section}:RequireLowercase");
    options.Password.RequireUppercase = builder.Configuration.GetValue<bool>($"{AppPasswordOptions.Section}:RequireUppercase");
    options.Password.RequireDigit = builder.Configuration.GetValue<bool>($"{AppPasswordOptions.Section}:RequireDigit");

    // Set lockout options.
    options.Lockout.AllowedForNewUsers = builder.Configuration.GetValue<bool>($"{AppLockoutOptions.Section}:AllowedForNewUsers");
    options.Lockout.MaxFailedAccessAttempts = builder.Configuration.GetValue<int>($"{AppLockoutOptions.Section}:MaxFailedAccessAttempts");
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(builder.Configuration.GetValue<double>($"{AppLockoutOptions.Section}:DefaultLockoutTimeSpan"));

    // Require confirmed accounts for sign in.
    options.SignIn.RequireConfirmedAccount = builder.Configuration.GetValue<bool>($"{AppUserOptions.Section}:RequireConfirmedAccount");
});

// Build the app.
var app = builder.Build();

// Create a new service scope.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        // Ensure the database is created.
        var dbContext = services.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.EnsureCreatedAsync();

        // Get required services.
        var identitySeeder = services.GetRequiredService<IAppSeederService>();
        var tenantResolver = services.GetRequiredService<ITenantResolver>();

        // Seed the database.
        await ApplicationDbContextSeeder.SeedAsync(identitySeeder, tenantResolver.TenantMode);
    }
    catch (Exception ex)
    {
        // Log any errors that occur.
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        // Get the error string to include in the exception message.
        var errorMessage = string.IsNullOrEmpty(ex.InnerException?.ToString()) ? ex.Message : $"{ex.Message} - {ex.InnerException.Message}";

        logger.LogError(ex, $"An error occurred while migrating or seeding the database.| {errorMessage}");
    }
}

// Configure health checks.
app.UseHealthChecks("/health");

// Configure HTTPS redirection.
//app.UseHttpsRedirection();

// Serve static files.
app.UseStaticFiles();

// Configure CORS.
app.UseCors("CorsPolicy");

// Configure app localization.
app.UseAppLocalization();

// Configure global exception handler.
app.UseBpExceptionHandler(loggingEnabled: true, debuggingEnabled: app.Environment.IsDevelopment());

// Configure routing.
app.UseRouting();

// Configure Swagger API.
app.UseSwaggerApi();

// Configure multi-tenancy.
app.UseMultiTenancy();

// Configure Identity options.
app.UseIdentityOptions();

// Configure authentication.
app.UseAuth();

// Add the Hangfire Dashboard UI to the middleware pipeline, allowing you to view and manage
// background jobs via a web-based dashboard If you want to access the hangfire dashboard from
// outside the localhost, please refer to this link. https://docs.hangfire.io/en/latest/configuration/using-dashboard.html.
app.UseHangfireDashboard();

// Map the default route for ASP.NET Core MVC controllers, so that incoming HTTP requests can be
// routed to the appropriate controller and action method.
app.MapControllers();

// Map the DashboardHub SignalR hub to the "Hubs/DashboardHub" URL path.
app.MapHub<DashboardHub>("Hubs/DashboardHub");

// Map the DataExportHub SignalR hub to the "Hubs/DataExportHub" URL path.
app.MapHub<DataExportHub>("Hubs/DataExportHub");

// Map the AssetHub SignalR hub to the "Hubs/AssetHub" URL path.
app.MapHub<AssetHub>("Hubs/AssetHub");

// Map the LiveStreamHub SignalR hub to the "Hubs/LiveStreamHub" URL path.
app.MapHub<LiveStreamHub>("Hubs/LiveStreamHub");

// Start the web application and listens for incoming requests.
app.Run();