using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OttApiPlatform.Domain.Entities.TransactionManagement;

public class PaymentGatewayModel : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }
    public string ApiKey { get; set; }
    public string ApiSecret { get; set; }
    public string ApiUrl { get; set; }
    public string ApiVersion { get; set; }
    public string ApiMode { get; set; }
    public string ApiUsername { get; set; }
    public string ApiPassword { get; set; }
    public string ApiSignature { get; set; }
    public string ApiToken { get; set; }
}
