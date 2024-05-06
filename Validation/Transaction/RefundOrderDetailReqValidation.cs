using FluentValidation;
using ptdn_net.Data.Dto.Transaction;

namespace ptdn_net.Validation;

public class RefundOrderDetailReqValidation : AbstractValidator<RefundOrderDetailReq>
{
    public RefundOrderDetailReqValidation()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        
        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo((short)0);
        
        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0);
        
        RuleFor(x => x.Note)
            .MaximumLength(255);
    }
}