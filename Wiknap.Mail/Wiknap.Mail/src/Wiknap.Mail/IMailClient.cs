namespace Wiknap.Mail;

public interface IMailClient
{
    public Task SendMailAsync(string mailTo, string subject, string message, bool isHtml = false,
        CancellationToken ct = default);

    public Task<string?> GetMailContentAsync(SearchParameters parameters,
        MailContentType? contentType = null, CancellationToken ct = default);
}

public enum MailContentType
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