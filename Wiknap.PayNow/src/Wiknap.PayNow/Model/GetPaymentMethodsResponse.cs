using System.Text.Json.Serialization;

using JetBrains.Annotations;

using Wiknap.PayNow.Converters;

namespace Wiknap.PayNow.Model;

[PublicAPI]
public sealed record GetPaymentMethodsResponse(
    [property: JsonPropertyName("type")] PaymentMethodType? Type,
    [property: JsonPropertyName("paymentMethods")]
    PaymentMethod[] PaymentMethods);

[PublicAPI]
[JsonConverter(typeof(PaymentMethodTypeJsonConverter))]
public enum PaymentMethodType
{
    Blik,
    PayByLink,
    Card
}

[PublicAPI]
public sealed record PaymentMethod(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("description")]
    string Description,
    [property: JsonPropertyName("image")] string ImageUrl,
    [property: JsonPropertyName("status")] PaymentMethodStatus? Status);

[PublicAPI]
[JsonConverter(typeof(PaymentMethodStatusJsonConverter))]
public enum PaymentMethodStatus
{
    Enabled,
    Disabled
}
