using Shouldly;

using Wiknap.Email.Models;
using Wiknap.Email.Tests.Integration.Fixture;

using Xunit;

namespace Wiknap.Email.Tests.Integration.EmailClient;

public sealed class GetEmailContentAsyncTests : IntegrationTestsBase
{
    public GetEmailContentAsyncTests(EmailServer emailServer) : base(emailServer)
    {
    }

    [Fact]
    public async Task
        Given_EmailSubjectAndTextContent_When_GetUserEmailContentAsyncBySubject_Then_TextContentIsReturned()
    {
        // Arrange
        var subject = Faker.Lorem.Word();
        var content = Faker.Lorem.Sentence();
        var emailMessage = new EmailMessage { Subject = subject, Body = content };
        emailMessage.Recipients.Add(UserEmail);
        await EmailClient.SendEmailAsync(emailMessage);

        // Act
        await Task.Delay(TimeSpan.FromSeconds(1));
        var emailContent = await GetUserEmailContentAsync(new SearchParameters { Subject = subject });

        // Assert
        emailContent.ShouldNotBeNull();
        emailContent.Body.ShouldNotBeNull();
        emailContent.Body.Message.Trim().ShouldBe(content);
        emailContent.Body.ContentType.ShouldBe(EmailContentType.Text);
        emailContent.Attachments.ShouldBeEmpty();
    }

    [Fact]
    public async Task
        Given_EmailSubjectAndHtmlContent_When_GetUserEmailContentAsyncBySubject_Then_HtmlContentIsReturned()
    {
        // Arrange
        var subject = Faker.Lorem.Word();
        var content = Faker.Lorem.Sentence();
        var emailMessage = new EmailMessage { Subject = subject, Body = new EmailBody(content, EmailContentType.Html) };
        emailMessage.Recipients.Add(UserEmail);
        await EmailClient.SendEmailAsync(emailMessage);

        // Act
        await Task.Delay(TimeSpan.FromSeconds(1));
        var emailContent = await GetUserEmailContentAsync(new SearchParameters { Subject = subject });

        // Assert
        emailContent.ShouldNotBeNull();
        emailContent.Body.ShouldNotBeNull();
        emailContent.Body.Message.Trim().ShouldBe(content);
        emailContent.Body.ContentType.ShouldBe(EmailContentType.Html);
        emailContent.Attachments.ShouldBeEmpty();
    }

    [Fact]
    public async Task
        Given_EmailSubjectAndHtmlContent_When_GetUserEmailContentAsyncWithBySubjectDifferentSubject_Then_ContentNotReturned()
    {
        // Arrange
        var subject = Faker.Lorem.Word();
        var content = Faker.Lorem.Sentence();
        var emailMessage = new EmailMessage { Subject = subject, Body = new EmailBody(content, EmailContentType.Html) };
        emailMessage.Recipients.Add(UserEmail);
        await EmailClient.SendEmailAsync(emailMessage);

        // Act
        await Task.Delay(TimeSpan.FromSeconds(1));
        var emailContent = await GetUserEmailContentAsync(new SearchParameters { Subject = subject + "123" });

        // Assert
        emailContent.ShouldBeNull();
    }

    [Fact]
    public async Task
        Given_EmailSubjectAndTextContent_When_GetUserEmailContentAsyncBySender_Then_TextContentIsReturned()
    {
        // Arrange
        var subject = Faker.Lorem.Word();
        var content = Faker.Lorem.Sentence();
        var newEmail = GetNewEmail();
        var emailMessage = new EmailMessage { Subject = subject, Body = content };
        emailMessage.Recipients.Add(UserEmail);
        await SendEmailAsync(newEmail, emailMessage);

        // Act
        await Task.Delay(TimeSpan.FromSeconds(1));
        var emailContent = await GetUserEmailContentAsync(new SearchParameters { SenderEmail = newEmail });

        // Assert
        emailContent.ShouldNotBeNull();
        emailContent.Body.ShouldNotBeNull();
        emailContent.Body.Message.Trim().ShouldBe(content);
        emailContent.Body.ContentType.ShouldBe(EmailContentType.Text);
        emailContent.Attachments.ShouldBeEmpty();
    }

    [Fact]
    public async Task
        Given_EmailSubjectAndTextContent_When_GetUserEmailContentAsyncBySenderWithDifferentEmail_Then_ContentNotReturned()
    {
        // Arrange
        var subject = Faker.Lorem.Random.Int().ToString();
        var content = Faker.Lorem.Sentence();
        var emailMessage = new EmailMessage { Subject = subject, Body = content };
        emailMessage.Recipients.Add(UserEmail);
        await EmailClient.SendEmailAsync(emailMessage);

        // Act
        await Task.Delay(TimeSpan.FromSeconds(1));
        var emailContent =
            await GetUserEmailContentAsync(new SearchParameters { SenderEmail = Faker.Internet.Email() });

        // Assert
        emailContent.ShouldBeNull();
    }

    [Fact]
    public async Task
        Given_EmailSubjectAndTextContent_When_GetUserEmailContentAsyncByDeliveredAfter_Then_TextContentIsReturned()
    {
        // Arrange
        var testTime = DateTime.Now;
        var subject = Faker.Lorem.Random.Int().ToString();
        var content = Faker.Lorem.Sentence();
        var emailMessage = new EmailMessage { Subject = subject, Body = content };
        emailMessage.Recipients.Add(UserEmail);
        await EmailClient.SendEmailAsync(emailMessage);

        // Act
        await Task.Delay(TimeSpan.FromSeconds(1));
        var emailContent =
            await GetUserEmailContentAsync(new SearchParameters { DeliveredAfter = testTime.AddSeconds(-1) });

        // Assert
        emailContent.ShouldNotBeNull();
        emailContent.Body.ShouldNotBeNull();
        emailContent.Body.Message.Trim().ShouldBe(content);
        emailContent.Body.ContentType.ShouldBe(EmailContentType.Text);
        emailContent.Attachments.ShouldBeEmpty();
    }

    [Fact]
    public async Task
        Given_EmailSubjectAndTextContent_When_GetUserEmailContentAsyncByDeliveredAfterWithNextDayTime_Then_ContentNotReturned()
    {
        // Arrange
        var testTime = DateTime.Now;
        var subject = Faker.Lorem.Random.Int().ToString();
        var content = Faker.Lorem.Sentence();
        var emailMessage = new EmailMessage { Subject = subject, Body = content };
        emailMessage.Recipients.Add(UserEmail);
        await EmailClient.SendEmailAsync(emailMessage);

        // Act
        await Task.Delay(TimeSpan.FromSeconds(1));
        var emailContent =
            await GetUserEmailContentAsync(new SearchParameters { DeliveredAfter = testTime.AddDays(1) });

        // Assert
        emailContent.ShouldBeNull();
    }

    [Fact]
    public async Task
        Given_EmailSubjectAndTextContent_When_GetUserEmailContentAsyncByDeliveredAfterWithNextFiveSeconds_Then_ContentNotReturned()
    {
        // Arrange
        var testTime = DateTime.Now;
        var subject = Faker.Lorem.Random.Int().ToString();
        var content = Faker.Lorem.Sentence();
        var emailMessage = new EmailMessage { Subject = subject, Body = content };
        emailMessage.Recipients.Add(UserEmail);
        await EmailClient.SendEmailAsync(emailMessage);

        // Act
        await Task.Delay(TimeSpan.FromSeconds(1));
        var emailContent =
            await GetUserEmailContentAsync(new SearchParameters { DeliveredAfter = testTime.AddSeconds(5) });

        // Assert
        emailContent.ShouldBeNull();
    }
}
