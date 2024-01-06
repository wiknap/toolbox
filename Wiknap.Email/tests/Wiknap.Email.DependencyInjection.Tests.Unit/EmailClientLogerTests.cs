using Bogus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;
using NSubstitute;
using Shouldly;
using Wiknap.Email.DependencyInjection.Decorators;
using Xunit;

namespace Wiknap.Email.DependencyInjection.Tests.Unit;

public sealed class EmailClientLoggerTests
{
    [Fact]
    public async Task When_SendEmailAsync_ShouldLog()
    {
        // Arrange
        var ec = Substitute.For<IEmailClient>();
        var l = new FakeLogger<EmailClientLoggingDecorator>();
        var logger = new EmailClientLoggingDecorator(ec, l);
        var faker = new Faker();
        var email = faker.Internet.Email();
        var subject = faker.Lorem.Word();
        var content = faker.Lorem.Sentence();

        // Act
        await logger.SendEmailAsync(email, subject, content);

        //Assert
        await ec.Received(1).SendEmailAsync(email, subject, content);
        var records = l.Collector.GetSnapshot();
        records.ShouldNotBeEmpty();
        records.Count.ShouldBe(2);
        records.ShouldAllBe(r => r.Level == LogLevel.Trace);
        records.ShouldContain(r => r.Message == $"Sending email to {email}");
        records.ShouldContain(r => r.Message == $"Email sent to {email}");
    }

    [Fact]
    public async Task Give_ExistingSearchParameters_When_GetEmailContentAsync_ShouldLog()
    {
        // Arrange
        var faker = new Faker();
        var ec = Substitute.For<IEmailClient>();
        var content = faker.Lorem.Sentence();
        ec.GetEmailContentAsync(Arg.Any<SearchParameters>()).Returns(content);
        var l = new FakeLogger<EmailClientLoggingDecorator>();
        var logger = new EmailClientLoggingDecorator(ec, l);
        var email = faker.Internet.Email();
        var parameters = new SearchParameters { SenderEmail = email };

        // Act
        var result = await logger.GetEmailContentAsync(parameters);

        //Assert
        result.ShouldBe(content);
        await ec.Received(1).GetEmailContentAsync(parameters);
        var records = l.Collector.GetSnapshot();
        records.ShouldNotBeEmpty();
        records.Count.ShouldBe(2);
        records.ShouldAllBe(r => r.Level == LogLevel.Trace);
        records.ShouldContain(r => r.Message == "Searching for email");
        records.ShouldContain(r => r.Message == "Email found");
    }
    
    [Fact]
    public async Task Give_NonExistingSearchParameters_When_GetEmailContentAsync_ShouldLog()
    {
        // Arrange
        var faker = new Faker();
        var ec = Substitute.For<IEmailClient>();
        ec.GetEmailContentAsync(Arg.Any<SearchParameters>()).Returns(default(string?));
        var l = new FakeLogger<EmailClientLoggingDecorator>();
        var logger = new EmailClientLoggingDecorator(ec, l);
        var email = faker.Internet.Email();
        var parameters = new SearchParameters { SenderEmail = email };

        // Act
        var result = await logger.GetEmailContentAsync(parameters);

        //Assert
        result.ShouldBeNull();
        await ec.Received(1).GetEmailContentAsync(parameters);
        var records = l.Collector.GetSnapshot();
        records.ShouldNotBeEmpty();
        records.Count.ShouldBe(2);
        records.ShouldAllBe(r => r.Level == LogLevel.Trace);
        records.ShouldContain(r => r.Message == "Searching for email");
        records.ShouldContain(r => r.Message == "Email not found");
    }
}