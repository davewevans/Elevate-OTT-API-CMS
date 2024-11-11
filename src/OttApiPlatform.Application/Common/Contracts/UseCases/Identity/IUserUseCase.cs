using OttApiPlatform.Application.Features.Identity.Users.Commands.CreateUser;
using OttApiPlatform.Application.Features.Identity.Users.Commands.DeleteUser;
using OttApiPlatform.Application.Features.Identity.Users.Commands.GrantOrRevokeUserPermissions;
using OttApiPlatform.Application.Features.Identity.Users.Commands.UpdateUser;
using OttApiPlatform.Application.Features.Identity.Users.Queries.GetUserForEdit;
using OttApiPlatform.Application.Features.Identity.Users.Queries.GetUserPermissions;
using OttApiPlatform.Application.Features.Identity.Users.Queries.GetUsers;

namespace OttApiPlatform.Application.Common.Contracts.UseCases.Identity;

public interface IUserUseCase
{
    #region Public Methods

    Task<Envelope<GetUserForEditQuery>> GetUser(GetUserForEditQuery request);

    Task<Envelope<UsersResponse>> GetUsers(GetUsersQuery request);

    Task<Envelope<CreateUserResponse>> AddUser(CreateUserCommand request);

    Task<Envelope<string>> EditUser(UpdateUserCommand request);

    Task<Envelope<string>> DeleteUser(DeleteUserCommand request);

    Task<Envelope<UserPermissionsResponse>> GetUserPermissions(GetUserPermissionsQuery request);

    Task<Envelope<string>> GrantOrRevokeUserPermissions(GrantOrRevokeUserPermissionsCommand request);

    Task<List<PermissionItem>> GetUserPermissionsWithoutExcluded(ApplicationUser user);

    #endregion Public Methods
}