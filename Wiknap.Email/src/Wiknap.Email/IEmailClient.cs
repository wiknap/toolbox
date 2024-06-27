using Wiknap.Email.Models;

namespace Wiknap.Email;

public interface IEmailClient
{
    public Task SendEmailAsync(EmailMessage message, CancellationToken ct = default);

    public Task<EmailContent?> GetEmailContentAsync(SearchParameters parameters, CancellationToken ct = default);
}
