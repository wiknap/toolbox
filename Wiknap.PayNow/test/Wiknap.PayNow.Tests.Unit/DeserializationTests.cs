using System.Text.Json;

using Shouldly;

using Wiknap.PayNow.Model;

using Xunit;

namespace Wiknap.PayNow.Tests.Unit;

public sealed class DeserializationTests
{
    [Fact]
    public void PhoneTest_ShouldDeserialize()
    {
        const string json = "{" +
                            "    \"prefix\": \"+48\"," +
                            "    \"number\": \"666666666\" " +
                            "}";

        var deserialized = JsonSerializer.Deserialize<Phone>(json);

        deserialized.ShouldNotBeNull();
        deserialized.Prefix.ShouldBe("+48");
        deserialized.Number.ShouldBe("666666666");
    }

    [Fact]
    public void PostRefundRequest_ShouldDeserialize()
    {
        const string json = "{" +
                            "    \"amount\": 12," +
                            "    \"reason\": \"REFUND_BEFORE_14\"" +
                            "}";

        var deserialized = JsonSerializer.Deserialize<PostRefundRequest>(json);

        deserialized.ShouldNotBeNull();
        deserialized.Amount.ShouldBe(12);
        deserialized.Reason.ShouldBe(RefundReason.RefundBefore14);
    }

    [Fact]
    public void PostRefundRequestTestWithNull_ShouldDeserialize()
    {
        const string json = "{" +
                            "    \"amount\": 12" +
                            "}";

        var deserialized = JsonSerializer.Deserialize<PostRefundRequest>(json);

        deserialized.ShouldNotBeNull();
        deserialized.Amount.ShouldBe(12);
        deserialized.Reason.ShouldBe(null);
    }
}
