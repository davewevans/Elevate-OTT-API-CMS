using System.Reflection.Metadata;
using OttApiPlatform.Domain.Entities.ContentManagement;

namespace OttApiPlatform.Domain.Entities.TransactionManagement;

[Table("Transactions")]
public class TransactionModel : BaseEntity, IMustHaveTenant
{
    public Guid TenantId { get; set; }
    public Guid UserId { get; set; }
    public TransactionType Type { get; set; }
    public TransactionStatus Status { get; set; }
    public string TransactionDescription { get; set; }
    public decimal Amount { get; set; }
    public DateTime TransactionDate { get; set; }
    public PaymentGateway PaymentGateway { get; set; }

    #region Foreign Key Properties

    public Guid? ContentId { get; set; } 

    #endregion

    #region Navigational Properties 

    public ContentModel Content { get; set; } 

    #endregion
}
