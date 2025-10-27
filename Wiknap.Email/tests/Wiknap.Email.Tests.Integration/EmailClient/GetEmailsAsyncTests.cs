using Shouldly;

using Wiknap.Email.Models;
using Wiknap.Email.Tests.Integration.Fixture;

using Xunit;

namespace Wiknap.Email.Tests.Integration.EmailClient;

public sealed class GetEmailsAsyncTests : IntegrationTestsBase
{
    public GetEmailsAsyncTests(EmailServer emailServer) : base(emailServer)
    {
    }

    [Fact]
    public async Task Given_ThreeEmails_When_GetEmailsAsyncBySubject_Then_ThreeEmails()
    {
        // Arrange
        var subject = Faker.Lorem.Word();
        var emailMessage1 = new EmailMessage { Subject = subject, Body = Faker.Lorem.Sentence() };
        emailMessage1.Recipients.Add(UserEmail);
        var emailMessage2 = new EmailMessage { Subject = subject, Body = Faker.Lorem.Sentence() };
        emailMessage2.Recipients.Add(UserEmail);
        var emailMessage3 = new EmailMessage { Subject = subject, Body = Faker.Lorem.Sentence() };
        emailMessage3.Recipients.Add(UserEmail);
        await EmailClient.SendEmailAsync(emailMessage1);
        await EmailClient.SendEmailAsync(emailMessage2);
        await EmailClient.SendEmailAsync(emailMessage3);

        // Act
        await Task.Delay(TimeSpan.FromSeconds(1));
        var messages = await GetEmailsAsync(new SearchParameters { Subject = subject }, 3);

        // Assert
        messages.Count.ShouldBe(3);
        messages.ShouldContain(m =>
            m.Subject == subject &&
            m.Content.Body!.ContentType == EmailContentType.Text &&
            m.Content.Body.Message.Trim() == emailMessage3.Body.Message &&
            m.Content.Attachments.Count == 0);
        messages.ShouldContain(m =>
            m.Subject == subject &&
            m.Content.Body!.ContentType == EmailContentType.Text &&
            m.Content.Body.Message.Trim() == emailMessage2.Body.Message &&
            m.Content.Attachments.Count == 0);
        messages.ShouldContain(m =>
            m.Subject == subject &&
            m.Content.Body!.ContentType == EmailContentType.Text &&
            m.Content.Body.Message.Trim() == emailMessage1.Body.Message &&
            m.Content.Attachments.Count == 0);
    }

    [Fact]
    public async Task Given_ThreeEmailsWithOneWithDifferentSubject_When_GetEmailsAsyncBySubject_Then_TwoEmails()
    {
        // Arrange
        var subject = Faker.Lorem.Random.Int().ToString();
        var emailMessage1 = new EmailMessage { Subject = subject, Body = Faker.Lorem.Sentence() };
        emailMessage1.Recipients.Add(UserEmail);
        var emailMessage2 =
            new EmailMessage { Subject = Faker.Lorem.Random.Int().ToString(), Body = Faker.Lorem.Sentence() };
        emailMessage2.Recipients.Add(UserEmail);
        var emailMessage3 = new EmailMessage { Subject = subject, Body = Faker.Lorem.Sentence() };
        emailMessage3.Recipients.Add(UserEmail);
        await EmailClient.SendEmailAsync(emailMessage1);
        await EmailClient.SendEmailAsync(emailMessage2);
        await EmailClient.SendEmailAsync(emailMessage3);

        // Act
        await Task.Delay(TimeSpan.FromSeconds(1));
        var messages = await GetEmailsAsync(new SearchParameters { Subject = subject }, 2);

        // Assert
        messages.Count.ShouldBe(2);
        messages.ShouldContain(m =>
            m.Subject == subject &&
            m.Content.Body!.ContentType == EmailContentType.Text &&
            m.Content.Body.Message.Trim() == emailMessage3.Body.Message &&
            m.Content.Attachments.Count == 0);
        messages.ShouldContain(m =>
            m.Subject == subject &&
            m.Content.Body!.ContentType == EmailContentType.Text &&
            m.Content.Body.Message.Trim() == emailMessage1.Body.Message &&
            m.Content.Attachments.Count == 0);
    }

