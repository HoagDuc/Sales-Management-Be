using FluentValidation;
using ptdn_net.Data.Dto.User;

namespace ptdn_net.Validation;

public class UserReqValidation : AbstractValidator<UserReq>
{
    private const int UsernameMaxLength = 100;
    private const string PasswordRegex = @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$";
    private const int EmailMaxLength = 150;
    private const int FullNameMaxLength = 150;
    private const string PhoneRegex = @"^(0)[1-9][0-9]{8}$";

    public UserReqValidation()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Username)
            .MaximumLength(UsernameMaxLength)
            .NotEmpty();

        RuleFor(x => x.Password)
           // .Matches(PasswordRegex)
            .NotEmpty();

        RuleFor(x => x.Email)
            .MaximumLength(EmailMaxLength)
            .EmailAddress()
            .NotEmpty();

        RuleFor(x => x.Fullname)
            .MaximumLength(FullNameMaxLength)
            .NotEmpty();

        RuleFor(x => x.Dob)
            .LessThanOrEqualTo(DateTime.Now);
    }
}