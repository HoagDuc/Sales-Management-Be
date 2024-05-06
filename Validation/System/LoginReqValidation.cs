using FluentValidation;
using ptdn_net.Data.Dto.Auth;

namespace ptdn_net.Validation;

public class LoginReqValidation : AbstractValidator<LoginReq>
{
    public LoginReqValidation()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Username)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}