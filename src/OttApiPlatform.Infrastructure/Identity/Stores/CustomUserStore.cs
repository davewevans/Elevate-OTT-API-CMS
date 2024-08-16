namespace OttApiPlatform.Infrastructure.Identity.Stores;

/// <summary>
/// Custom implementation of a UserStore for ApplicationUser that extends the base UserStore class.
/// </summary>
public class CustomUserStore : UserStore<ApplicationUser>
{
    #region Private Fields

    /// <summary>
    /// Private field that holds a reference to an instance of IApplicationDbContext.
    /// </summary>
    private readonly IApplicationDbContext _context;

    #endregion Private Fields

    #region Public Constructors

    /// <summary>
    /// Initializes a new instance of the CustomUserStore class.
    /// </summary>
    /// <param name="context">
    /// An instance of IApplicationDbContext used to communicate with the database.
    /// </param>
    public CustomUserStore(IApplicationDbContext context) : base((DbContext)context)
    {
        _context = context;
    }

    #endregion Public Constructors
}