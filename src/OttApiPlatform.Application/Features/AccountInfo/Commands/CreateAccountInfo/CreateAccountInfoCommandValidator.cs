using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OttApiPlatform.Application.Features.AccountInfo.Commands.CreateAccountInfo;
public class CreateAccountInfoCommandValidator : AbstractValidator<CreateAccountInfoCommand>
{
    #region Public Constructors

    public CreateAccountInfoCommandValidator()
    {
        RuleFor(v => v.ChannelName)
            .NotEmpty()
            .WithMessage(Resource.Channel_name_required);

        RuleFor(v => v.LicenseKey)
            .NotEmpty()
            .Matches(@"^[A-F0-9]{4}-[A-F0-9]{4}-[A-F0-9]{4}-[A-F0-9]{4}-[A-F0-9]{4}-[A-F0-9]{4}-[A-F0-9]{4}$")
            .WithMessage("LicenseKey format: 4797-4D5F-9168-9458-E00F-B6F4-5DB8");

        RuleFor(v => v.SubDomain)
            .NotEmpty()
            .Matches(@"^[a-zA-Z0-9-]+$")
            .WithMessage(Resource.SubDomain_should_only_contain_alphanumeric_characters_and_hyphens);

        RuleFor(v => v.CustomDomain)
            .Matches(@"^[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*$")
            .WithMessage("CustomDomain should be a valid domain name.")
            .When(v => !string.IsNullOrEmpty(v.CustomDomain));

        RuleFor(v => v.StorageFileNamePrefix)
            .NotEmpty()
            .Matches(@"^[a-f0-9]{32}$")
            .WithMessage("StorageFileNamePrefix format: 72c0ae6965a4413086ff19377beab256");

        RuleFor(v => v.TenantId)
            .NotEmpty()
            .WithMessage("TenantId is required.");
    }

    #endregion Public Constructors
}
