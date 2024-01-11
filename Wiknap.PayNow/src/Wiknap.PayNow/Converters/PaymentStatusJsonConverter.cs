using Wiknap.PayNow.Model;

namespace Wiknap.PayNow.Converters;

public sealed class PaymentStatusJsonConverter : CustomEnumJsonConverter<PaymentStatus>
{
    public PaymentStatusJsonConverter()
        : base(new Dictionary<PaymentStatus, string>
        {
            { PaymentStatus.New, "NEW" },
            { PaymentStatus.Error, "ERROR" },
            { PaymentStatus.Expired, "EXPIRED" },
            { PaymentStatus.Pending, "PENDING" },
            { PaymentStatus.Abandoned, "ABANDONED" },
            { PaymentStatus.Rejected, "REJECTED" },
            { PaymentStatus.Confirmed, "CONFIRMED" }
        })
    {
    }
}
