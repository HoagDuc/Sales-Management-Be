using FluentValidation;
using ptdn_net.Data.Dto.Transaction;

namespace ptdn_net.Validation;

public class TransactionTypeReqValidation : AbstractValidator<TransactionTypeReq>
{
    private const int NameMaxLength = 150;

    public TransactionTypeReqValidation()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(NameMaxLength);
    }
}