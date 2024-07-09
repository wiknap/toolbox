using System.Text.Json.Serialization;

using JetBrains.Annotations;

using Wiknap.PayNow.Converters;

namespace Wiknap.PayNow.Model;

[PublicAPI]
public sealed record PostRefundResponse
{
    [JsonPropertyName("refundId")]
    public required string RefundId { get; init; }

    [JsonPropertyName("status")]
    public required PostRefundStatus Status { get; init; }
}

[PublicAPI]
[JsonConverter(typeof(PostRefundStatusJsonConverter))]
public enum PostRefundStatus
{
    New,
    Pending,
    Successful
}
