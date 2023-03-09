using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace Wiknap.PayNow.Model;

[PublicAPI]
public sealed record GetPaymentMethodsResponse(
    [property: JsonPropertyName("type")] PaymentMethodType? Type,
    [property: JsonPropertyName("paymentMethods")] PaymentMethod[] PaymentMethods);

[PublicAPI]
public enum PaymentMethodType
{
    BLIK,
    PBL,
    CARD
}

[PublicAPI]
public sealed record PaymentMethod(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("description")] string Description,
    [property: JsonPropertyName("image")] string ImageUrl,
    [property: JsonPropertyName("status")] PaymentMethodStatus? Status);

[PublicAPI]
public enum PaymentMethodStatus
{
    ENABLED,
    DISABLED
}