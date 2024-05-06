using FluentValidation;
using ptdn_net.Data.Dto.Auth;

namespace ptdn_net.Validation;

public class RoleReqValidation : AbstractValidator<RoleReq>
{
    public RoleReqValidation()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.Description)
            .MaximumLength(255);
    }
}