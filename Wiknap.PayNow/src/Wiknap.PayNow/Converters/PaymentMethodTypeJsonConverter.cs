using Wiknap.PayNow.Model;

namespace Wiknap.PayNow.Converters;

public sealed class PaymentMethodTypeJsonConverter : CustomEnumJsonConverter<PaymentMethodType>
{
    public PaymentMethodTypeJsonConverter()
        : base(new Dictionary<PaymentMethodType, string>
        {
            { PaymentMethodType.Blik, "BLIK" },
            { PaymentMethodType.Card, "CARD" },
            { PaymentMethodType.PayByLink, "PBL" }
        })
    {
    }
}