using System.Text.Json.Serialization;

namespace Wiknap.PayNow.Model;

public sealed record PostRefundRequest
{
    public PostRefundRequest(int amount, RefundReason? reason)
    {
        Amount = amount;
        Reason = reason;
    }

    [JsonPropertyName("amount")]
    public int Amount { get; }

    [JsonPropertyName("reason")]
    public RefundReason? Reason { get; }
}

public enum RefundReason
{
    RMA,
    REFUND_BEFORE_14,
    REFUND_AFTER_14,
    OTHER,
}