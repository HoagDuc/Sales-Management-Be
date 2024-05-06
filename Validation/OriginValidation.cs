using FluentValidation;
using ptdn_net.Data.Dto.Product;

namespace ptdn_net.Validation;

public class OriginValidation : AbstractValidator<OriginReq>
{
    private const int CodeMaxLength = 100;
    private const int NameMaxLength = 150;

    public OriginValidation()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(NameMaxLength);

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(CodeMaxLength);
    }
}