using System.Globalization;
using System.Text.Json.Serialization;

namespace Wiknap.PayNow.Model;

public sealed record PostPaymentRequest
{
    public PostPaymentRequest(decimal amount, Currency? currency, string externalId, string description, string continueUrl, Buyer buyer)
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
            var valueAsString = AmountAsInt.ToString();
            var valueWithComma = $"{valueAsString[..^2]},{valueAsString[^2..]}";
            return decimal.Parse(valueWithComma);
        }
        private init
        {
            var valueAsString = $"{value:0.00}";
            var valueNoComma = valueAsString.Replace(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, "");
            AmountAsInt = int.Parse(valueNoComma);
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

public sealed record Buyer
{
    public Buyer(string email, string firstName, string lastName, Phone phone, string locale)
    {
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        Phone = phone;
        Locale = locale;
    }

    [JsonPropertyName("email")]
    public string Email { get; }

    [JsonPropertyName("firstName")]
    public string FirstName { get; }

    [JsonPropertyName("lastName")]
    public string LastName { get; }

    [JsonPropertyName("phone")]
    public Phone Phone { get; }

    [JsonPropertyName("locale")]
    public string Locale { get; }
}

public sealed record Phone
{
    public Phone(string prefix, string number)
    {
        Prefix = prefix;
        Number = number;
    }

    [JsonPropertyName("Prefix")]
    public string Prefix { get; }

    [JsonPropertyName("number")]
    public string Number { get; }
}