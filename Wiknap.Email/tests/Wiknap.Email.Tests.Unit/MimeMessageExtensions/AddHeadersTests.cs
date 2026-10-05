using System.Collections.ObjectModel;

using MimeKit;

using Shouldly;

using Xunit;

namespace Wiknap.Email.Tests.Unit.MimeMessageExtensions;

public sealed class AddHeadersTests : TestsBase
{
    [Fact]
    public void Given_Headers_When_AddHeaders_Then_HeadersAddedWithValues()
    {
        // Arrange
        var message = new MimeMessage();
        var tenant = Faker.Lorem.Word();
        var configurationSet = Faker.Lorem.Word();
        var headers = new Dictionary<string, string>
        {
            ["X-SES-TENANT"] = tenant,
            ["X-SES-CONFIGURATION-SET"] = configurationSet
        };

        // Act
        message.AddHeaders(headers);

        // Assert
        message.Headers["X-SES-TENANT"].ShouldBe(tenant);
        message.Headers["X-SES-CONFIGURATION-SET"].ShouldBe(configurationSet);
    }

    [Fact]
    public void Given_NoHeaders_When_AddHeaders_Then_HeadersUnchanged()
    {
        // Arrange
        var message = new MimeMessage();
        var countBefore = message.Headers.Count;

        // Act
        message.AddHeaders(ReadOnlyDictionary<string, string>.Empty);

        // Assert
        message.Headers.Count.ShouldBe(countBefore);
    }
}
