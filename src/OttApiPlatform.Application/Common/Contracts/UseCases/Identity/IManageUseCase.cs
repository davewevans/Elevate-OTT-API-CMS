using OttApiPlatform.Application.Features.Account.Manage.Commands.ChangePassword;
using OttApiPlatform.Application.Features.Account.Manage.Commands.DeletePersonalData;
using OttApiPlatform.Application.Features.Account.Manage.Commands.EnableAuthenticator;
using OttApiPlatform.Application.Features.Account.Manage.Commands.ResetAuthenticator;
using OttApiPlatform.Application.Features.Account.Manage.Commands.SetPassword;
using OttApiPlatform.Application.Features.Account.Manage.Commands.UpdateUserAvatar;
using OttApiPlatform.Application.Features.Account.Manage.Commands.UpdateUserProfile;
using OttApiPlatform.Application.Features.Account.Manage.Queries.CheckUser2faState;
using OttApiPlatform.Application.Features.Account.Manage.Queries.DownloadPersonalData;
using OttApiPlatform.Application.Features.Account.Manage.Queries.GenerateRecoveryCodes;
using OttApiPlatform.Application.Features.Account.Manage.Queries.LoadSharedKeyAndQrCodeUri;

namespace OttApiPlatform.Application.Common.Contracts.UseCases.Identity;

public interface IManageUseCase
{
    #region Public Methods

    Task<Envelope<ChangeEmailResponse>> ChangeEmail(ChangeEmailCommand request);

    Task<Envelope<bool>> RequirePassword();

    Task<Envelope<ChangePasswordResponse>> ChangePassword(ChangePasswordCommand request);

    Task<Envelope<SetPasswordResponse>> SetPassword(SetPasswordCommand request);

    Task<Envelope<string>> Disable2Fa();

    //Task<Envelope<CurrentUserForEdit>> GetCurrentUser();

    Task<Envelope<DownloadPersonalDataResponse>> DownloadPersonalData();

    Task<Envelope<string>> DeletePersonalData(DeletePersonalDataCommand request);

    Task<Envelope<EnableAuthenticatorResponse>> EnableAuthenticator(EnableAuthenticatorCommand request);

    Task<Envelope<ResetAuthenticatorResponse>> ResetAuthenticator();

   // Task<Envelope<Get2FaStateResponse>> GetTwoFactorAuthenticationState();

    Task<Envelope<GenerateRecoveryCodesResponse>> GenerateRecoveryCodes();

    Task<Envelope<string>> UpdateUserProfile(UpdateUserProfileCommand request);

    Task<Envelope<ChangeEmailResponse>> ConfirmEmailChange(string userId, string email, string code);

    Task<Envelope<LoadSharedKeyAndQrCodeUriResponse>> LoadSharedKeyAndQrCodeUri();

    Task<Envelope<User2FaStateResponse>> CheckUser2FaState();

    //Task<Envelope<UserAvatarForEdit>> GetUserAvatar();

    Task<Envelope<string>> UpdateUserAvatar(UpdateUserAvatarCommand request);

    #endregion Public Methods
}