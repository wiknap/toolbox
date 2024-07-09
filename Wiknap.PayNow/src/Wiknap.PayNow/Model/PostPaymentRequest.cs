using System.Globalization;
using System.Text.Json.Serialization;

using JetBrains.Annotations;

namespace Wiknap.PayNow.Model;

[PublicAPI]
public sealed record PostPaymentRequest
{
    [JsonConstructor]
    private PostPaymentRequest(int amountAsInt, Currency? currency, string externalId, string description,
        string? continueUrl, Buyer buyer)
    {
        AmountAsInt = amountAsInt;
        Currency = currency;
        ExternalId = externalId;
        Description = description;
        ContinueUrl = continueUrl;
        Buyer = buyer;
    }

    public PostPaymentRequest(decimal amount, string externalId, string description, Buyer buyer,
        Currency? currency = null, string? continueUrl = null)
    {
        Amount = amount;
        Currency = currency;
        ExternalId = externalId;
        Description = description;
        ContinueUrl = continueUrl;
        Buyer = buyer;
    }

    [JsonIgnore]
    public decimal Amount
    {
        get => decimal.Divide(AmountAsInt, 100);
        set => AmountAsInt = (int)Math.Floor(value * 100);
    }

    [JsonPropertyName("amount")]
    public int AmountAsInt { get; private set; }

    [JsonPropertyName("currency")]
    public Currency? Currency { get; set; }

    [JsonPropertyName("externalId")]
    public string ExternalId { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("continueUrl")]
    public string? ContinueUrl { get; set; }

    [JsonPropertyName("buyer")]
    public Buyer Buyer { get; set; }
}

[PublicAPI]
public sealed record Buyer
{
    [JsonPropertyName("email")] public required string Email { get; set; }

    [JsonPropertyName("firstName")] public string? FirstName { get; set; }

    [JsonPropertyName("lastName")] public string? LastName { get; set; }

    [JsonPropertyName("phone")] public Phone? Phone { get; set; }

    [JsonPropertyName("locale")] public string? Locale { get; set; }
}

[PublicAPI]
public sealed record Phone
{
    [JsonPropertyName("prefix")] public required string Prefix { get; set; }

    [JsonPropertyName("number")] public required string Number { get; set; }
}
