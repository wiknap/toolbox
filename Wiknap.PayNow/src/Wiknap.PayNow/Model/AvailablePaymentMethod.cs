using System.Text.Json.Serialization;

using JetBrains.Annotations;

using Wiknap.PayNow.Converters;

namespace Wiknap.PayNow.Model;

[PublicAPI]
public sealed record AvailablePaymentMethod
{
    [JsonPropertyName("type")]
    public required PaymentMethodType Type { get; set; }

    [JsonPropertyName("paymentMethods")]
    public required PaymentMethod[] PaymentMethods { get; set; } = [];
}

[PublicAPI]
[JsonConverter(typeof(PaymentMethodTypeJsonConverter))]
public enum PaymentMethodType
{
    Blik,
    PayByLink,
    Card
}

[PublicAPI]
public sealed record PaymentMethod
{
    [JsonPropertyName("id")]
    public required int Id { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("image")]
    public required string ImageUrl { get; set; }

    [JsonPropertyName("status")]
    public required PaymentMethodStatus Status { get; set; }

    [JsonPropertyName("authorizationType")]
    public required AuthorizationType AuthorizationType { get; set; }
}

[PublicAPI]
[JsonConverter(typeof(PaymentMethodStatusJsonConverter))]
public enum PaymentMethodStatus
{
    Enabled,
    Disabled
}


[PublicAPI]
[JsonConverter(typeof(AuthorizationTypeJsonConverter))]
public enum AuthorizationType
{
    Redirect,
    Code
}

