using Wiknap.PayNow.Model;

namespace Wiknap.PayNow.Converters;

public sealed class StatusJsonConverter : CustomEnumJsonConverter<Status>
{
    public StatusJsonConverter()
        : base(new Dictionary<Status, string>
        {
            { Status.New, "NEW" },
            { Status.Error, "ERROR" }
        })
    {
    }
}
