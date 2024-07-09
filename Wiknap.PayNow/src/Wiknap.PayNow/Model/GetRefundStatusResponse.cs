using System.Text.Json.Serialization;

using JetBrains.Annotations;

using Wiknap.PayNow.Converters;

namespace Wiknap.PayNow.Model;

[PublicAPI]
public sealed record GetRefundStatusResponse
{
    [JsonPropertyName("refundId")]
    public required string RefundId { get; init; }

    [JsonPropertyName("status")]
    public required GetRefundStatus Status { get; init; }

    [JsonPropertyName("failureReason")]
    public required GetFailureReason FailureReason { get; init; }
}

[PublicAPI]
[JsonConverter(typeof(GetRefundStatusJsonConverter))]
public enum GetRefundStatus
{
    New,
    Pending,
    Successful,
    Failed,
    Cancelled
}

[PublicAPI]
[JsonConverter(typeof(GetFailureReasonJsonConverter))]
public enum GetFailureReason
{
    CardBalanceError,
    BuyerAccountClosed,
    Other
}
