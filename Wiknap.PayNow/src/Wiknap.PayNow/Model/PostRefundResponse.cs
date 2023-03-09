using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace Wiknap.PayNow.Model;

[PublicAPI]
public sealed record PostRefundResponse(
    [property: JsonPropertyName("refundId")] string RefundId,
    [property: JsonPropertyName("status")] PostRefundStatus? Status);

[PublicAPI]
public enum PostRefundStatus
{
    NEW,
    PENDING,
    SUCCESSFUL
}