var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");

builder.Services.AddHttpInterceptor(builder);

builder.Services.AddOptions();

builder.Services.AddSingleton<IAppStateManager, AppStateManager>();

builder.Services.AddScoped<SnackbarApiExceptionProvider>();

builder.Services.AddSingleton<INavigationService, NavigationService>();

builder.Services.AddSingleton<IBreadcrumbService, BreadcrumbService>();

builder.Services.AddSingleton<ILocalizationService, LocalizationService>();

builder.Services.AddScoped<IAccessTokenProvider, AccessTokenProvider>();

builder.Services.AddScoped<IApiUrlProvider, ApiUrlProvider>();

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

builder.Services.AddScoped<AuthStateProvider>();

builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();

builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();

builder.Services.AddScoped<IReturnUrlProvider, ReturnUrlProvider>();

builder.Services.AddScoped<IHttpService, HttpService>();

builder.Services.AddScoped<IAccountsClient, AccountsClient>();

builder.Services.AddScoped<IManageClient, ManageClient>();

builder.Services.AddScoped<IRolesClient, RolesClient>();

builder.Services.AddScoped<IPermissionsClient, PermissionsClient>();

builder.Services.AddScoped<IUsersClient, UsersClient>();

builder.Services.AddScoped<IMyTenantClient, MyTenantClient>();

builder.Services.AddScoped<ITenantsClient, TenantsClient>();

builder.Services.AddScoped<IAppSettingsClient, AppSettingsClient>();

builder.Services.AddScoped<IApplicantsClient, ApplicantsClient>();

builder.Services.AddScoped<IReportsClient, ReportsClient>();

builder.Services.AddScoped<IDashboardClient, DashboardClient>();

builder.Services.AddScoped<IFileUploadClient, FileUploadClient>();

builder.Services.AddLocalization();

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.ClearAfterNavigation = false;
    config.SnackbarConfiguration.BackgroundBlurred = true;
    config.SnackbarConfiguration.PreventDuplicates = true;
    config.SnackbarConfiguration.NewestOnTop = true;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 4000;
    config.SnackbarConfiguration.HideTransitionDuration = 1000;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});

builder.Services.AddAuthorizationCore();

var localStorage = builder.Build().Services.GetRequiredService<ILocalStorageService>();

var culture = await localStorage.GetItemAsync<string>("Culture");

var selectedCulture = culture == null ? new CultureInfo("en-US") : new CultureInfo(culture);

//var selectedCulture = culture; You can uncomment this line and comment the above line.

CultureInfo.DefaultThreadCurrentCulture = selectedCulture;

CultureInfo.DefaultThreadCurrentUICulture = selectedCulture;

await builder.Build().RunAsync();