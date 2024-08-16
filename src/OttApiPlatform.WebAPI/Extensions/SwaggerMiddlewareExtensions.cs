namespace OttApiPlatform.WebAPI.Extensions;

public static class SwaggerMiddlewareExtensions
{
    #region Public Methods

    /// <summary>
    /// Adds Swagger documentation to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">
    /// The <see cref="IServiceCollection"/> to add the Swagger documentation to.
    /// </param>
    /// <returns>The modified <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddSwaggerApi(this IServiceCollection services)
    {
        // Define the security scheme used by Swagger.
        var securityScheme = new OpenApiSecurityScheme()
        {
            Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT"
        };

        // Define the security requirement needed to access the API endpoints.
        var securityRequirement = new OpenApiSecurityRequirement
                                  {
                                      {
                                          new OpenApiSecurityScheme
                                          {
                                              Reference = new OpenApiReference
                                                          {
                                                              Type = ReferenceType.SecurityScheme,
                                                              Id = "bearerAuth"
                                                          }
                                          },
                                          Array.Empty<string>()
                                      }
                                  };

        // Configure Swagger generation options.
        services.AddSwaggerGen(options =>
        {
            // Add the "Authorization" header parameter to all API endpoints.
            options.OperationFilter<SwaggerParameterAppender>();
            // Use the default schema ID for .NET types.
            options.CustomSchemaIds(type => type.ToString());
            // Define the Swagger document for the API.
            options.SwaggerDoc("v7", new OpenApiInfo
            {
                Version = "v7",
                Title = "OttApiPlatform",
                Description = "A startup project template for .NET applications.",
                TermsOfService = new Uri("https://www.CMS.net/terms-and-conditions"),
                Contact = new OpenApiContact
                {
                    Name = "OttApiPlatforms",
                    Email = "info@CMS.net",
                    Url = new Uri("https://www.CMS.net"),
                },
                License = new OpenApiLicense
                {
                    Url = new Uri("https://CMS.net/eula"),
                }
            });

            // Add the "bearerAuth" security definition to the Swagger document.
            options.AddSecurityDefinition("bearerAuth", securityScheme);
            // Add the "bearerAuth" security requirement to the Swagger document.
            options.AddSecurityRequirement(securityRequirement);
            // Include the XML comments file in the Swagger document.
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });

        return services;
    }

    /// <summary>
    /// Configures Swagger and Swagger UI for the specified <see cref="IApplicationBuilder"/>.
    /// </summary>
    /// <param name="app">
    /// The <see cref="IApplicationBuilder"/> to configure Swagger and Swagger UI for.
    /// </param>
    /// <returns>The modified <see cref="IApplicationBuilder"/>.</returns>
    public static IApplicationBuilder UseSwaggerApi(this IApplicationBuilder app)
    {
        // Enable the Swagger middleware.
        app.UseSwagger();

        // Configure Swagger UI.
        app.UseSwaggerUI(c =>
                         {
                             // Configure the Swagger endpoint for the API.
                             c.SwaggerEndpoint("./v7/swagger.json", "OttApiPlatform v7");
                             c.InjectStylesheet("/api/swagger-ui-themes/theme-dark.css");
                             c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
                         });

        return app;
    }

    #endregion Public Methods
}