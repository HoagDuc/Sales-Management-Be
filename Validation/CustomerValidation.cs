using FluentValidation;
using ptdn_net.Data.Dto.Customer;

namespace ptdn_net.Validation;

public class CustomerValidation : AbstractValidator<CustomerReq>
{
    private const int CodeMaxLength = 100;
    private const int NameMaxLength = 150;
    private const int EmailMaxLength = 150;
    private const string PhoneRegex = @"^(0)[1-9][0-9]{8}$";

    public CustomerValidation()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(NameMaxLength);

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(CodeMaxLength);

        RuleFor(x => x.Email)
            .MaximumLength(EmailMaxLength)
            .EmailAddress();

        RuleFor(x => x.Phone)
            .Matches(PhoneRegex);

        RuleFor(x => x.Dob)
            .LessThan(DateTime.Now);

        RuleFor(x => x.Gender)
            .IsInEnum();
    }
}