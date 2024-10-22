namespace OttApiPlatform.WebAPI.Services.HubServices;

public class SignalRContextProvider : ISignalRContextProvider
{
    #region Private Fields

    private readonly IApplicationDbContext _dbContext;
    private readonly ITenantResolver _tenantResolver;

    #endregion Private Fields

    #region Public Constructors

    public SignalRContextProvider(IApplicationDbContext dbContext, ITenantResolver tenantResolver)
    {
        _dbContext = dbContext;
        _tenantResolver = tenantResolver;
    }

    #endregion Public Constructors

    #region Public Methods

    public string GetHostName(HubCallerContext hubCallerContext)
    {
        // Check if the given HubCallerContext is null, throw an ArgumentNullException if it is.
        ThrowExceptionIfNull(hubCallerContext);

        // Get the HttpContext from the given HubCallerContext and retrieve the request object from it.
        var httpContext = hubCallerContext.GetHttpContext()?.Request;

        // Return a formatted string with the Scheme and Host of the request object.
        return $"{httpContext?.Scheme}://{httpContext?.Host}";
    }

    public Guid? GetTenantId(HubCallerContext hubCallerContext)
    {
        // Retrieve the Tenant name from the given HubCallerContext.
        var tenantName = GetTenantName(hubCallerContext);

        // Check if the Tenant name is null or empty, return null if it is.
        if (string.IsNullOrEmpty(tenantName)) return null;

        // Return the Id of the first Tenant whose name matches the given Tenant name.
        return _dbContext.Tenants.FirstOrDefault(t => t.Name == tenantName)?.Id;
    }

    public string GetTenantName(HubCallerContext hubCallerContext)
    {
        // Check if the given HubCallerContext is null, throw an ArgumentNullException if it is.
        ThrowExceptionIfNull(hubCallerContext);

        // Retrieve the HttpContext from the given HubCallerContext and retrieve the value of the
        // "X-Tenant" query parameter Return the value as a string.
        return hubCallerContext.GetHttpContext()?.Request.Query["X-Tenant"].ToString();
    }

    public string GetUserName(HubCallerContext hubCallerContext)
    {
        // Check if the given HubCallerContext is null, throw an ArgumentNullException if it is.
        ThrowExceptionIfNull(hubCallerContext);

        // Check if the user is authenticated, throw an Exception if they are not.
        if (!hubCallerContext.User.IsAuthenticated()) throw new Exception(Resource.You_are_not_authorized);

        // Split the username by the "@" symbol and return the first part.
        return hubCallerContext.User?.Identity?.Name?.Split("@")[0];
    }

    public string GetUserNameIdentifier(HubCallerContext hubCallerContext)
    {
        // Check if the given HubCallerContext is null, throw an ArgumentNullException if it is.
        ThrowExceptionIfNull(hubCallerContext);

        // Return the value of the first claim whose type matches ClaimTypes.NameIdentifier.
        return hubCallerContext.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
    }

    public void SetTenantIdViaTenantResolver(HubCallerContext hubCallerContext)
    {
        // Check if the given HubCallerContext is null, throw an ArgumentNullException if it is.
        ThrowExceptionIfNull(hubCallerContext);

        // Get the TenantId from the GetTenantId method.
        var tenantId = GetTenantId(hubCallerContext);

        // Set the TenantId using the TenantResolver.
        _tenantResolver.SetTenantId(tenantId);
    }

    #endregion Public Methods

    #region Private Methods

    private void ThrowExceptionIfNull(HubCallerContext hubCallerContext)
    {
        // Throw an ArgumentNullException if the given HubCallerContext is null.
        if (hubCallerContext is null)
            throw new ArgumentNullException(nameof(hubCallerContext));
    }

    #endregion Private Methods
}