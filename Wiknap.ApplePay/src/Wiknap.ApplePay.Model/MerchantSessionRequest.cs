namespace Wiknap.ApplePay.Model;

public record MerchantSessionRequest(string? MerchantIdentifier, string? DisplayName, string? Initiative, string? InitiativeContext);