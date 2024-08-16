namespace OttApiPlatform.Domain.Common.Contracts;

/// <summary>
/// Interface to be implemented by entities that must have a tenant association.
/// </summary>
public interface IMustHaveTenant
{
    #region Public Properties

    /// <summary>
    /// The unique identifier of the tenant that owns this entity.
    /// </summary>
    Guid TenantId { get; set; }

    #endregion Public Properties
}