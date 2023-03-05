using System.Text.Json.Serialization;

namespace Wiknap.PayNow.Model;

public sealed record PostPaymentResponse
{
    public PostPaymentResponse(string redirectUrl, string paymentId, Status? status)
    {
        RedirectUrl = redirectUrl;
        PaymentId = paymentId;
        Status = status;
    }

    [JsonPropertyName("redirectUrl")]
    public string RedirectUrl { get; }

    [JsonPropertyName("paymentId")]
    public string PaymentId { get; }

    [JsonPropertyName("status")]
    public Status? Status { get; }
}

public enum Status
{
    NEW,
    ERROR
}