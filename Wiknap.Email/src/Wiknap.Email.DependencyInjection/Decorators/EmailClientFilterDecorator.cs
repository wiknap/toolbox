using Microsoft.Extensions.Logging;

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

    public Task SendEmailAsync(string mailTo, string subject, string message, bool isHtml = false,
        CancellationToken ct = default)
    {
        if (ShouldSend(mailTo))
            return emailClient.SendEmailAsync(mailTo, subject, message, isHtml, ct);

        logger.EmailExcluded(mailTo);
        return Task.CompletedTask;
    }

    public Task<string?> GetEmailContentAsync(SearchParameters parameters, EmailContentType? contentType = null,
        CancellationToken ct = default)
    {
        return emailClient.GetEmailContentAsync(parameters, contentType, ct);
    }

    private bool ShouldSend(string mailTo)
    {
        if (IsIncluded(mailTo))
            return true;

        return !IsExcluded(mailTo);
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