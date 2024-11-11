using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OttApiPlatform.Domain.Enums.TenantAudience;

public enum AudienceType
{
    /// <summary>
    /// Indicates the general audience.
    /// </summary>
    General = 0,

    /// <summary>
    /// Indicates the kids audience.
    /// </summary>
    Kids = 1,

    /// <summary>
    /// Indicates the adult audience.
    /// </summary>
    Adult = 2
}
