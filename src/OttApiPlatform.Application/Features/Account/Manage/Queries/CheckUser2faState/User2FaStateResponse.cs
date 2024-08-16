namespace OttApiPlatform.Application.Features.Account.Manage.Queries.CheckUser2faState;

public class User2FaStateResponse
{
    #region Public Properties

    public bool IsTwoFactorEnabled { get; set; }
    public string StatusMessage { get; set; }

    #endregion Public Properties
}