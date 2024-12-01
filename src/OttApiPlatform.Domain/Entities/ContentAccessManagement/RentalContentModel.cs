using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using OttApiPlatform.Domain.Entities.ContentManagement;

namespace OttApiPlatform.Domain.Entities.ContentAccessManagement;

[Table("RentalContent")]
public class RentalContentModel : BaseEntity, IMustHaveTenant
{
    public Guid TenantId { get; set; }

    public Guid ContentId { get; set; }

    public Guid RentalId { get; set; }

    public ContentModel Content { get; set; }

    public RentalModel Rental { get; set; }

    public int Order { get; set; }
}
