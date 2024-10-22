namespace OttApiPlatform.WebAPI.Models;

/// <summary>
/// Adds required header parameters to Swagger operations.
/// </summary>
public class SwaggerParameterAppender : IOperationFilter
{
    #region Public Methods

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Ensure that the operation has parameters.
        operation.Parameters ??= new List<OpenApiParameter>();

        // Add a new parameter for X-Tenant header.
        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "X-Tenant",
            In = ParameterLocation.Header,
            Schema = new OpenApiSchema
            {
                Type = "String"
            },
            Required = false
        });

        // Add a new parameter for Accept-Language header.
        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "Accept-Language",
            In = ParameterLocation.Header,
            Schema = new OpenApiSchema
            {
                Type = "String"
            },
            Required = false
        });
    }

    #endregion Public Methods
}