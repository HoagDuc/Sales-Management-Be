using FluentValidation;
using ptdn_net.Data.Dto.Product;

namespace ptdn_net.Validation;

public class ProductReqValidation : AbstractValidator<ProductReq>
{
    private const int CodeMaxLength = 100;
    private const int NameMaxLength = 150;
    private const int ShortDescriptionMaxLength = 150;
    private const int BarCodeMaxLength = 100;

    public ProductReqValidation()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Name)
            .MaximumLength(NameMaxLength);

        RuleFor(x => x.Code)
            .MaximumLength(CodeMaxLength);

        RuleFor(x => x.ShortDescription)
            .MaximumLength(ShortDescriptionMaxLength);
        
        RuleFor(x => x.Barcode)
            .MaximumLength(BarCodeMaxLength);
    }
}