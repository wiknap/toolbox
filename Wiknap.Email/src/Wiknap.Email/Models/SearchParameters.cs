namespace Wiknap.Email.Models;

public sealed record SearchParameters
{
    public string? SenderEmail { get; init; }
    public string? Subject { get; init; }
    public DateTimeOffset? DeliveredAfter { get; init; }
}
