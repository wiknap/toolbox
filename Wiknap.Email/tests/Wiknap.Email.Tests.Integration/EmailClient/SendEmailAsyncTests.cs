using Shouldly;

using Wiknap.Email.Models;
using Wiknap.Email.Tests.Integration.Fixture;

using Xunit;

namespace Wiknap.Email.Tests.Integration.EmailClient;

public sealed class SendEmailAsyncTests : IntegrationTestsBase
{
    public SendEmailAsyncTests(EmailServer emailServer) : base(emailServer)
    {
    }

    [Fact]
    public async Task Given_EmailSubjectAndTextContent_When_SendEmailAsync_Then_EmailIsSentWithTextBody()
    {
        // Arrange
        var subject = Faker.Lorem.Word();
        var content = Faker.Lorem.Sentence();
        var emailMessage = new EmailMessage { Subject = subject, Body = content };
        emailMessage.Recipients.Add(UserEmail);

        // Act
        await EmailClient.SendEmailAsync(emailMessage, ct: Cts.Token);

        // Assert
        var emailContent = await GetUserEmailContentAsync(new SearchParameters { Subject = subject });
        emailContent.ShouldNotBeNull();
        emailContent.Body.ShouldNotBeNull();
        emailContent.Body.Message.Trim().ShouldBe(content);
        emailContent.Body.ContentType.ShouldBe(EmailContentType.Text);
        emailContent.Attachments.ShouldBeEmpty();
    }

    [Fact]
    public async Task Given_EmailSubjectAndHtmlContent_When_SendEmailAsync_Then_EmailIsSentWithHtmlBody()
    {
        // Arrange
        var subject = Faker.Lorem.Word();
        var content = Faker.Lorem.Sentence();
        var emailMessage = new EmailMessage { Subject = subject, Body = new EmailBody(content, EmailContentType.Html) };
        emailMessage.Recipients.Add(UserEmail);

        // Act
        await EmailClient.SendEmailAsync(emailMessage, ct: Cts.Token);

        // Assert
        var emailContent = await GetUserEmailContentAsync(new SearchParameters { Subject = subject });
        emailContent.ShouldNotBeNull();
        emailContent.Body.ShouldNotBeNull();
        emailContent.Body.Message.Trim().ShouldBe(content);
        emailContent.Body.ContentType.ShouldBe(EmailContentType.Html);
        emailContent.Attachments.ShouldBeEmpty();
    }

    [Fact]
    public async Task
        Given_EmailSubjectAndEmptyContentAndPngAttachment_When_SendEmailAsync_Then_EmailIsSentWithEmptyBodyAndPngAttachment()
    {
        // Arrange
        var subject = Faker.Lorem.Word();
        const string fileName = "image.png";
        await using var imageStream = EmbeddedResources.GetTestPngImage();
        await using var imageMemoryStream = new MemoryStream();
        await imageStream.CopyToAsync(imageMemoryStream);
        var imageBytes = imageMemoryStream.ToArray();
        var emailMessage = new EmailMessage { Subject = subject };
        emailMessage.Recipients.Add(UserEmail);
        emailMessage.Attachments.Add(new EmailAttachment(fileName, EmailAttachmentType.Png, imageStream));

        // Act
        await EmailClient.SendEmailAsync(emailMessage, ct: Cts.Token);

        // Assert
        await using var emailContent = await GetUserEmailContentAsync(new SearchParameters { Subject = subject });
        emailContent.ShouldNotBeNull();
        emailContent.Body.ShouldBeNull();
        emailContent.Attachments.Count.ShouldBe(1);
        var attachment = emailContent.Attachments.First();
        attachment.Filename.ShouldBe(fileName);
        attachment.Type.ShouldBe(EmailAttachmentType.Png);
        await using var attachmentMemoryStream = new MemoryStream();
        await attachment.Content.CopyToAsync(attachmentMemoryStream);
        var attachmentBytes = attachmentMemoryStream.ToArray();
        attachmentBytes.ShouldBe(imageBytes);
    }

    [Fact]
    public async Task
        Given_EmailSubjectAndTextContentAndPngAttachment_When_SendEmailAsync_Then_EmailIsSentWithTextBodyAndPngAttachment()
    {
        // Arrange
        var subject = Faker.Lorem.Word();
        var content = Faker.Lorem.Sentence();
        const string fileName = "image.png";
        await using var imageStream = EmbeddedResources.GetTestPngImage();
        await using var imageMemoryStream = new MemoryStream();
        await imageStream.CopyToAsync(imageMemoryStream);
        var imageBytes = imageMemoryStream.ToArray();
        var emailMessage = new EmailMessage { Subject = subject, Body = new EmailBody(content) };
        emailMessage.Recipients.Add(UserEmail);
        emailMessage.Attachments.Add(new EmailAttachment(fileName, EmailAttachmentType.Png, imageStream));

        // Act
        await EmailClient.SendEmailAsync(emailMessage, ct: Cts.Token);

        // Assert
        await using var emailContent = await GetUserEmailContentAsync(new SearchParameters { Subject = subject });
        emailContent.ShouldNotBeNull();
        emailContent.Body.ShouldNotBeNull();
        emailContent.Body.Message.Trim().ShouldBe(content);
        emailContent.Body.ContentType.ShouldBe(EmailContentType.Text);
        emailContent.Attachments.Count.ShouldBe(1);
        var attachment = emailContent.Attachments.First();
        attachment.Filename.ShouldBe(fileName);
        attachment.Type.ShouldBe(EmailAttachmentType.Png);
        await using var attachmentMemoryStream = new MemoryStream();
        await attachment.Content.CopyToAsync(attachmentMemoryStream);
        var attachmentBytes = attachmentMemoryStream.ToArray();
        attachmentBytes.ShouldBe(imageBytes);
    }
}
