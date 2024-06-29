using Shouldly;

using Wiknap.Email.Models;

using Xunit;

namespace Wiknap.Email.Tests.Unit.EmailMessageExtensions;

public sealed class ToTextPartTests : TestsBase
{
    [Fact]
    public void Given_TextBody_When_ToTextPart_When_TextPartWithMessageAndTextMimeTypeCreated()
    {
        // Arrange
        var message = Faker.Lorem.Sentence();
        var body = new EmailBody(message);

        // Act
        var textPart = body.ToTextPart();

        // Assert
        textPart.Text.ShouldBe(message);
        textPart.ContentType.MimeType.ShouldBe("text/plain");
    }

    [Fact]
    public void Given_HtmlBody_When_ToTextPart_When_TextPartWithMessageAndHtmlMimeTypeCreated()
    {
        // Arrange
        var message = Faker.Lorem.Sentence();
        var body = new EmailBody(message, EmailContentType.Html);

        // Act
        var textPart = body.ToTextPart();

        // Assert
        textPart.Text.ShouldBe(message);
        textPart.ContentType.MimeType.ShouldBe("text/html");
    }
}
