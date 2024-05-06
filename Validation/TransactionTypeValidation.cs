using FluentValidation;
using ptdn_net.Data.Dto.Transaction;

namespace ptdn_net.Validation;

public class TransactionTypeValidation : AbstractValidator<TransactionTypeReq>
{
    private const int NameMaxLength = 150;

    public TransactionTypeValidation()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(NameMaxLength);
    }
}