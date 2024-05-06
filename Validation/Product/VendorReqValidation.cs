using FluentValidation;
using ptdn_net.Data.Dto.Product;

namespace ptdn_net.Validation;

public class VendorReqValidation : AbstractValidator<VendorReq>
{
    private const int CodeMaxLength = 100;
    private const int NameMaxLength = 150;
    private const int EmailMaxLength = 150;
    private const string PhoneRegex = @"^(0)[1-9][0-9]{8}$";

    public VendorReqValidation()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(NameMaxLength);

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(CodeMaxLength);

        RuleFor(x => x.Email)
            .MaximumLength(EmailMaxLength)
            .EmailAddress();

        RuleFor(x => x.Phone)
            .Matches(PhoneRegex);
    }
}