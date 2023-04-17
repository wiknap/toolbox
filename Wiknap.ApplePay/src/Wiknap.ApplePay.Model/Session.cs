namespace Wiknap.ApplePay.Model;

public record Session(PaymentRequest Request, int Version = 6);