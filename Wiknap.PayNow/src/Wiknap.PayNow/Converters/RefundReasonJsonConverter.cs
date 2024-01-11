using Wiknap.PayNow.Model;

namespace Wiknap.PayNow.Converters;

public sealed class RefundReasonJsonConverter : CustomEnumJsonConverter<RefundReason>
{
    public RefundReasonJsonConverter()
        : base(new Dictionary<RefundReason, string>
        {
            { RefundReason.RMA, "RMA" },
            { RefundReason.RefundAfter14, "REFUND_AFTER_14" },
            { RefundReason.RefundBefore14, "REFUND_BEFORE_14" },
            { RefundReason.Other, "OTHER" }
        })
    {
    }
}
