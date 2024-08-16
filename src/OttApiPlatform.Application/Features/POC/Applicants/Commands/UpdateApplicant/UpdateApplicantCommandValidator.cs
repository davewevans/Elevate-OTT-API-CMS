namespace OttApiPlatform.Application.Features.POC.Applicants.Commands.UpdateApplicant;

public class UpdateApplicantCommandValidator : AbstractValidator<UpdateApplicantCommand>
{
    #region Public Constructors

    public UpdateApplicantCommandValidator()
    {
        RuleFor(v => v.Ssn).Cascade(CascadeMode.Stop)
                           .NotEmpty()
                           .WithMessage(Resource.Social_security_number_is_required)
                           .Must(v => v is > 99999999 and < 1000000000)
                           .WithMessage(Resource.Social_security_number_must_contain_only_9_digits)
                           .Must(v => v != 123456789)
                           .WithMessage(Resource.Social_security_number_must_not_contain_consecutive_digits)
                           .Must(v => v != 111111111)
                           .WithMessage(Resource.Repeated_Ones_are_not_valid_Social_security_number)
                           .Must(v => v != 333333333)
                           .WithMessage(Resource.Repeated_Threes_are_not_valid_Social_security_number);

        RuleFor(v => v.FirstName).Cascade(CascadeMode.Stop)
                                 .NotEmpty()
                                 .WithMessage(Resource.First_name_is_required);

        RuleFor(v => v.LastName).Cascade(CascadeMode.Stop)
                                .NotEmpty()
                                .WithMessage(Resource.Last_name_is_required);

        RuleFor(v => v.DateOfBirth).Cascade(CascadeMode.Stop)
                                   .NotEmpty()
                                   .WithMessage(Resource.Date_of_Birth_is_required)
                                   .InclusiveBetween(DateTime.Now.AddYears(-28), DateTime.Now.AddYears(-18))
                                   .WithMessage(Resource.Only_those_between_the_ages_of_18_and_28_are_allowed_for_enlisting);

        RuleFor(v => v.Height).Cascade(CascadeMode.Stop)
                              .NotEmpty()
                              .WithMessage(Resource.Height_is_required)
                              .InclusiveBetween(100, 250)
                              .WithMessage(Resource.Only_those_whose_heights_between_100_and_250_with_normal_BMI_are_allowed_for_enlisting);

        RuleFor(v => v.Weight).Cascade(CascadeMode.Stop)
                              .NotEmpty()
                              .WithMessage(Resource.Weight_is_required)
                              .InclusiveBetween(40, 200)
                              .WithMessage(Resource.Only_those_who_weigh_between_50_and_200_with_normal_BMI_are_allowed_for_enlisting);

        RuleFor(v => v.Bmi).Cascade(CascadeMode.Stop)
                           .InclusiveBetween(18.5m, 24.9m)
                           .WithMessage(Resource.Only_those_whose_BMI_between_18_5_and_24_9_are_allowed_for_enlisting);

        RuleForEach(v => v.ModifiedApplicantReferences).SetValidator(new ReferenceItemForEditValidator());

        RuleForEach(v => v.NewApplicantReferences).SetValidator(new ReferenceItemForAddValidator());
    }

    #endregion Public Constructors

    #region Public Classes

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

    #endregion Public Classes
}