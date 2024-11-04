namespace OttApiPlatform.Domain.Entities
{
    /// <summary>
    /// Represents account information for a tenant.
    /// </summary>
    [Table("AccountInfo")]
    public class AccountInfo : IAuditable, IMustHaveTenant
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the channel name.
        /// </summary>
        public string ChannelName { get; set; }

        /// <summary>
        /// Gets or sets the license key.
        /// </summary>
        public string LicenseKey { get; set; }

        /// <summary>
        /// Gets or sets the subdomain.
        /// </summary>
        public string SubDomain { get; set; }

        /// <summary>
        /// Gets or sets the custom domain.
        /// </summary>
        public string CustomDomain { get; set; }

        /// <summary>
        /// Gets or sets the storage file name prefix.
        /// </summary>
        public string StorageFileNamePrefix { get; set; }

        public Guid TenantId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }

        #endregion Public Properties
    }
}