    [Fact]
    public async Task Given_ThreeEmails_When_GetEmailsAsyncByEmail_Then_ThreeEmails()
    {
        // Arrange
        var newEmail = GetNewEmail();
        var emailMessage1 =
            new EmailMessage { Subject = Faker.Lorem.Random.Int().ToString(), Body = Faker.Lorem.Sentence() };
        emailMessage1.Recipients.Add(UserEmail);
        var emailMessage2 =
            new EmailMessage { Subject = Faker.Lorem.Random.Int().ToString(), Body = Faker.Lorem.Sentence() };
        emailMessage2.Recipients.Add(UserEmail);
        var emailMessage3 =
            new EmailMessage { Subject = Faker.Lorem.Random.Int().ToString(), Body = Faker.Lorem.Sentence() };
        emailMessage3.Recipients.Add(UserEmail);
        await SendEmailAsync(newEmail, emailMessage1);
        await SendEmailAsync(newEmail, emailMessage2);
        await SendEmailAsync(newEmail, emailMessage3);

        // Act
        await Task.Delay(TimeSpan.FromSeconds(1));
        var messages = await GetEmailsAsync(new SearchParameters { SenderEmail = newEmail }, 3);

        // Assert
        messages.Count.ShouldBe(3);
        messages.ShouldContain(m =>
            m.Subject == emailMessage3.Subject &&
            m.Content.Body!.ContentType == EmailContentType.Text &&
            m.Content.Body.Message.Trim() == emailMessage3.Body.Message &&
            m.Content.Attachments.Count == 0);
        messages.ShouldContain(m =>
            m.Subject == emailMessage2.Subject &&
            m.Content.Body!.ContentType == EmailContentType.Text &&
            m.Content.Body.Message.Trim() == emailMessage2.Body.Message &&
            m.Content.Attachments.Count == 0);
        messages.ShouldContain(m =>
            m.Subject == emailMessage1.Subject &&
            m.Content.Body!.ContentType == EmailContentType.Text &&
            m.Content.Body.Message.Trim() == emailMessage1.Body.Message &&
            m.Content.Attachments.Count == 0);
    }

    [Fact]
    public async Task Given_ThreeEmailsWithOneDifferentEmail_When_GetEmailsAsyncByEmail_Then_TwoEmails()
    {
        // Arrange
        var newEmail = GetNewEmail();
        var emailMessage1 =
            new EmailMessage { Subject = Faker.Lorem.Random.Int().ToString(), Body = Faker.Lorem.Sentence() };
        emailMessage1.Recipients.Add(UserEmail);
        var emailMessage2 =
            new EmailMessage { Subject = Faker.Lorem.Random.Int().ToString(), Body = Faker.Lorem.Sentence() };
        emailMessage2.Recipients.Add(UserEmail);
        var emailMessage3 =
            new EmailMessage { Subject = Faker.Lorem.Random.Int().ToString(), Body = Faker.Lorem.Sentence() };
        emailMessage3.Recipients.Add(UserEmail);
        await SendEmailAsync(newEmail, emailMessage1);
        await EmailClient.SendEmailAsync(emailMessage2);
        await SendEmailAsync(newEmail, emailMessage3);

        // Act
        await Task.Delay(TimeSpan.FromSeconds(1));
        var messages = await GetEmailsAsync(new SearchParameters { SenderEmail = newEmail }, 2);

        // Assert
        messages.Count.ShouldBe(2);
        messages.ShouldContain(m =>
            m.Subject == emailMessage3.Subject &&
            m.Content.Body!.ContentType == EmailContentType.Text &&
            m.Content.Body.Message.Trim() == emailMessage3.Body.Message &&
            m.Content.Attachments.Count == 0);
        messages.ShouldContain(m =>
            m.Subject == emailMessage1.Subject &&
            m.Content.Body!.ContentType == EmailContentType.Text &&
            m.Content.Body.Message.Trim() == emailMessage1.Body.Message &&
            m.Content.Attachments.Count == 0);
    }

