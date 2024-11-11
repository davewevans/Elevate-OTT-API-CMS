using OttApiPlatform.Application.Features.Identity.Roles.Commands.CreateRole;
using OttApiPlatform.Application.Features.Identity.Roles.Commands.DeleteRole;
using OttApiPlatform.Application.Features.Identity.Roles.Commands.UpdateRole;
using OttApiPlatform.Application.Features.Identity.Roles.Queries.GetRoleForEdit;
using OttApiPlatform.Application.Features.Identity.Roles.Queries.GetRolePermissions;
using OttApiPlatform.Application.Features.Identity.Roles.Queries.GetRoles;

namespace OttApiPlatform.Application.Common.Contracts.UseCases.Identity;

public interface IRoleUseCase
{
    #region Public Methods

    //Task<Envelope<RoleForEdit>> GetRole(GetRoleForEditQuery request);

    Task<Envelope<RolesResponse>> GetRoles(GetRolesQuery request);

    Task<Envelope<CreateRoleResponse>> AddRole(CreateRoleCommand request);

    Task<Envelope<string>> EditRole(UpdateRoleCommand request);

    Task<Envelope<string>> DeleteRole(DeleteRoleCommand request);

    //Task<Envelope<RolePermissionsForEdit>> GetRolePermissions(GetRolePermissionsForEditQuery request);

    #endregion Public Methods
}