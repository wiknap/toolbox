using System.Globalization;
using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace Wiknap.PayNow.Model;

[PublicAPI]
public sealed record PostPaymentRequest
{
    public PostPaymentRequest(
        decimal amount, Currency? currency, string externalId, string description, string continueUrl, Buyer buyer)
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
        get
        {
            var valueAsString = AmountAsInt.ToString(CultureInfo.InvariantCulture);
            var valueWithComma = $"{valueAsString[..^2]},{valueAsString[^2..]}";
            return decimal.Parse(valueWithComma, CultureInfo.InvariantCulture);
        }
        private init
        {
            var valueAsString = $"{value:0.00}";
            var valueNoComma = valueAsString.Replace(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator,
                string.Empty);
            AmountAsInt = int.Parse(valueNoComma, CultureInfo.InvariantCulture);
        }
    }

    [JsonPropertyName("amount")]
    public int AmountAsInt { get; private init; }

    [JsonPropertyName("currency")]
    public Currency? Currency { get; }

    [JsonPropertyName("externalId")]
    public string ExternalId { get; }

    [JsonPropertyName("description")]
    public string Description { get; }

    [JsonPropertyName("continueUrl")]
    public string ContinueUrl { get; }

    [JsonPropertyName("buyer")]
    public Buyer Buyer { get; }
}

[PublicAPI]
public sealed record Buyer(
    [property: JsonPropertyName("email")] string Email,
    [property: JsonPropertyName("firstName")] string FirstName,
    [property: JsonPropertyName("lastName")] string LastName,
    [property: JsonPropertyName("phone")] Phone Phone,
    [property: JsonPropertyName("locale")] string Locale);

[PublicAPI]
public sealed record Phone(
    [property: JsonPropertyName("prefix")] string Prefix,
    [property: JsonPropertyName("number")] string Number);