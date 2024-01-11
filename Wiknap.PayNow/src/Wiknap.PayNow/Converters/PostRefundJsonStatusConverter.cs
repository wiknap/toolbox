using Wiknap.PayNow.Model;

namespace Wiknap.PayNow.Converters;

public sealed class PostRefundStatusJsonConverter : CustomEnumJsonConverter<PostRefundStatus>
{
    public PostRefundStatusJsonConverter()
        : base(new Dictionary<PostRefundStatus, string>
        {
            { PostRefundStatus.New, "NEW" },
            { PostRefundStatus.Pending, "PENDING" },
            { PostRefundStatus.Successful, "SUCCESSFUL" }
        })
    {
    }
}
