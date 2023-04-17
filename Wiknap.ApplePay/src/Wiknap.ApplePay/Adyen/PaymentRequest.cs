namespace Wiknap.ApplePay.Adyen;

public record PaymentRequest(Amount Amount, string Reference, PaymentMethod PaymentMethod, string ReturnUrl,
    string MerchantAccount);

public record Amount(string Currency, int Value);

public record PaymentMethod(string Type, string ApplePayToken);