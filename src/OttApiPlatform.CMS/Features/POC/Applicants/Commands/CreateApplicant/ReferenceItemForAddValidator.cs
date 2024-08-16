namespace OttApiPlatform.CMS.Features.POC.Applicants.Commands.CreateApplicant;

public class ReferenceItemForAddValidator : AbstractValidator<ReferenceItemForAdd>
{
    #region Public Constructors

    public ReferenceItemForAddValidator()
    {
        RuleFor(v => v.Name).Cascade(CascadeMode.Stop)
                            .NotEmpty()
                            .WithMessage(Resource.Reference_name_is_required);

        RuleFor(v => v.Phone).Cascade(CascadeMode.Stop)
                             .NotEmpty()
                             .WithMessage(Resource.Phone_number_is_required);
    }

    #endregion Public Constructors
}