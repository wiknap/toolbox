using System.Diagnostics;

using Wiknap.Email.DependencyInjection.OpenTelemetry;
using Wiknap.Email.Models;

namespace Wiknap.Email.DependencyInjection.Decorators;

internal sealed class EmailClientTracingDecorator : IEmailClient
{
    private readonly IEmailClient _emailClient;
    private readonly WiknapEmailInstrumentation _wiknapEmailInstrumentation;

    public EmailClientTracingDecorator(IEmailClient emailClient, WiknapEmailInstrumentation wiknapEmailInstrumentation)
    {
        _emailClient = emailClient;
        _wiknapEmailInstrumentation = wiknapEmailInstrumentation;
    }

    public async Task SendEmailAsync(EmailMessage message, CancellationToken ct = new())
    {
        using (_wiknapEmailInstrumentation.ActivitySource.StartActivity(ActivityKind.Client))
            await _emailClient.SendEmailAsync(message, ct);
    }

    public async Task<EmailContent?> GetEmailContentAsync(SearchParameters parameters, CancellationToken ct = new())
    {
        using (_wiknapEmailInstrumentation.ActivitySource.StartActivity(ActivityKind.Client))
            return await _emailClient.GetEmailContentAsync(parameters, ct);
    }

    public async Task SendEmailAsync(string mailTo, string subject, string message, bool isHtml = false,
        CancellationToken ct = default)
    {
        using (_wiknapEmailInstrumentation.ActivitySource.StartActivity(ActivityKind.Client))
            await _emailClient.SendEmailAsync(mailTo, subject, message, isHtml, ct);
    }
}
