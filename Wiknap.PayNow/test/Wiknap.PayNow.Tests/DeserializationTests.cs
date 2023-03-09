using System.Text.Json;
using Wiknap.PayNow.Model;
using Xunit;

namespace Wiknap.PayNow.Tests;

public class DeserializationTests
{
    [Fact]
    public void PhoneTest()
    {
        var json = @"{" +
                   "    \"prefix\": \"+48\"," +
                   "    \"number\": \"666666666\" " +
                   "}";
        var deserialized = JsonSerializer.Deserialize<Phone>(json);

        Assert.NotNull(deserialized);
    }
}