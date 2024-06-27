using Wiknap.Email.Models;

namespace Wiknap.Email;

public static class EmailClientExtensions
{
    public static Task SendEmailAsync(this IEmailClient emailClient, string mailTo, string? subject, string message,
        bool isHtml = false,
        CancellationToken ct = default)
    {
        var emailMessage = new EmailMessage
        {
            Subject = subject, Body = new EmailBody(message, isHtml ? EmailContentType.Html : EmailContentType.Text)
        };

        emailMessage.Recipients.Add(mailTo);
        return emailClient.SendEmailAsync(emailMessage, ct);
    }

    public static Task SendEmailAsync(this IEmailClient emailClient, Recipient[] recipients, string subject,
        string message, bool isHtml = false,
        CancellationToken ct = default)
    {
        var emailMessage = new EmailMessage
        {
            Subject = subject, Body = new EmailBody(message, isHtml ? EmailContentType.Html : EmailContentType.Text)
        };

        foreach (var recipient in recipients)
            emailMessage.Recipients.Add(recipient);

        return emailClient.SendEmailAsync(emailMessage, ct);
    }
}
