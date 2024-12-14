using OttApiPlatform.Domain.Entities.ContentManagement;

namespace OttApiPlatform.Domain.Entities
{
    /// <summary>
    /// Represents account information for a tenant.
    /// </summary>
    [Table("AccountInfo")]
    public class AccountInfoModel : BaseEntity, IMustHaveTenant
    {
        #region Public Properties

        public Guid TenantId { get; set; }

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

        /// <summary>
        /// Gets or sets the streaming service used.
        /// </summary>
        public VodStreamingService VodStreamingService { get; set; }
     
        #region Foreign Key Properties

        public Guid? CountryId { get; set; } 

        #endregion

        #endregion Public Properties

        #region Navigational Properties

        public CountryModel Country { get; set; }

        #endregion


    }
}
