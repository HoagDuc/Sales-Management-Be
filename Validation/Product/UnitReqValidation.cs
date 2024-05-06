using FluentValidation;
using ptdn_net.Data.Dto.Product;

namespace ptdn_net.Validation;

public class UnitReqValidation : AbstractValidator<UnitReq>
{
    private const int NameMaxLength = 150;


    public UnitReqValidation()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(NameMaxLength);
    }
}