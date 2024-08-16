namespace OttApiPlatform.Domain.Common.Contracts;

/// <summary>
/// Represents an entity that may or may not have a tenant associated with it.
/// </summary>
public interface IMayHaveTenant
{
    #region Public Properties

    /// <summary>
    /// Gets or sets the ID of the tenant associated with the entity. Null if the entity is not.
    /// associated with a tenant.
    /// </summary>
    Guid? TenantId { get; set; }

    #endregion Public Properties
}