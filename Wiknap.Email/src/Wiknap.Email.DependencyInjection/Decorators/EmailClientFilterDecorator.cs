using Microsoft.Extensions.Logging;

using Wiknap.Email.Models;

namespace Wiknap.Email.DependencyInjection.Decorators;

internal sealed class EmailClientFilterDecorator : IEmailClient
{
    private readonly IEmailFilterConfiguration configuration;
    private readonly IEmailClient emailClient;
    private readonly ILogger<EmailClientFilterDecorator> logger;

    public EmailClientFilterDecorator(IEmailClient emailClient, IEmailFilterConfiguration configuration,
        ILogger<EmailClientFilterDecorator> logger)
    {
        this.emailClient = emailClient;
        this.configuration = configuration;
        this.logger = logger;
    }

    public Task SendEmailAsync(EmailMessage message, CancellationToken ct = new())
    {
        foreach (var recipient in message.Recipients)
        {
            if (ShouldSend(recipient))
                continue;

            message.Recipients.Remove(recipient);
            logger.EmailExcluded(recipient.EmailAddress.Email);
        }

        return message.Recipients.Count > 0
            ? emailClient.SendEmailAsync(message, ct)
            : Task.CompletedTask;
    }

    public Task<EmailContent?> GetEmailContentAsync(SearchParameters parameters, CancellationToken ct = new())
        => emailClient.GetEmailContentAsync(parameters, ct);

    public Task SendEmailAsync(string mailTo, string subject, string message, bool isHtml = false,
        CancellationToken ct = default)
    {
        if (ShouldSend(mailTo))
            return emailClient.SendEmailAsync(mailTo, subject, message, isHtml, ct);

        logger.EmailExcluded(mailTo);
        return Task.CompletedTask;
    }

    private bool ShouldSend(Recipient recipient)
    {
        if (IsIncluded(recipient.EmailAddress.Email))
            return true;

        return !IsExcluded(recipient.EmailAddress.Email);
    }

    private bool IsExcluded(string mailTo)
    {
        if (configuration.ExcludeAll)
            return true;

        foreach (var rule in configuration.Exclude)
        {
            if (rule.Validate(mailTo))
                return true;
        }

        return false;
    }

    private bool IsIncluded(string mailTo)
    {
        foreach (var rule in configuration.Include)
        {
            if (rule.Validate(mailTo))
                return true;
        }

        return false;
    }
}
