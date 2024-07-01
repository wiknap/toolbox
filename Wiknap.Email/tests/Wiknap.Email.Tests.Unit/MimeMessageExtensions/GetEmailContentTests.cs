using MimeKit;

using Shouldly;

using Wiknap.Email.Models;

using Xunit;

namespace Wiknap.Email.Tests.Unit.MimeMessageExtensions;

public sealed class GetEmailContentTests : TestsBase
{
    [Fact]
    public void Given_EmptyBodyAndAttachments_When_GetEmailContent_Then_EmailContentWithEmptyBodyAndAttachmentsReturned()
    {
        // Arrange
        var message = new MimeMessage();

        // Act
        var content = message.GetEmailContent();

        // Assert
        content.Body.ShouldBeNull();
        content.Attachments.ShouldBeEmpty();
    }

    [Fact]
    public void Given_TxtBodyAndEmptyAttachments_When_GetEmailContent_Then_EmailContentWithTxtBodyAndEmptyAttachmentsReturned()
    {
        // Arrange
        var text = Faker.Lorem.Sentence();
        var message = new MimeMessage();
        message.Body = new TextPart("plain") { Text = text };

        // Act
        var content = message.GetEmailContent();

        // Assert
        content.Body.ShouldNotBeNull();
        content.Body.Message.ShouldBe(text);
        content.Body.ContentType.ShouldBe(EmailContentType.Text);
        content.Attachments.ShouldBeEmpty();
    }

    [Fact]
    public void Given_HtmlBodyAndEmptyAttachments_When_GetEmailContent_Then_EmailContentWithHtmlBodyAndEmptyAttachmentsReturned()
    {
        // Arrange
        var text = Faker.Lorem.Sentence();
        var message = new MimeMessage();
        message.Body = new TextPart("html") { Text = text };

        // Act
        var content = message.GetEmailContent();

        // Assert
        content.Body.ShouldNotBeNull();
        content.Body.Message.ShouldBe(text);
        content.Body.ContentType.ShouldBe(EmailContentType.Html);
        content.Attachments.ShouldBeEmpty();
    }

    [Fact]
    public void Given_TxtBodyAndOneAttachment_When_GetEmailContent_Then_EmailContentWithTxtBodyAndOneAttachmentReturned()
    {
        // Arrange
        var text = Faker.Lorem.Sentence();
        const string filename = "text.txt";
        var message = new MimeMessage();
        message.Body = new Multipart
        {
            new TextPart("plain") { Text = text },
            new MimePart("image/png")
            {
                Content = new MimeContent(Stream.Null),
                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = filename
            }
        };

        // Act
        var content = message.GetEmailContent();

        // Assert
        content.Body.ShouldNotBeNull();
        content.Body.Message.ShouldBe(text);
        content.Body.ContentType.ShouldBe(EmailContentType.Text);
        content.Attachments.Count.ShouldBe(1);
        var attachment = content.Attachments[0];
        attachment.Filename.ShouldBe(filename);
        attachment.Type.ShouldBe(EmailAttachmentType.Png);
        attachment.Content.ShouldNotBeNull();
    }
}
