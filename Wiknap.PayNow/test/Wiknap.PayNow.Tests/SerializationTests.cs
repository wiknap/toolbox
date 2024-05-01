using System.Text.Json;

using Shouldly;

using Wiknap.PayNow.Model;

using Xunit;

namespace Wiknap.PayNow.Tests;

public sealed class SerializationTests
{
    [Fact]
    public void PostPaymentRequestTest_ShouldSerialize()
    {
        var request = new PostPaymentRequest(10M)
        {
            Description = "Desc",
            ExternalId = "Id",
            Buyer = new Buyer { Email = "mail@mail.com"}
        };

        var serialized = JsonSerializer.Serialize(request);

        serialized.ShouldBe("{\"amount\":1000,\"currency\":null,\"externalId\":\"Id\",\"description\":\"Desc\",\"continueUrl\":null,\"buyer\":{\"email\":\"mail@mail.com\",\"firstName\":null,\"lastName\":null,\"phone\":null,\"locale\":null}}");
    }
}
