using System.Text.Json.Serialization;

namespace Wiknap.PayNow.Model;

public sealed record GetPaymentStatusResponse
{
    public GetPaymentStatusResponse(string paymentId, PaymentStatus? status)
    {
        PaymentId = paymentId;
        Status = status;
    }

    [JsonPropertyName("paymentId")]
    public string PaymentId { get; }

    [JsonPropertyName("status")]
    public PaymentStatus? Status { get; }
}