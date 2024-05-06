using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using ptdn_net.Services.interfaces;
using ptdn_net.Utils.Email;

namespace ptdn_net.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;

    public EmailService(IOptions<EmailSettings> options)
    {
        _emailSettings = options.Value;
    }

    public async Task SendEmailAsync(MailRequest mailRequest)
    {
        var email = new MimeMessage();
        email.Sender = MailboxAddress.Parse(_emailSettings.Email);
        email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
        email.Subject = mailRequest.Subject;
        var builder = new BodyBuilder();


        // byte[] fileBytes;
        // if (File.Exists("Attachment/dummy.pdf"))
        // {
        //     var file = new FileStream("Attachment/dummy.pdf", FileMode.Open, FileAccess.Read);
        //     using (var ms = new MemoryStream())
        //     {
        //         await file.CopyToAsync(ms);
        //         fileBytes = ms.ToArray();
        //     }
        //
        //     builder.Attachments.Add("attachment.pdf", fileBytes, ContentType.Parse("application/octet-stream"));
        //     builder.Attachments.Add("attachment2.pdf", fileBytes, ContentType.Parse("application/octet-stream"));
        // }

        builder.HtmlBody = mailRequest.Body;
        email.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_emailSettings.Email, _emailSettings.Password);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}