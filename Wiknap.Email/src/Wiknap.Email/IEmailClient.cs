using Wiknap.Email.Models;

namespace Wiknap.Email;

public interface IEmailClient
{
    Task SendEmailAsync(EmailMessage message, CancellationToken ct = default);

    Task<EmailContent?> GetEmailContentAsync(SearchParameters parameters, CancellationToken ct = default);

    Task<IReadOnlyCollection<ReceivedEmailMessage>> GetEmailsAsync(SearchParameters parameters, int maxMessages = 10,
        CancellationToken ct = default);
}
