using Wiknap.PayNow.Model;

namespace Wiknap.PayNow.Converters;

public sealed class PaymentMethodStatusJsonConverter : CustomEnumJsonConverter<PaymentMethodStatus>
{
    public PaymentMethodStatusJsonConverter()
        : base(new Dictionary<PaymentMethodStatus, string>
        {
            { PaymentMethodStatus.Enabled, "ENABLED" },
            { PaymentMethodStatus.Disabled, "DISABLED" }
        })
    {
    }
}
