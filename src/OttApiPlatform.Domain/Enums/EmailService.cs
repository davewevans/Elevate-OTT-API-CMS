using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OttApiPlatform.Domain.Enums;
public enum EmailService
{
    /// <summary>
    /// Indicates the Azure email service.
    /// </summary>
    AzureCommunicationServices = 0,

    /// <summary>
    /// Indicates the PostMark email service.
    /// </summary>
    Postmark = 1
}
