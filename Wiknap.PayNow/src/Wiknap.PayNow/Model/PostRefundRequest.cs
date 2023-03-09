using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace Wiknap.PayNow.Model;

[PublicAPI]
public sealed record PostRefundRequest(
    [property: JsonPropertyName("amount")] int Amount,
    [property: JsonPropertyName("reason")] RefundReason? Reason);

[PublicAPI]
public enum RefundReason
{
    RMA,
    REFUND_BEFORE_14,
    REFUND_AFTER_14,
    OTHER
}