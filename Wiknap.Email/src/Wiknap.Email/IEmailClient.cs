namespace Wiknap.Email;

public interface IEmailClient
{
    public Task SendEmailAsync(string mailTo, string subject, string message, bool isHtml = false,
        CancellationToken ct = default);

    public Task SendEmailAsync(Recipient[] recipients, string subject, string message, bool isHtml = false,
        CancellationToken ct = default);

    public Task<string?> GetEmailContentAsync(SearchParameters parameters,
        EmailContentType? contentType = null, CancellationToken ct = default);
}

public enum RecipientType
{
    To,
    Cc,
    Bcc
}

public record Recipient(string Email, RecipientType Type = RecipientType.To, string? Name = null);

public enum EmailContentType
{
    Text,
    Html
}

public record SearchParameters
{
    public string? SenderEmail { get; init; }
    public string? Subject { get; init; }
    public DateTime? DeliveredAfter { get; init; }
}
