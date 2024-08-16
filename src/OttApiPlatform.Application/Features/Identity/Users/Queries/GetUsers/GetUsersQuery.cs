namespace OttApiPlatform.Application.Features.Identity.Users.Queries.GetUsers;

public class GetUsersQuery : FilterableQuery, IRequest<Envelope<UsersResponse>>
{
    #region Public Constructors

    public GetUsersQuery()
    {
        SelectedRoleIds = new List<string>();
    }

    #endregion Public Constructors

    #region Public Properties

    public IReadOnlyList<string> SelectedRoleIds { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, Envelope<UsersResponse>>
    {
        #region Private Fields

        private readonly IApplicationDbContext _dbContext;

        #endregion Private Fields

        #region Public Constructors

        public GetUsersQueryHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<UsersResponse>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            // Set the base query to retrieve all users from the database.
            var query = _dbContext.Users.AsQueryable();

            // If there are any selected role ids, filter the query to only include users with those roles.
            if (request.SelectedRoleIds.Count != 0)
                query = (from ur in _dbContext.UserRoles
                         where request.SelectedRoleIds.Contains(ur.RoleId)
                         select ur.User).Distinct();

            // If there is any search text, filter the query to include only users whose username or
            // email contains the search text.
            if (!string.IsNullOrWhiteSpace(request.SearchText))
                query = query.Where(u => u.UserName.Contains(request.SearchText) && u.Email.Contains(request.SearchText));

            // Sort the query based on the requested sort column and order, or by username in
            // ascending order if no sort column was requested.
            query = !string.IsNullOrWhiteSpace(request.SortBy) ? query.SortBy(request.SortBy) : query.OrderBy(u => u.UserName);

            // Execute the query and map the resulting User entities to UserItem view models, then
            // paginate the results.
            var userItems = await query.Select(q => UserItem.MapFromEntity(q))
                                       .AsNoTracking()
                                       .ToPagedListAsync(request.PageNumber, request.RowsPerPage);

            // Create a UsersResponse view model and populate it with the paginated UserItem view models.
            var usersResponse = new UsersResponse
            {
                Users = userItems
            };

            // Return an Ok result with the UsersResponse view model.
            return Envelope<UsersResponse>.Result.Ok(usersResponse);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}