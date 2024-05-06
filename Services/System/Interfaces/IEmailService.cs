using ptdn_net.Utils.Email;

namespace ptdn_net.Services.interfaces;

public interface IEmailService
{
    Task SendEmailAsync(MailRequest mailRequest);
}