using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OttApiPlatform.Domain.Entities.TenantAudience;

// TODO rethink this

[Table("ClientMembers")]
public class ClientMember : BaseEntity, IMustHaveTenant
{
    public Guid TenantId { get; set; }


    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "First Name is required.")]
    [MaxLength(50, ErrorMessage = "Maximum length for the First Name is 50 characters.")]
    public string FirstName { get; set; } = string.Empty;

    [MaxLength(50, ErrorMessage = "Maximum length for the Last Name is 50 characters.")]
    public string LastName { get; set; } = string.Empty;

    [Phone]
    public string Phone { get; set; } = string.Empty;

    public string Company { get; set; } = string.Empty;

    public string Locale { get; set; } = string.Empty;

    public string AddressLine1 { get; set; } = string.Empty;

    public string AddressLine2 { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string StateCode { get; set; } = string.Empty;

    public string State { get; set; } = string.Empty;

    public string CountryCode { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;

    public string PostalCode { get; set; } = string.Empty;

    // public string ValidationStatus { get; set; } = string.Empty;

    public bool Deleted { get; set; }
}
