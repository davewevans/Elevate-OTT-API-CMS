namespace OttApiPlatform.Application.Features.Identity.Roles.Commands.DeleteRole;

public class DeleteRoleCommand : IRequest<Envelope<string>>
{
    #region Public Properties

    public string Id { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, Envelope<string>>
    {
        #region Private Fields

        private readonly ApplicationRoleManager _roleManager;

        #endregion Private Fields

        #region Public Constructors

        public DeleteRoleCommandHandler(ApplicationRoleManager roleManager)
        {
            _roleManager = roleManager;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<string>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            // Check if the role ID is valid.
            if (string.IsNullOrEmpty(request.Id))
                return Envelope<string>.Result.BadRequest(Resource.Invalid_role_Id);

            // Get the role to delete from the role manager.
            var role = await _roleManager.Roles.Include(r => r.RoleClaims).FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken: cancellationToken);

            // If the role is not found, return an error message.
            if (role == null)
                return Envelope<string>.Result.NotFound(Resource.Unable_to_load_role);

            // If the role is static, return an error message.
            if (role.IsStatic)
                return Envelope<string>.Result.ServerError(Resource.Unable_to_delete_static_role);

            // Delete the role from the role manager.
            var identityResult = await _roleManager.DeleteAsync(role);

            // If the delete succeeded, remove the role from all users that have it.
            if (identityResult.Succeeded)
                await _roleManager.RemoveExcludedPermissionsFromAllUsers(role);

            // Return an error message if the delete failed, or a success message if it succeeded.
            return !identityResult.Succeeded
                ? Envelope<string>.Result.AddErrors(identityResult.Errors.ToApplicationResult(), HttpStatusCode.InternalServerError)
                : Envelope<string>.Result.Ok(Resource.Role_has_been_deleted_successfully);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}