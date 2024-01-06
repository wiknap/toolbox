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
        if (!IsExcluded(mailTo))
            return emailClient.SendEmailAsync(mailTo, subject, message, isHtml, ct);

        logger.EmailExcluded(mailTo);
        return Task.CompletedTask;
    }

    public Task<string?> GetEmailContentAsync(SearchParameters parameters, EmailContentType? contentType = null,
        CancellationToken ct = default)
    {
        return emailClient.GetEmailContentAsync(parameters, contentType, ct);
    }

    private bool IsExcluded(string mailTo)
    {
        if (configuration.ExcludeAll)
            return true;

        return false;
    }
}