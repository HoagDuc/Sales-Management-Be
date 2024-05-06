using FluentValidation;
using ptdn_net.Data.Dto.Auth;

namespace ptdn_net.Validation;

public class PermissionReqValidation : AbstractValidator<PermissionReq>
{
    public PermissionReqValidation()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(255);
    }
}