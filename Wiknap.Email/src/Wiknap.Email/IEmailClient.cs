using JetBrains.Annotations;

namespace Wiknap.Email;

[PublicAPI]
public interface IEmailClient
{
    public Task SendEmailAsync(string mailTo, string subject, string message, bool isHtml = false,
        CancellationToken ct = default);

    public Task<string?> GetEmailContentAsync(SearchParameters parameters,
        EmailContentType? contentType = null, CancellationToken ct = default);
}

[PublicAPI]
public enum EmailContentType
{
    Text,
    Html
}

[PublicAPI]
public record SearchParameters
{
    public string? SenderEmail { get; init; }
    public string? Subject { get; init; }
    public DateTime? DeliveredAfter { get; init; }
}