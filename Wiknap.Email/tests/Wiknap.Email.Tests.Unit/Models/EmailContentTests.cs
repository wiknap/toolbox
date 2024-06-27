using Shouldly;

using Wiknap.Email.Models;

using Xunit;

namespace Wiknap.Email.Tests.Unit.Models;

public sealed class EmailContentTests : TestsBase
{
    [Fact]
    public void Given_NullBody_When_Constructor_Then_ValidEmailContentWithNullBodyAndEmptyAttachmentsCreated()
    {
        // Act
        var content = new EmailContent(null);

        // Assert
        content.Body.ShouldBeNull();
        content.Attachments.ShouldBeEmpty();
    }

    [Fact]
    public void Given_Body_When_Constructor_Then_ValidEmailContentWithBodyAndEmptyAttachmentsCreated()
    {
        // Arrange
        var body = new EmailBody(Faker.Lorem.Sentence());

        // Act
        var content = new EmailContent(body);

        // Assert
        content.Body.ShouldNotBeNull();
        content.Body.Message.ShouldBe(body.Message);
        content.Body.ContentType.ShouldBe(body.ContentType);
        content.Attachments.ShouldBeEmpty();
    }

    [Fact]
    public void Given_BodyAndOneAttachment_When_Constructor_Then_ValidEmailContentWithBodyAndOneAttachmentsCreated()
    {
        // Arrange
        var body = new EmailBody(Faker.Lorem.Sentence());
        var attachment = new EmailAttachment("name.png", EmailAttachmentType.Png, Stream.Null);

        // Act
        var content = new EmailContent(body, [attachment]);

        // Assert
        content.Body.ShouldNotBeNull();
        content.Body.Message.ShouldBe(body.Message);
        content.Body.ContentType.ShouldBe(body.ContentType);
        content.Attachments.Count.ShouldBe(1);
        var contentAttachment = content.Attachments[0];
        contentAttachment.Filename.ShouldBe(contentAttachment.Filename);
        contentAttachment.Type.ShouldBe(attachment.Type);
        contentAttachment.Content.ShouldNotBeNull();
    }

    [Fact]
    public async Task Given_OneAttachment_When_DisposeAsync_Then_AttachmentContentIsDisposed()
    {
        // Arrange
        var body = new EmailBody(Faker.Lorem.Sentence());
        await using var memoryStream = new MemoryStream([0]);
        var attachment = new EmailAttachment("name.png", EmailAttachmentType.Png, memoryStream);
        var content = new EmailContent(body, [attachment]);

        // Act
        await content.DisposeAsync();

        // Assert
        Should.Throw<ObjectDisposedException>(() => memoryStream.Length);
    }
}
