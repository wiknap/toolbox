using Shouldly;

using Wiknap.Email.Models;

using Xunit;

namespace Wiknap.Email.Tests.Unit.EmailMessageExtensions;

public sealed class ToContentTypeTests : TestsBase
{
    [Theory]
    [InlineData(EmailAttachmentType.Png, "image/png")]
    [InlineData(EmailAttachmentType.Gif, "image/gif")]
    public void Given_EmailAttachmentType_When_ToContentType_Then_ReturnContentTypeWityMatchingMimeType(EmailAttachmentType type, string expectedType)
    {
        // Act
        var contentType = type.ToContentType();

        // Assert
        contentType.MimeType.ShouldBe(expectedType);
    }
}
