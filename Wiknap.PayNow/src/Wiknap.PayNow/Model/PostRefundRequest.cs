using System.Text.Json.Serialization;
using JetBrains.Annotations;
using Wiknap.PayNow.Converters;

namespace Wiknap.PayNow.Model;

[PublicAPI]
public sealed record PostRefundRequest(
    [property: JsonPropertyName("amount")] int Amount,
    [property: JsonPropertyName("reason")] RefundReason? Reason);

[PublicAPI]
[JsonConverter(typeof(RefundReasonJsonConverter))]
public enum RefundReason
{
    RMA,
    RefundBefore14,
    RefundAfter14,
    Other
}