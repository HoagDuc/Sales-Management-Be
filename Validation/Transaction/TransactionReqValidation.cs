using FluentValidation;
using ptdn_net.Data.Dto.Transaction;

namespace ptdn_net.Validation;

public class TransactionReqValidation : AbstractValidator<TransactionReq>
{
    private const int CodeMaxLength = 150;

    public TransactionReqValidation()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(CodeMaxLength);
        
        RuleFor(x => x.TransactionTypeId)
            .NotEmpty();
        
        RuleFor(x => x.Price)
            .NotEmpty();
        
        RuleFor(x => x.Date)
            .NotEmpty();
    }
}