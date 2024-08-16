namespace OttApiPlatform.Application.Features.Identity.Roles.Queries.GetRoles;

public class GetRolesQuery : FilterableQuery, IRequest<Envelope<RolesResponse>>
{
    #region Public Classes

    public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, Envelope<RolesResponse>>
    {
        #region Private Fields

        private readonly ApplicationRoleManager _roleManager;

        #endregion Private Fields

        #region Public Constructors

        public GetRolesQueryHandler(ApplicationRoleManager roleManager)
        {
            _roleManager = roleManager;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<RolesResponse>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            // Get the roles from the role manager.
            var query = _roleManager.Roles.AsQueryable();

            // Filter by search text, if any.
            if (!string.IsNullOrWhiteSpace(request.SearchText))
                query = query.Where(r => r.Name.Contains(request.SearchText));

            // Sort the roles by the given sort column, if any; otherwise, sort by name.
            query = !string.IsNullOrWhiteSpace(request.SortBy) ? query.SortBy(request.SortBy) : query.OrderBy(r => r.Name);

            // Get the role items in paged list format.
            var roleItems = await query.Select(q => RoleItem.MapFromEntity(q))
                                       .AsNoTracking()
                                       .ToPagedListAsync(request.PageNumber, request.RowsPerPage);

            // Create the response with the role items.
            var rolesResponse = new RolesResponse
            {
                Roles = roleItems
            };

            // Return the response in an Envelope with a success status.
            return Envelope<RolesResponse>.Result.Ok(rolesResponse);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}