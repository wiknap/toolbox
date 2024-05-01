using System.Globalization;
using System.Text.Json.Serialization;

using JetBrains.Annotations;

namespace Wiknap.PayNow.Model;

[PublicAPI]
public sealed record PostPaymentRequest
{
    public PostPaymentRequest(decimal amount)
    {
        var valueAsString = $"{amount:0.00}";
        var valueNoComma = valueAsString.Replace(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator,
            string.Empty);
        Amount = int.Parse(valueNoComma, CultureInfo.InvariantCulture);
    }

    [JsonPropertyName("amount")]
    public int Amount { get; private set; }

    [JsonPropertyName("currency")]
    public Currency? Currency { get; set; }

    [JsonPropertyName("externalId")]
    public required string ExternalId { get; set; }

    [JsonPropertyName("description")]
    public required string Description { get; set; }

    [JsonPropertyName("continueUrl")]
    public string? ContinueUrl { get; set; }

    [JsonPropertyName("buyer")]
    public required Buyer Buyer { get; set; }
}

[PublicAPI]
public sealed record Buyer
{
    [JsonPropertyName("email")]
    public required string Email { get; set; }

    [JsonPropertyName("firstName")]
    public string? FirstName { get; set; }

    [JsonPropertyName("lastName")]
    public string? LastName { get; set; }

    [JsonPropertyName("phone")]
    public Phone? Phone { get; set; }

    [JsonPropertyName("locale")]
    public string? Locale { get; set; }
}

[PublicAPI]
public sealed record Phone
{
    [JsonPropertyName("prefix")]
    public required string Prefix { get; set; }

    [JsonPropertyName("number")]
    public required string Number { get; set; }
}
