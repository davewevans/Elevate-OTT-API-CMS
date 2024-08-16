var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();
builder.Services.AddScoped<ITenantUrlProvider, TenantUrlProvider>();
builder.Services.AddScoped<SnackbarApiExceptionProvider>();

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.Configuration.GetSection("UrlOptions:BaseApiUrl").Value ?? throw new InvalidOperationException())
});

builder.Services.AddScoped<IHttpService, HttpService>();
builder.Services.AddScoped<ITenantsClient, TenantsClient>();
builder.Services.Configure<UrlOptions>(builder.Configuration.GetSection(UrlOptions.Section));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();