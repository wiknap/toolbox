using System.Text.Json.Serialization;

namespace Wiknap.PayNow.Model;

public sealed record PostRefundResponse
{
    public PostRefundResponse(string refundId, PostRefundStatus? status)
    {
        RefundId = refundId;
        Status = status;
    }

    [JsonPropertyName("refundId")]
    public string RefundId { get; }

    [JsonPropertyName("status")]
    public PostRefundStatus? Status { get; }
}

public enum PostRefundStatus
{
    NEW,
    PENDING,
    SUCCESSFUL
}