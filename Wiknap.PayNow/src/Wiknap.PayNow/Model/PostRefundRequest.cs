using System.Globalization;
using System.Text.Json.Serialization;

using JetBrains.Annotations;

using Wiknap.PayNow.Converters;

namespace Wiknap.PayNow.Model;

[PublicAPI]
public sealed record PostRefundRequest
{
    [JsonConstructor]
    private PostRefundRequest(int amountAsInt, RefundReason reason)
    {
        Reason = reason;
        AmountAsInt = amountAsInt;
    }


    public PostRefundRequest(decimal amount, RefundReason reason)
    {
        Reason = reason;
        Amount = amount;
    }

    [JsonIgnore]
    public decimal Amount
    {
        get => decimal.Divide(AmountAsInt, 100);
        set => AmountAsInt = (int)Math.Floor(value * 100);
    }

    [JsonPropertyName("amount")]
    public int AmountAsInt { get; private set; }

    [JsonPropertyName("reason")]
    public RefundReason Reason { get; set; }
}

[PublicAPI]
[JsonConverter(typeof(RefundReasonJsonConverter))]
public enum RefundReason
{
    RMA,
    RefundBefore14,
    RefundAfter14,
    Other
}
