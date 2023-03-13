using System.Text.Json.Serialization;
using JetBrains.Annotations;
using Wiknap.PayNow.Converters;

namespace Wiknap.PayNow.Model;

[PublicAPI]
public sealed record GetRefundStatusResponse(
    [property: JsonPropertyName("refundId")] string RefundId,
    [property: JsonPropertyName("status")] GetRefundStatus? Status,
    [property: JsonPropertyName("failureReason")] GetFailureReason? FailureReason);

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