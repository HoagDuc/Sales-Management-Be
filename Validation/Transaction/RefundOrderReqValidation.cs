using FluentValidation;
using ptdn_net.Data.Dto.Transaction;

namespace ptdn_net.Validation;

public class RefundOrderReqValidation : AbstractValidator<RefundOrderReq>
{
    public RefundOrderReqValidation()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        
        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(100);
        
        RuleFor(x => x.Adress)
            .MaximumLength(255);
        
        RuleFor(x => x.Note)
            .MaximumLength(255);
        
        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo((short)0);
        
        RuleFor(x => x.AmountOther)
            .GreaterThanOrEqualTo(0);
        
        RuleFor(x => x.TotalAmount)
            .GreaterThanOrEqualTo(0);
        
        RuleFor(x => x.Status)
            .NotEmpty();
        
        RuleFor(x => x.RefundOrderDetails)
            .NotEmpty();
        
        RuleFor(x => x.OrderId)
            .NotEmpty();
    }
}