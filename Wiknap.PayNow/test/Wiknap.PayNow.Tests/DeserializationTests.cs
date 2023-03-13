using System.Text.Json;
using Wiknap.PayNow.Model;
using Xunit;

namespace Wiknap.PayNow.Tests;

public class DeserializationTests
{
    [Fact]
    public void PhoneTest_ShouldDeserialize()
    {
        const string json = @"{" +
                            "    \"prefix\": \"+48\"," +
                            "    \"number\": \"666666666\" " +
                            "}";

        var deserialized = JsonSerializer.Deserialize<Phone>(json);

        Assert.NotNull(deserialized);
    }

    [Fact]
    public void PostRefundRequest_ShouldDeserialize()
    {
        const string json = @"{" +
                            "    \"amount\": 12," +
                            "    \"reason\": \"REFUND_BEFORE_14\"" +
                            "}";

        var deserialized = JsonSerializer.Deserialize<PostRefundRequest>(json);

        Assert.NotNull(deserialized);
    }

    [Fact]
    public void PostRefundRequest_ShouldSerialize()
    {
        var request = new PostRefundRequest(20, RefundReason.RefundAfter14);
        const string expected = "{\"amount\":20,\"reason\":\"REFUND_AFTER_14\"}";

        var deserialized = JsonSerializer.Serialize(request);

        Assert.Equal(expected, deserialized);
    }

    [Fact]
    public void PostRefundRequestTestWithNull_ShouldDeserialize()
    {
        const string json = @"{" +
                            "    \"amount\": 12"+
                            "}";

        var deserialized = JsonSerializer.Deserialize<PostRefundRequest>(json);

        Assert.NotNull(deserialized);
    }
}