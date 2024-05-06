using FluentValidation;
using ptdn_net.Data.Dto.Product;

namespace ptdn_net.Validation;

public class BrandReqValidation : AbstractValidator<BrandReq>
{
    public BrandReqValidation()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        
        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(100);
        
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);
    }
}