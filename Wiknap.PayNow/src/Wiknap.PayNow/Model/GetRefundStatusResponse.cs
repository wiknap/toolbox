using System.Text.Json.Serialization;

namespace Wiknap.PayNow.Model;

public sealed record GetRefundStatusResponse
{
    public GetRefundStatusResponse(string refundId, GetRefundStatus? status, GetFailureReason? failureReason)
    {
        RefundId = refundId;
        Status = status;
        FailureReason = failureReason;
    }

    [JsonPropertyName("refundId")]
    public string RefundId { get; }

    [JsonPropertyName("status")]
    public GetRefundStatus? Status { get; }

    [JsonPropertyName("failureReason")]
    public GetFailureReason? FailureReason { get; }
}

public enum GetRefundStatus
{
    NEW,
    PENDING,
    SUCCESSFUL,
    FAILED,
    CANCELLED
}

public enum GetFailureReason
{
    CARD_BALANCE_ERROR,
    BUYER_ACCOUNT_CLOSED,
    OTHER
}