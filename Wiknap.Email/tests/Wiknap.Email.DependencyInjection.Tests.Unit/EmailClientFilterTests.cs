using Bogus;
using Microsoft.Extensions.Logging.Testing;
using NSubstitute;
using Shouldly;
using Wiknap.Email.DependencyInjection.Decorators;
using Xunit;

namespace Wiknap.Email.DependencyInjection.Tests.Unit;

public sealed class EmailClientFilterTests
{
    [Fact]
    public async Task Given_ConfigurationWithExcludeAllFalse_When_SendEmailAsync_ShouldSend()
    {
        // Arrange
        var ec = Substitute.For<IEmailClient>();
        var c = Substitute.For<IEmailFilterConfiguration>();
        var l = new FakeLogger<EmailClientFilterDecorator>();
        var filter = new EmailClientFilterDecorator(ec, c, l);
        var faker = new Faker();
        var email = faker.Internet.Email();
        var subject = faker.Lorem.Word();
        var content = faker.Lorem.Sentence();

        // Act
        await filter.SendEmailAsync(email, subject, content);

        //Assert
        await ec.Received(1).SendEmailAsync(email, subject, content);
    }

    [Fact]
    public async Task Given_ConfigurationWithExcludeAllTrue_When_SendEmailAsync_ShouldNotSend()
    {
        // Arrange
        var ec = Substitute.For<IEmailClient>();
        var c = Substitute.For<IEmailFilterConfiguration>();
        c.ExcludeAll.Returns(true);
        var l = new FakeLogger<EmailClientFilterDecorator>();
        var filter = new EmailClientFilterDecorator(ec, c, l);
        var faker = new Faker();
        var email = faker.Internet.Email();
        var subject = faker.Lorem.Word();
        var content = faker.Lorem.Sentence();

        // Act
        await filter.SendEmailAsync(email, subject, content);

        //Assert
        await ec.DidNotReceive().SendEmailAsync(email, subject, content);
    }

    [Fact]
    public async Task When_GetEmailContentAsync_ShouldReturnContent()
    {
        // Arrange
        var faker = new Faker();
        var ec = Substitute.For<IEmailClient>();
        var content = faker.Lorem.Sentence();
        ec.GetEmailContentAsync(Arg.Any<SearchParameters>()).Returns(content);
        var c = Substitute.For<IEmailFilterConfiguration>();
        var l = new FakeLogger<EmailClientFilterDecorator>();
        var filter = new EmailClientFilterDecorator(ec, c, l);
        var email = faker.Internet.Email();
        var parameters = new SearchParameters { SenderEmail = email };

        // Act
        var result = await filter.GetEmailContentAsync(parameters);

        //Assert
        result.ShouldBe(content);
        await ec.Received(1).GetEmailContentAsync(parameters);
    }
}