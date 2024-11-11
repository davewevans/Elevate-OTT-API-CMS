using OttApiPlatform.Application.Features.Account.Commands.ForgotPassword;
using OttApiPlatform.Application.Features.Account.Commands.LoginWith2fa;
using OttApiPlatform.Application.Features.Account.Commands.LoginWithRecoveryCode;
using OttApiPlatform.Application.Features.Account.Commands.RefreshToken;
using OttApiPlatform.Application.Features.Account.Commands.Register;
using OttApiPlatform.Application.Features.Account.Commands.ResendEmailConfirmation;
using OttApiPlatform.Application.Features.Account.Commands.ResetPassword;

namespace OttApiPlatform.Application.Common.Contracts.UseCases.Identity;

public interface IAccountUseCase
{
    #region Public Methods

    Task<Envelope<LoginResponse>> Login(LoginCommand request);

    Task<Envelope<LoginWith2FaResponse>> LoginWith2Fa(LoginWith2FaCommand request);

    Task<Envelope<LoginWithRecoveryCodeResponse>> LoginWithRecoveryCode(LoginWithRecoveryCodeCommand request);

    Task<Envelope<AuthResponse>> RefreshToken(RefreshTokenCommand request);

    Task<Envelope<RegisterResponse>> Register(RegisterCommand request);

    Task<Envelope<string>> ConfirmEmail(string userId, string code);

    Task<Envelope<ForgetPasswordResponse>> ForgotPassword(ForgetPasswordCommand request);

    Task<Envelope<string>> ResetPassword(ResetPasswordCommand request);

    Task<Envelope<ResendEmailConfirmationResponse>> ResendEmailConfirmation(ResendEmailConfirmationCommand request);

    #endregion Public Methods
}