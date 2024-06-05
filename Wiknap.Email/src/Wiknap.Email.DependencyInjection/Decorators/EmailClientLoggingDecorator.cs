using Microsoft.Extensions.Logging;

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

    public async Task SendEmailAsync(string mailTo, string subject, string message, bool isHtml = false,
        CancellationToken ct = default)
    {
        logger.SendingEmail(mailTo);
        await emailClient.SendEmailAsync(mailTo, subject, message, isHtml, ct).ConfigureAwait(false);
        logger.EmailSent(mailTo);
    }

    public async Task SendEmailAsync(Recipient[] recipients, string subject, string message, bool isHtml = false,
        CancellationToken ct = default)
    {
        var emailsList = string.Join(',', recipients.Select(r => r.Email));
        logger.SendingEmail(emailsList);
        await emailClient.SendEmailAsync(recipients, subject, message, isHtml, ct).ConfigureAwait(false);
        logger.EmailSent(emailsList);
    }

    public async Task<string?> GetEmailContentAsync(SearchParameters parameters, EmailContentType? contentType = null,
        CancellationToken ct = default)
    {
        logger.SearchingEmail();
        var content = await emailClient.GetEmailContentAsync(parameters, contentType, ct).ConfigureAwait(false);

        if (!string.IsNullOrEmpty(content))
            logger.EmailFound();
        else
            logger.EmailNotFound();

        return content;
    }
}
