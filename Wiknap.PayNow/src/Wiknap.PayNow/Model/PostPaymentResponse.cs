using System.Text.Json.Serialization;

using JetBrains.Annotations;

using Wiknap.PayNow.Converters;

namespace Wiknap.PayNow.Model;

[PublicAPI]
public sealed record PostPaymentResponse(
    [property: JsonPropertyName("redirectUrl")]
    string RedirectUrl,
    [property: JsonPropertyName("paymentId")]
    string PaymentId,
    [property: JsonPropertyName("status")] Status? Status);

[PublicAPI]
[JsonConverter(typeof(StatusJsonConverter))]
public enum Status
{
    New,
    Error
}