    [Fact]
    public async Task Given_ThreeEmails_When_GetEmailsAsyncByDeliveryAfter_Then_ThreeEmails()
    {
        // Arrange
        await Task.Delay(TimeSpan.FromSeconds(1));
        var now = DateTime.Now;
        await Task.Delay(TimeSpan.FromSeconds(1));
        var emailMessage1 =
            new EmailMessage { Subject = Faker.Lorem.Random.Int().ToString(), Body = Faker.Lorem.Sentence() };
        emailMessage1.Recipients.Add(UserEmail);
        var emailMessage2 =
            new EmailMessage { Subject = Faker.Lorem.Random.Int().ToString(), Body = Faker.Lorem.Sentence() };
        emailMessage2.Recipients.Add(UserEmail);
        var emailMessage3 =
            new EmailMessage { Subject = Faker.Lorem.Random.Int().ToString(), Body = Faker.Lorem.Sentence() };
        emailMessage3.Recipients.Add(UserEmail);
        await EmailClient.SendEmailAsync(emailMessage1);
        await EmailClient.SendEmailAsync(emailMessage2);
        await EmailClient.SendEmailAsync(emailMessage3);

        // Act
        await Task.Delay(TimeSpan.FromSeconds(1));
        var messages = await GetEmailsAsync(new SearchParameters { DeliveredAfter = now }, 3);

        // Assert
        messages.Count.ShouldBe(3);
        messages.ShouldContain(m =>
            m.Subject == emailMessage3.Subject &&
            m.Content.Body!.ContentType == EmailContentType.Text &&
            m.Content.Body.Message.Trim() == emailMessage3.Body.Message &&
            m.Content.Attachments.Count == 0);
        messages.ShouldContain(m =>
            m.Subject == emailMessage2.Subject &&
            m.Content.Body!.ContentType == EmailContentType.Text &&
            m.Content.Body.Message.Trim() == emailMessage2.Body.Message &&
            m.Content.Attachments.Count == 0);
        messages.ShouldContain(m =>
            m.Subject == emailMessage1.Subject &&
            m.Content.Body!.ContentType == EmailContentType.Text &&
            m.Content.Body.Message.Trim() == emailMessage1.Body.Message &&
            m.Content.Attachments.Count == 0);
    }

    [Fact]
    public async Task Given_ThreeEmailsOneMailBefore_When_GetEmailsAsyncByDeliveryAfter_Then_NoEmails()
    {
        // Arrange
        var emailMessage1 =
            new EmailMessage { Subject = Faker.Lorem.Random.Int().ToString(), Body = Faker.Lorem.Sentence() };
        emailMessage1.Recipients.Add(UserEmail);
        var emailMessage2 =
            new EmailMessage { Subject = Faker.Lorem.Random.Int().ToString(), Body = Faker.Lorem.Sentence() };
        emailMessage2.Recipients.Add(UserEmail);
        var emailMessage3 =
            new EmailMessage { Subject = Faker.Lorem.Random.Int().ToString(), Body = Faker.Lorem.Sentence() };
        emailMessage3.Recipients.Add(UserEmail);
        await EmailClient.SendEmailAsync(emailMessage1);
        await Task.Delay(TimeSpan.FromSeconds(1));
        var now = DateTime.Now;
        await Task.Delay(TimeSpan.FromSeconds(1));
        await EmailClient.SendEmailAsync(emailMessage2);
        await EmailClient.SendEmailAsync(emailMessage3);

        // Act
        await Task.Delay(TimeSpan.FromSeconds(1));
        var messages = await GetEmailsAsync(new SearchParameters { DeliveredAfter = now }, 2);

        // Assert
        messages.Count.ShouldBe(2);
        messages.ShouldContain(m =>
            m.Subject == emailMessage3.Subject &&
            m.Content.Body!.ContentType == EmailContentType.Text &&
            m.Content.Body.Message.Trim() == emailMessage3.Body.Message &&
            m.Content.Attachments.Count == 0);
        messages.ShouldContain(m =>
            m.Subject == emailMessage2.Subject &&
            m.Content.Body!.ContentType == EmailContentType.Text &&
            m.Content.Body.Message.Trim() == emailMessage2.Body.Message &&
            m.Content.Attachments.Count == 0);
    }
}
