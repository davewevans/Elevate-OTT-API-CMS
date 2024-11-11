namespace OttApiPlatform.Application.Features.ContentManagement.Videos.Commands.CreateVideo;

public class CreateVideoCommandValidator : AbstractValidator<CreateVideoCommand>
{
    #region Public Constructors

    public CreateVideoCommandValidator()
    {
        //RuleFor(a => a.Ssn).Cascade(CascadeMode.Stop)
        //    .NotEmpty().WithMessage(Resource.Social_security_number_is_required)
        //    .Must(a => a > 99999999 && a < 1000000000).WithMessage(Resource.Social_security_number_must_contain_only_9_digits)
        //    .Must(a => a != 123456789).WithMessage(Resource.Social_security_number_must_not_contain_consecutive_digits)
        //    .Must(a => a != 111111111).WithMessage(Resource.Repeated_Ones_are_not_valid_Social_security_number)
        //    .Must(a => a != 333333333).WithMessage(Resource.Repeated_Threes_are_not_valid_Social_security_number);

        //RuleFor(a => a.FirstName).Cascade(CascadeMode.Stop)
        //    .NotEmpty().WithMessage(Resource.First_name_is_required);

        //RuleFor(a => a.LastName).Cascade(CascadeMode.Stop)
        //    .NotEmpty().WithMessage(Resource.Last_name_is_required);

        //RuleFor(a => a.DateOfBirth).Cascade(CascadeMode.Stop)
        //    .NotEmpty().WithMessage(Resource.Date_of_Birth_is_required)
        //    .InclusiveBetween(DateTime.Now.AddYears(-28), DateTime.Now.AddYears(-18)).WithMessage(Resource.Only_those_between_the_ages_of_18_and_28_are_allowed_for_enlisting);

        //RuleFor(a => a.Height).Cascade(CascadeMode.Stop)
        //    .NotEmpty().WithMessage(Resource.Height_is_required)
        //    .InclusiveBetween(100, 250).WithMessage(Resource.Only_those_whose_heights_between_100_and_250_with_normal_BMI_are_allowed_for_enlisting);

        //RuleFor(a => a.Weight).Cascade(CascadeMode.Stop)
        //    .NotEmpty().WithMessage(Resource.Weight_is_required)
        //    .InclusiveBetween(40, 200).WithMessage(Resource.Only_those_who_weigh_between_50_and_200_with_normal_BMI_are_allowed_for_enlisting);

        //RuleFor(a => a.Bmi).Cascade(CascadeMode.Stop)
        //    .InclusiveBetween(18.5m, 24.9m).WithMessage(Resource.Only_those_whose_BMI_between_18_5_and_24_9_are_allowed_for_enlisting);

    }

    #endregion Public Constructors
}
