using Shouldly;

using Wiknap.Email.Tests.Integration.Fixture;

using Xunit;

namespace Wiknap.Email.Tests.Integration.EmailClient;

public sealed class SendEmailAsyncTests : IntegrationTestsBase
{
    public SendEmailAsyncTests(EmailServer emailServer) : base(emailServer)
    {
    }

    [Fact]
    public async Task Given_EmailSubjectAndTextContent_When_SendEmailAsync_Then_EmailIsSent()
    {
        // Arrange
        var subject = Faker.Lorem.Word();
        var content = Faker.Lorem.Sentence();

        // Act
        await EmailClient.SendEmailAsync(UserEmail, subject, content, ct: Cts.Token);

        // Assert
        var message = await GetUserEmailContentAsync(new SearchParameters { Subject = subject });
        message.ShouldNotBeNull();
        message.Trim().ShouldBe(content);
    }

    [Fact]
    public async Task Given_EmailSubjectAndHtmlContent_When_SendEmailAsync_Then_EmailIsSent()
    {
        // Arrange
        var subject = Faker.Lorem.Word();
        var content = Faker.Lorem.Sentence();

        // Act
        await EmailClient.SendEmailAsync(UserEmail, subject, content, true, ct: Cts.Token);

        // Assert
        var message = await GetUserEmailContentAsync(new SearchParameters { Subject = subject });
        message.ShouldNotBeNull();
        message.Trim().ShouldBe(content);
    }
}
