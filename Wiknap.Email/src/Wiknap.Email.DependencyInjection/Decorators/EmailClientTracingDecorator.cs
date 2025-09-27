using System.Diagnostics;

using Wiknap.Email.Models;

namespace Wiknap.Email.DependencyInjection.Decorators;

internal sealed class EmailClientTracingDecorator : IEmailClient
{
    private static readonly ActivitySource EmailActivity = new("Wiknap.Email");

    private readonly IEmailClient _emailClient;

    public EmailClientTracingDecorator(IEmailClient emailClient)
    {
        _emailClient = emailClient;
    }

    public async Task SendEmailAsync(EmailMessage message, CancellationToken ct = new())
    {
        using (EmailActivity.StartActivity())
            await _emailClient.SendEmailAsync(message, ct);
    }

    public async Task<EmailContent?> GetEmailContentAsync(SearchParameters parameters, CancellationToken ct = new())
    {
        using (EmailActivity.StartActivity())
            return await _emailClient.GetEmailContentAsync(parameters, ct);
    }

    public async Task SendEmailAsync(string mailTo, string subject, string message, bool isHtml = false,
        CancellationToken ct = default)
    {
        using (EmailActivity.StartActivity())
            await _emailClient.SendEmailAsync(mailTo, subject, message, isHtml, ct);
    }
}
