using Brevo.Client;
using Microsoft.Extensions.Hosting;
using Portfolio.Business.Emails.Models;
using Attachment = Brevo.Client.Attachment;

namespace Portfolio.Business.Emails.Services;

public interface IEmailService
{
    Task<string?> SendEmailAsync(MailRequest message, CancellationToken ct = default);
}

public class EmailService(IBrevoClient brevoApi, IHostEnvironment environment) : IEmailService
{
    private readonly bool _isProduction = environment.IsProduction();

    public async Task<string?> SendEmailAsync(MailRequest message, CancellationToken cancellationToken = default)
    {
        var (finalTo, finalCc, finalBcc) = RedirectRecipientsIfNeeded(message.To, message.Cc, message.Bcc);
        message.To = finalTo;
        message.Cc = finalCc;
        message.Bcc = finalBcc;

        var smtpEmailRequest = MapToSendSmtpEmail(message);

        var response = await brevoApi.SmtpEmailPostAsync(smtpEmailRequest, cancellationToken);
        return response.MessageId;
    }

    private (ICollection<Recipient> to, ICollection<Recipient>? cc, ICollection<Recipient>? bcc) RedirectRecipientsIfNeeded(ICollection<Recipient> to, ICollection<Recipient>? cc, ICollection<Recipient>? bcc)
    {
        if (_isProduction)
        {
            return (to, cc, bcc);
        }

        // In non-production → force only dev mailbox, no CC/BCC
        return
        (
            [new Recipient { Email = "ilias.elmakrini@outlook.be", Name = "Ilias" }],
            null,
            null
        );
    }

    private SendSmtpEmail MapToSendSmtpEmail(MailRequest request)
    {
        var smtpEmail = new SendSmtpEmail
        {
            To = request.To.Select(x => new To { Email = x.Email, Name = x.Name }).ToList(),
            Cc = request.Cc?.Select(x => new Cc { Email = x.Email, Name = x.Name }).ToList(),
            Bcc = request.Bcc?.Select(x => new Bcc { Email = x.Email, Name = x.Name }).ToList(),
            Subject = request.Subject,
            HtmlContent = request.HtmlContent,
            TemplateId = request.TemplateId ?? 0,
            Params = request.Params,
            Attachment = request.Attachments?.Select(x => new Attachment
            {
                Name = x.Name,
                Url = x.Url,
                Content = x.Content
            }).ToList()
        };

        return smtpEmail;
    }
}