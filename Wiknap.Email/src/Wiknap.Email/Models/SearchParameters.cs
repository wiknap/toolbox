namespace Wiknap.Email.Models;

public record SearchParameters
{
    public string? SenderEmail { get; init; }
    public string? Subject { get; init; }
    public DateTime? DeliveredAfter { get; init; }
}
