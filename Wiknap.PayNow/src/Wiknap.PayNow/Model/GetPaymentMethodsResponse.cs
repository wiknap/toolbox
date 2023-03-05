using System.Text.Json.Serialization;

namespace Wiknap.PayNow.Model;

public sealed record GetPaymentMethodsResponse
{
    public GetPaymentMethodsResponse(PaymentMethodType? type, PaymentMethod[] paymentMethods)
    {
        Type = type;
        PaymentMethods = paymentMethods;
    }

    [JsonPropertyName("type")]
    public PaymentMethodType? Type { get; }

    [JsonPropertyName("paymentMethods")]
    public PaymentMethod[] PaymentMethods { get; set; }
}

public enum PaymentMethodType
{
    BLIK,
    PBL,
    CARD
}

public sealed record PaymentMethod
{
    public PaymentMethod(int id, string name, string description, string imageUrl, PaymentMethodStatus? status)
    {
        Id = id;
        Name = name;
        Description = description;
        ImageUrl = imageUrl;
        Status = status;
    }

    [JsonPropertyName("id")]
    public int Id { get; }

    [JsonPropertyName("name")]
    public string Name { get; }

    [JsonPropertyName("description")]
    public string Description { get; }

    [JsonPropertyName("image")]
    public string ImageUrl { get; }

    [JsonPropertyName("status")]
    public PaymentMethodStatus? Status { get; }
}

public enum PaymentMethodStatus
{
    ENABLED,
    DISABLED
}