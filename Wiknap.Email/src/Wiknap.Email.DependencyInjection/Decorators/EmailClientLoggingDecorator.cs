using Microsoft.Extensions.Logging;

using Wiknap.Email.Models;

namespace Wiknap.Email.DependencyInjection.Decorators;

internal sealed class EmailClientLoggingDecorator : IEmailClient
{
    private readonly IEmailClient emailClient;
    private readonly ILogger<EmailClientLoggingDecorator> logger;

    public EmailClientLoggingDecorator(IEmailClient emailClient, ILogger<EmailClientLoggingDecorator> logger)
    {
        this.emailClient = emailClient;
        this.logger = logger;
    }

    public async Task SendEmailAsync(EmailMessage message, CancellationToken ct = new())
    {
        var emailsList = string.Join(',', message.Recipients.Select(r => r.EmailAddress.Email));
        logger.SendingEmail(emailsList);
        await emailClient.SendEmailAsync(message, ct).ConfigureAwait(false);
        logger.EmailSent(emailsList);
    }

    public async Task<EmailContent?> GetEmailContentAsync(SearchParameters parameters, CancellationToken ct = new())
    {
        logger.SearchingEmail();
        var content = await emailClient.GetEmailContentAsync(parameters, ct).ConfigureAwait(false);

        if (content is not null)
            logger.EmailFound();
        else
            logger.EmailNotFound();

        return content;
    }

    public async Task SendEmailAsync(string mailTo, string subject, string message, bool isHtml = false,
        CancellationToken ct = default)
    {
        logger.SendingEmail(mailTo);
        await emailClient.SendEmailAsync(mailTo, subject, message, isHtml, ct).ConfigureAwait(false);
        logger.EmailSent(mailTo);
    }
}
