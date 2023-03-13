using Wiknap.PayNow.Model;

namespace Wiknap.PayNow.Converters;

public sealed class GetRefundStatusJsonConverter : CustomEnumJsonConverter<GetRefundStatus>
{
    public GetRefundStatusJsonConverter()
        : base(new Dictionary<GetRefundStatus, string>
        {
            { GetRefundStatus.New, "NEW" },
            { GetRefundStatus.Failed, "FAILED" },
            { GetRefundStatus.Pending, "PENDING" },
            { GetRefundStatus.Cancelled, "CANCELLED" },
            { GetRefundStatus.Successful, "SUCCESSFUL" }
        })
    {
    }
}