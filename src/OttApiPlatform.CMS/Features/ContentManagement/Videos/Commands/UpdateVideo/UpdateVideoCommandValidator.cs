using OttApiPlatform.CMS.Features.ContentManagement.Videos.Queries.GetVideoForEdit;

namespace OttApiPlatform.CMS.Features.ContentManagement.Videos.Commands.UpdateVideo;

public class UpdateVideoCommandValidator : AbstractValidator<VideoForEdit>
{
    #region Public Constructors
    public UpdateVideoCommandValidator()
    {
        // TODO slug - no spaces or special chars


        //RuleFor(a => a.Ssn).Cascade(CascadeMode.Stop)
        //    .NotEmpty().WithMessage(BackendResources.Resource.Social_security_number_is_required)
        //    .Must(a => a > 99999999 && a < 1000000000).WithMessage(BackendResources.Resource.Social_security_number_must_contain_only_9_digits)
        //    .Must(a => a != 123456789).WithMessage(BackendResources.Resource.Social_security_number_must_not_contain_consecutive_digits)
        //    .Must(a => a != 111111111).WithMessage(BackendResources.Resource.Repeated_Ones_are_not_valid_Social_security_number)
        //    .Must(a => a != 333333333).WithMessage(BackendResources.Resource.Repeated_Threes_are_not_valid_Social_security_number);

        //RuleFor(a => a.FirstName).Cascade(CascadeMode.Stop)
        //    .NotEmpty().WithMessage(BackendResources.Resource.First_name_is_required);

        //RuleFor(a => a.LastName).Cascade(CascadeMode.Stop)
        //    .NotEmpty().WithMessage(BackendResources.Resource.Last_name_is_required);

        //RuleFor(a => a.DateOfBirth).Cascade(CascadeMode.Stop)
        //    .NotEmpty().WithMessage(BackendResources.Resource.Date_of_Birth_is_required)
        //    .InclusiveBetween(DateTime.Now.AddYears(-28), DateTime.Now.AddYears(-18)).WithMessage(BackendResources.Resource.Only_those_between_the_ages_of_18_and_28_are_allowed_for_enlisting);

        //RuleFor(a => a.Height).Cascade(CascadeMode.Stop)
        //    .NotEmpty().WithMessage(BackendResources.Resource.Height_is_required)
        //    .InclusiveBetween(100, 250).WithMessage(BackendResources.Resource.Only_those_whose_heights_between_100_and_250_with_normal_BMI_are_allowed_for_enlisting);

        //RuleFor(a => a.Weight).Cascade(CascadeMode.Stop)
        //    .NotEmpty().WithMessage(BackendResources.Resource.Weight_is_required)
        //    .InclusiveBetween(40, 200).WithMessage(BackendResources.Resource.Only_those_who_weigh_between_50_and_200_with_normal_BMI_are_allowed_for_enlisting);

        //RuleFor(a => a.Bmi).Cascade(CascadeMode.Stop)
        //    .InclusiveBetween(18.5m, 24.9m).WithMessage(BackendResources.Resource.Only_those_whose_BMI_between_18_5_and_24_9_are_allowed_for_enlisting);
    }
    #endregion Public Constructors
}
