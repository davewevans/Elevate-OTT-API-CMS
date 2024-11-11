namespace OttApiPlatform.Application.Common.Contracts.DemoUserServices;

public interface IDemoIdentitySeeder
{
    #region Public Methods

    Task<Envelope<ApplicationUser>> SeedDemoOfficersUsers();

    #endregion Public Methods
}