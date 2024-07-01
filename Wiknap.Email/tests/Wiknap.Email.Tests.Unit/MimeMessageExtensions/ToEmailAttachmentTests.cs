using MimeKit;

using Shouldly;

using Wiknap.Email.Models;

using Xunit;

namespace Wiknap.Email.Tests.Unit.MimeMessageExtensions;

public sealed class ToEmailAttachmentTests : TestsBase
{
    [Fact]
    public void Given_MessagePart_When_ToEmailAttachment_Then_ReturnsEmailAttachment()
    {
        // Arrange
        var name = Faker.Lorem.Word();
        var message = new MessagePart
        {
            Message = new MimeMessage(),
            ContentDisposition = new ContentDisposition { FileName = name }
        };

        // Act
        var attachment = message.ToEmailAttachment();

        // Assert
        attachment.ShouldNotBeNull();
        attachment.Filename.ShouldBe(name);
        attachment.Type.ShouldBe(EmailAttachmentType.Rfc822);
        attachment.Content.ShouldNotBeNull();
    }

    [Fact]
    public void Given_MimePart_When_ToEmailAttachment_Then_ReturnsEmailAttachment()
    {
        // Arrange
        var name = Faker.Lorem.Word();
        var mimePart = new MimePart("image/png")
        {
            Content = new MimeContent(Stream.Null),
            ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
            ContentTransferEncoding = ContentEncoding.Base64,
            FileName = name
        };

        // Act
        var attachment = mimePart.ToEmailAttachment();

        // Assert
        attachment.ShouldNotBeNull();
        attachment.Filename.ShouldBe(name);
        attachment.Type.ShouldBe(EmailAttachmentType.Png);
        attachment.Content.ShouldNotBeNull();
    }

    [Fact]
    public void Given_Multipart_When_ToEmailAttachment_Then_ReturnsNull()
    {
        // Arrange
        var multipart = new Multipart();

        // Act
        var attachment = multipart.ToEmailAttachment();

        // Assert
        attachment.ShouldBeNull();
    }
}
