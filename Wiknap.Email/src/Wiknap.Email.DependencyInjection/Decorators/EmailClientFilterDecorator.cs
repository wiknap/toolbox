using Microsoft.Extensions.Logging;

using Wiknap.Email.Models;

namespace Wiknap.Email.DependencyInjection.Decorators;

internal sealed class EmailClientFilterDecorator : IEmailClient
{
    private readonly IEmailFilterConfiguration _configuration;
    private readonly IEmailClient _emailClient;
    private readonly ILogger<EmailClientFilterDecorator> _logger;

    public EmailClientFilterDecorator(IEmailClient emailClient, IEmailFilterConfiguration configuration,
        ILogger<EmailClientFilterDecorator> logger)
    {
        _emailClient = emailClient;
        _configuration = configuration;
        _logger = logger;
    }

    public Task SendEmailAsync(EmailMessage message, CancellationToken ct = new())
    {
        foreach (var recipient in message.Recipients)
        {
            if (ShouldSend(recipient))
                continue;

            message.Recipients.Remove(recipient);
            _logger.EmailExcluded(recipient.EmailAddress.Email);
        }

        return message.Recipients.Count > 0
            ? _emailClient.SendEmailAsync(message, ct)
            : Task.CompletedTask;
    }

    public Task<EmailContent?> GetEmailContentAsync(SearchParameters parameters, CancellationToken ct = new())
        => _emailClient.GetEmailContentAsync(parameters, ct);

    public Task SendEmailAsync(string mailTo, string subject, string message, bool isHtml = false,
        CancellationToken ct = default)
    {
        if (ShouldSend(mailTo))
            return _emailClient.SendEmailAsync(mailTo, subject, message, isHtml, ct);

        _logger.EmailExcluded(mailTo);
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
        if (_configuration.ExcludeAll)
            return true;

        foreach (var rule in _configuration.Exclude)
        {
            if (rule.Validate(mailTo))
                return true;
        }

        return false;
    }

    private bool IsIncluded(string mailTo)
    {
        foreach (var rule in _configuration.Include)
        {
            if (rule.Validate(mailTo))
                return true;
        }

        return false;
    }
}
