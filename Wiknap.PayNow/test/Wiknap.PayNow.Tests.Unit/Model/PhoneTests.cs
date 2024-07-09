using System.Text.Json;

using Shouldly;

using Wiknap.PayNow.Model;

using Xunit;

namespace Wiknap.PayNow.Tests.Unit.Model;

public sealed class PhoneTests : TestsBase
{
    [Fact]
    public void Given_ValidJson_WhenDeserialize_ShouldReturnObject()
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
}
