using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;
using NSubstitute;
using Shouldly;
using Wiknap.Email.DependencyInjection.Decorators;
using Xunit;

namespace Wiknap.Email.DependencyInjection.Tests.Unit;

public sealed class AddEmailTests
{
    [Fact]
    public void When_SendEmailAsync_ShouldAddRequiredServices()
    {
        // Arrange
        var sc = new ServiceCollection();
        var ec = Substitute.For<IEmailClientConfiguration>();
        var fc = Substitute.For<IEmailFilterConfiguration>();
        var fl = new FakeLogger<EmailClientFilterDecorator>();
        var ll = new FakeLogger<EmailClientLoggingDecorator>();

        // Act
        sc
            .AddEmail()
            .AddScoped<IEmailClientConfiguration>(_ => ec)
            .AddScoped<IEmailFilterConfiguration>(_ => fc)
            .AddScoped<ILogger<EmailClientFilterDecorator>>(_ => fl)
            .AddScoped<ILogger<EmailClientLoggingDecorator>>(_ => ll);

        //Assert
        var sp = sc.BuildServiceProvider();
        var emailClient = sp.GetService<IEmailClient>();
        emailClient.ShouldNotBeNull();
        emailClient.ShouldBeOfType(typeof(EmailClientFilterDecorator));
    }
}