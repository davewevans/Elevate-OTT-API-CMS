﻿namespace OttApiPlatform.Application.Features.Account.Manage.Commands.ChangePassword;

public class ChangePasswordResponse
{
    #region Public Properties

    public string SuccessMessage { get; set; }
    public AuthResponse AuthResponse { get; set; }

    #endregion Public Properties
}