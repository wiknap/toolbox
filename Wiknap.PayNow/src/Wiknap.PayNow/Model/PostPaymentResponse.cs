using System.Text.Json.Serialization;

using JetBrains.Annotations;

using Wiknap.PayNow.Converters;

namespace Wiknap.PayNow.Model;

[PublicAPI]
public sealed record PostPaymentResponse
{
    [JsonPropertyName("redirectUrl")]
    public string? RedirectUrl { get; set; }

    [JsonPropertyName("paymentId")]
    public required string PaymentId { get; set; }

    [JsonPropertyName("status")]
    public required Status? Status { get; set; }
}

[PublicAPI]
[JsonConverter(typeof(StatusJsonConverter))]
public enum Status
{
    New,
    Error
}
