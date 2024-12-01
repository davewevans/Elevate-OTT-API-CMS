using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OttApiPlatform.Domain.Entities.ContentAccessManagement;

[Table("Rentals")]
public class RentalModel : BaseEntity, IMustHaveTenant
{
    public Guid TenantId { get; set; }

    public Guid ContentId { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }
    public string Details { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; }
    public RentalDuration RentalDuration { get; set; }

    #region Navigational Properties 

    public ICollection<RentalContentModel> RentalContents { get; set; }

    #endregion
}


