using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace Wiknap.PayNow.Model;

[PublicAPI]
public sealed record GetPaymentStatusResponse(
    [property: JsonPropertyName("paymentId")] string PaymentId,
    [property: JsonPropertyName("status")] PaymentStatus? Status);