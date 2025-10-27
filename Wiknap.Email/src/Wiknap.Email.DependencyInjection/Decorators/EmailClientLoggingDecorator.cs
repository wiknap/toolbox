using Microsoft.Extensions.Logging;

using Wiknap.Email.Models;

namespace Wiknap.Email.DependencyInjection.Decorators;

internal sealed class EmailClientLoggingDecorator : IEmailClient
{
    private readonly IEmailClient _emailClient;
    private readonly ILogger<EmailClientLoggingDecorator> _logger;

    public EmailClientLoggingDecorator(IEmailClient emailClient, ILogger<EmailClientLoggingDecorator> logger)
    {
        _emailClient = emailClient;
        _logger = logger;
    }

    public async Task SendEmailAsync(EmailMessage message, CancellationToken ct = new())
    {
        var emailsList = string.Join(',', message.Recipients.Select(r => r.EmailAddress.Email));
        _logger.SendingEmail(emailsList);
        await _emailClient.SendEmailAsync(message, ct).ConfigureAwait(false);
        _logger.EmailSent(emailsList);
    }

    public async Task<EmailContent?> GetEmailContentAsync(SearchParameters parameters, CancellationToken ct = new())
    {
        _logger.SearchingEmail();
        var content = await _emailClient.GetEmailContentAsync(parameters, ct).ConfigureAwait(false);

        if (content is not null)
            _logger.EmailFound();
        else
            _logger.EmailNotFound();

        return content;
    }

    public async Task<IReadOnlyCollection<ReceivedEmailMessage>> GetEmailsAsync(SearchParameters parameters, int maxMessages = 10, CancellationToken ct = new())
    {
        _logger.SearchingEmails();
        var messages = await _emailClient.GetEmailsAsync(parameters, maxMessages, ct).ConfigureAwait(false);

        if (messages.Count > 0)
            _logger.EmailsFound(messages.Count);
        else
            _logger.EmailsNotFound();

        return messages;
    }

    public async Task SendEmailAsync(string mailTo, string subject, string message, bool isHtml = false,
        CancellationToken ct = default)
    {
        _logger.SendingEmail(mailTo);
        await _emailClient.SendEmailAsync(mailTo, subject, message, isHtml, ct).ConfigureAwait(false);
        _logger.EmailSent(mailTo);
    }
}
