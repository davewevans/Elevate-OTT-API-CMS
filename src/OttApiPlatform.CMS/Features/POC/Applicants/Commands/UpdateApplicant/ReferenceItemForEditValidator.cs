namespace OttApiPlatform.CMS.Features.POC.Applicants.Commands.UpdateApplicant;

public class ReferenceItemForEditValidator : AbstractValidator<ReferenceItemForEdit>
{
    #region Public Constructors

    public ReferenceItemForEditValidator()
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