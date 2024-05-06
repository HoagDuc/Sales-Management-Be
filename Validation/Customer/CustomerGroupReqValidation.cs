using FluentValidation;
using ptdn_net.Data.Entity.CustomerEntity;

namespace ptdn_net.Validation;

public class CustomerGroupReqValidation : AbstractValidator<CustomerGroup>
{
    private const int CodeMaxLength = 100;
    private const int NameMaxLength = 150;
    private const short DiscountMax = 100;
    private const short DiscountMin = 0;

    public CustomerGroupReqValidation()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(NameMaxLength);

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(CodeMaxLength);

        RuleFor(x => x.Discount)
            .GreaterThanOrEqualTo(DiscountMin)
            .LessThanOrEqualTo(DiscountMax);
    }
}