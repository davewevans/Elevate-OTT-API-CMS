namespace OttApiPlatform.CMS.Features.Identity.Manage.Commands.UpdateUserProfile;

public class UpdateUserProfileCommand
{
    #region Public Properties

    public string Name { get; set; }
    public string Surname { get; set; }
    public string JobTitle { get; set; }
    public string PhoneNumber { get; set; }

    #endregion Public Properties
}