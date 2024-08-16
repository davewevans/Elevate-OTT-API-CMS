namespace OttApiPlatform.Infrastructure.Identity.Stores;

/// <summary>
/// Custom role store implementation that extends RoleStore and adds a reference to the application
/// database context.
/// </summary>
public class CustomRoleStore : RoleStore<ApplicationRole>
{
    #region Private Fields

    /// <summary>
    /// Reference to the application database context.
    /// </summary>
    private readonly IApplicationDbContext _context;

    #endregion Private Fields

    #region Public Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomRoleStore"/> class with the specified
    /// application database context.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public CustomRoleStore(IApplicationDbContext context) : base((DbContext)context)
    {
        _context = context;
    }

    #endregion Public Constructors
}