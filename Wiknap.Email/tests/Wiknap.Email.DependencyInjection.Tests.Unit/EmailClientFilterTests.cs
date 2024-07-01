using Bogus;

using Microsoft.Extensions.Logging.Testing;

using NSubstitute;

using Shouldly;

using Wiknap.Email.DependencyInjection.Decorators;
using Wiknap.Email.Models;

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
        var message = new EmailMessage { Subject = faker.Lorem.Word(), Body = faker.Lorem.Sentence() };
        message.Recipients.Add(faker.Internet.Email());

        // Act
        await filter.SendEmailAsync(message);

        //Assert
        await ec.Received(1).SendEmailAsync(message);
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
    public async Task Given_ConfigurationWithExcludeAllTrueAndIncludeEmailRule_When_SendEmailAsync_ShouldSend()
    {
        // Arrange
        var ec = Substitute.For<IEmailClient>();
        var faker = new Faker();
        var email = faker.Internet.Email();
        var c = Substitute.For<IEmailFilterConfiguration>();
        c.ExcludeAll.Returns(true);
        c.Include.Returns(new[] { new EmailRule(EmailRuleType.Email, email) });
        var l = new FakeLogger<EmailClientFilterDecorator>();
        var filter = new EmailClientFilterDecorator(ec, c, l);
        var message = new EmailMessage { Subject = faker.Lorem.Word(), Body = faker.Lorem.Sentence() };
        message.Recipients.Add(email);

        // Act
        await filter.SendEmailAsync(message);

        //Assert
        await ec.Received(1).SendEmailAsync(message);
    }

    [Fact]
    public async Task Given_ConfigurationWithExcludeAllTrueAndIncludeDomainRule_When_SendEmailAsync_ShouldSend()
    {
        // Arrange
        var ec = Substitute.For<IEmailClient>();
        var faker = new Faker();
        const string domain = "domain.pl";
        const string email = $"email@{domain}";
        var c = Substitute.For<IEmailFilterConfiguration>();
        c.ExcludeAll.Returns(true);
        c.Include.Returns(new[] { new EmailRule(EmailRuleType.Domain, domain) });
        var l = new FakeLogger<EmailClientFilterDecorator>();
        var filter = new EmailClientFilterDecorator(ec, c, l);
        var message = new EmailMessage { Subject = faker.Lorem.Word(), Body = faker.Lorem.Sentence() };
        message.Recipients.Add(email);

        // Act
        await filter.SendEmailAsync(message);

        //Assert
        await ec.Received(1).SendEmailAsync(message);
    }

    [Fact]
    public async Task Given_ConfigurationWithExcludeAllFalseAndExcludeDomainRule_When_SendEmailAsync_ShouldNotSend()
    {
        // Arrange
        var ec = Substitute.For<IEmailClient>();
        var faker = new Faker();
        const string domain = "domain.pl";
        const string email = $"email@{domain}";
        var c = Substitute.For<IEmailFilterConfiguration>();
        c.Include.Returns(Enumerable.Empty<EmailRule>());
        c.Exclude.Returns(new[] { new EmailRule(EmailRuleType.Domain, domain) });
        var l = new FakeLogger<EmailClientFilterDecorator>();
        var filter = new EmailClientFilterDecorator(ec, c, l);
        var subject = faker.Lorem.Word();
        var content = faker.Lorem.Sentence();

        // Act
        await filter.SendEmailAsync(email, subject, content);

        //Assert
        await ec.DidNotReceive().SendEmailAsync(email, subject, content);
    }

    [Fact]
    public async Task Given_ConfigurationWithExcludeAllFalseAndExcludeEmailRule_When_SendEmailAsync_ShouldSend()
    {
        // Arrange
        var ec = Substitute.For<IEmailClient>();
        var faker = new Faker();
        var email = faker.Internet.Email();
        var c = Substitute.For<IEmailFilterConfiguration>();
        c.Include.Returns(Enumerable.Empty<EmailRule>());
        c.Exclude.Returns(new[] { new EmailRule(EmailRuleType.Email, email) });
        var l = new FakeLogger<EmailClientFilterDecorator>();
        var filter = new EmailClientFilterDecorator(ec, c, l);
        var subject = faker.Lorem.Word();
        var content = faker.Lorem.Sentence();

        // Act
        await filter.SendEmailAsync(email, subject, content);

        //Assert
        await ec.DidNotReceive().SendEmailAsync(email, subject, content);
    }

    [Fact]
    public async Task
        Given_ConfigurationWithExcludeAllFalseAndExcludeDomainRuleAndIncludeEmailRule_When_SendEmailAsync_ShouldSend()
    {
        // Arrange
        var ec = Substitute.For<IEmailClient>();
        var faker = new Faker();
        const string domain = "domain.pl";
        const string email = $"email@{domain}";
        var c = Substitute.For<IEmailFilterConfiguration>();
        c.Include.Returns(new[] { new EmailRule(EmailRuleType.Email, email) });
        c.Exclude.Returns(new[] { new EmailRule(EmailRuleType.Domain, domain) });
        var l = new FakeLogger<EmailClientFilterDecorator>();
        var filter = new EmailClientFilterDecorator(ec, c, l);
        var message = new EmailMessage { Subject = faker.Lorem.Word(), Body = faker.Lorem.Sentence() };
        message.Recipients.Add(email);

        // Act
        await filter.SendEmailAsync(message);

        //Assert
        await ec.Received(1).SendEmailAsync(message);
    }

    [Fact]
    public async Task When_GetEmailContentAsync_ShouldReturnContent()
    {
        // Arrange
        var faker = new Faker();
        var ec = Substitute.For<IEmailClient>();
        var content = new EmailContent(null);
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
