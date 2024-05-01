using System.Text.Json.Serialization;

using JetBrains.Annotations;

namespace Wiknap.PayNow.Model;

[PublicAPI]
public sealed record GetPaymentStatusResponse
{
    [JsonPropertyName("paymentId")]
    public required string PaymentId { get; set; }

    [JsonPropertyName("status")]
    public required PaymentStatus Status { get; set; }
}
