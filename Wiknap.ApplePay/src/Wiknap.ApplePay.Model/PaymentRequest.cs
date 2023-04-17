namespace Wiknap.ApplePay.Model;

public record PaymentRequest(string CountryCode, string CurrencyCode, string[] SupportedNetworks,
    string[] MerchantCapabilities, LineItem Total);