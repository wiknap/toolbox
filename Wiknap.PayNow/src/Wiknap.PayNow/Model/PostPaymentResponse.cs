using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace Wiknap.PayNow.Model;

[PublicAPI]
public sealed record PostPaymentResponse(
    [property: JsonPropertyName("redirectUrl")] string RedirectUrl,
    [property: JsonPropertyName("paymentId")] string PaymentId,
    [property: JsonPropertyName("status")] Status? Status);

[PublicAPI]
public enum Status
{
    NEW,
    ERROR
}