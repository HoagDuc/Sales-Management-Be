using FluentValidation;
using ptdn_net.Data.Dto.Transaction;

namespace ptdn_net.Validation;

public class PurchaseOrderDetailReqValidation : AbstractValidator<PurchaseOrderDetailReq>
{
    public PurchaseOrderDetailReqValidation()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        
        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo((short)0);
        
        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0);
        
        RuleFor(x => x.Discount)
            .GreaterThanOrEqualTo((short)0)
            .LessThanOrEqualTo((short)100);
    }
}