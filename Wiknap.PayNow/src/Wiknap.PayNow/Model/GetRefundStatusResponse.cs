using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace Wiknap.PayNow.Model;

[PublicAPI]
public sealed record GetRefundStatusResponse(
    [property: JsonPropertyName("refundId")] string RefundId,
    [property: JsonPropertyName("status")] GetRefundStatus? Status,
    [property: JsonPropertyName("failureReason")] GetFailureReason? FailureReason);

[PublicAPI]
public enum GetRefundStatus
{
    NEW,
    PENDING,
    SUCCESSFUL,
    FAILED,
    CANCELLED
}

[PublicAPI]
public enum GetFailureReason
{
    CARD_BALANCE_ERROR,
    BUYER_ACCOUNT_CLOSED,
    OTHER
}