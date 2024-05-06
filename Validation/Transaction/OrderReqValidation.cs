using FluentValidation;
using ptdn_net.Data.Dto.Transaction;

namespace ptdn_net.Validation;

public class OrderReqValidation : AbstractValidator<OrderReq>
{
    private const string PhoneRegex = @"^(0)[1-9][0-9]{8}$";
    
    public OrderReqValidation()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        
        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(100);
        
        RuleFor(x => x.Phone)
            .Matches(PhoneRegex);
        
        RuleFor(x => x.Address)
            .MaximumLength(255);
        
        RuleFor(x => x.Note)
            .MaximumLength(255);
        
        RuleFor(x => x.Discount)
            .GreaterThanOrEqualTo((short)0)
            .LessThanOrEqualTo((short)100);

        RuleFor(x => x.Tax)
            .GreaterThanOrEqualTo(0);
        
        RuleFor(x => x.Vat)
            .GreaterThanOrEqualTo(0);
        
        RuleFor(x => x.AmountDue)
            .GreaterThanOrEqualTo(0);
        
        RuleFor(x => x.AmountPaid)
            .GreaterThanOrEqualTo(0);
        
        RuleFor(x => x.AmountRemaining)
            .GreaterThanOrEqualTo(0); 
        
        RuleFor(x => x.TotalAmount)
            .GreaterThanOrEqualTo(0);
        
        RuleFor(x => x.OrderDetails)
            .NotEmpty();
        
        RuleFor(x => x.Status)
            .NotEmpty();
    }
}