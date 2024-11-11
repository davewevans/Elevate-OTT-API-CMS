using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OttApiPlatform.Domain.Enums;
public enum Subscription
{
    /// <summary>
    /// Indicates the free subscription.
    /// </summary>
    Free = 0,

    /// <summary>
    /// Indicates the basic subscription.
    /// </summary>
    Basic = 1,

    /// <summary>
    /// Indicates the premium subscription.
    /// </summary>
    Premium = 2
}
