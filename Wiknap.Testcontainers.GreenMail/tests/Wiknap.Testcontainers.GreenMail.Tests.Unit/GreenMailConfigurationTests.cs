using Docker.DotNet.Models;

using DotNet.Testcontainers.Configurations;

using Shouldly;

using Xunit;

namespace Wiknap.Testcontainers.GreenMail.Tests.Unit;

public sealed class GreenMailConfigurationTests
{
    private static readonly GreenMailUser User = new("user@example.com", "pass321");

    [Fact]
    public void When_CreateWithDefaultConstructor_Then_ConfigInitialized()
    {
        // Act
        var config = new GreenMailConfiguration();

        // Assert
        config.AuthenticationDisabled.ShouldBeNull();
        config.Users.ShouldBeNull();
    }

    [Fact]
    public void When_CreateWithConfigValues_Then_ConfigInitialized()
    {
        // Act
        var config = new GreenMailConfiguration(false, [User]);

        // Assert
        config.AuthenticationDisabled.ShouldBe(false);
        config.Users.ShouldBe([User]);
    }

    [Fact]
    public void When_CreateWithResourceConfiguration_Then_ConfigInitialized()
    {
        // Act
        var config = new GreenMailConfiguration(new ResourceConfiguration<CreateContainerParameters>());

        // Assert
        config.AuthenticationDisabled.ShouldBeNull();
        config.Users.ShouldBeNull();
    }

    [Fact]
    public void When_CreateWithContainerConfiguration_Then_ConfigInitialized()
    {
        // Act
        var config = new GreenMailConfiguration(new ContainerConfiguration());

        // Assert
        config.AuthenticationDisabled.ShouldBeNull();
        config.Users.ShouldBeNull();
    }

    [Fact]
    public void When_CreateWithGreenMailContainerConfiguration_Then_ConfigInitialized()
    {
        // Arrange
        var newConfig = new GreenMailConfiguration(false, [User]);

        // Act
        var config = new GreenMailConfiguration(newConfig);

        // Assert
        config.AuthenticationDisabled.ShouldBe(false);
        config.Users.ShouldBe([User]);
    }

    [Fact]
    public void When_CreateWithGreenMailContainerConfigurations_Then_ConfigCombined()
    {
        // Arrange
        var otherUser = new GreenMailUser("other@example.com", "pass123");
        var oldConfig = new GreenMailConfiguration(true, [User]);
        var newConfig = new GreenMailConfiguration(false, [otherUser]);

        // Act
        var config = new GreenMailConfiguration(oldConfig, newConfig);

        // Assert
        config.AuthenticationDisabled.ShouldBe(false);
        config.Users.ShouldBe([User, otherUser]);
    }
}
