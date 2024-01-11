using System.Text.Json.Serialization;

using JetBrains.Annotations;

using Wiknap.PayNow.Converters;

namespace Wiknap.PayNow.Model;

[PublicAPI]
public sealed record PostRefundResponse(
    [property: JsonPropertyName("refundId")]
    string RefundId,
    [property: JsonPropertyName("status")] PostRefundStatus? Status);

[PublicAPI]
[JsonConverter(typeof(PostRefundStatusJsonConverter))]
public enum PostRefundStatus
{
    New,
    Pending,
    Successful
}
