using Shouldly;

using Wiknap.Email.Models;

using Xunit;

namespace Wiknap.Email.Tests.Unit.Models;

public sealed class EmailAttachmentTests : TestsBase
{
    [Fact]
    public void Given_ValidInput_When_Constructor_Then_ValidEmailAttachmentCreated()
    {
        // Arrange
        var filename = Faker.Lorem.Word();
        const EmailAttachmentType type = EmailAttachmentType.Gif;

        // Act
        var attachment = new EmailAttachment(filename, type, Stream.Null);

        // Assert
        attachment.Filename.ShouldBe(filename);
        attachment.Type.ShouldBe(type);
        attachment.Content.ShouldNotBeNull();
    }

    [Fact]
    public async Task Given_Attachment_When_DisposeAsync_Then_StreamDisposed()
    {
        // Arrange
        var filename = Faker.Lorem.Word();
        const EmailAttachmentType type = EmailAttachmentType.Gif;
        await using var memoryStream = new MemoryStream([1]);
        var attachment = new EmailAttachment(filename, type, memoryStream);

        // Act
        await attachment.DisposeAsync();

        // Assert
        attachment.Filename.ShouldBe(filename);
        attachment.Type.ShouldBe(type);
        attachment.Content.ShouldNotBeNull();
        Should.Throw<ObjectDisposedException>(() => memoryStream.Length);
    }
}
