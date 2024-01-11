using Wiknap.PayNow.Model;

namespace Wiknap.PayNow.Converters;

public sealed class GetFailureReasonJsonConverter : CustomEnumJsonConverter<GetFailureReason>
{
    public GetFailureReasonJsonConverter()
        : base(new Dictionary<GetFailureReason, string>
        {
            { GetFailureReason.CardBalanceError, "CARD_BALANCE_ERROR" },
            { GetFailureReason.BuyerAccountClosed, "BUYER_ACCOUNT_CLOSED" },
            { GetFailureReason.Other, "OTHER" }
        })
    {
    }
}
