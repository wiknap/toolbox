using Shouldly;

using Xunit;

namespace Wiknap.Testcontainers.GreenMail.Tests.Unit;

public sealed class GreenMailBuilderTests
{
    private readonly GreenMailBuilder _builder = new GreenMailBuilder().WithDockerEndpoint("https://localhost");

    [Fact]
    public void When_CreateWithDefaultConstructor_Then_BuildsContainerWithDefaultValues()
    {
        // Act
        var container = _builder.Build();

        // Assert
        container.Image.Repository.ShouldBe("greenmail/standalone");
        container.Image.Tag.ShouldBe("2.1.14");
        container.AuthenticationDisabled.ShouldBeTrue();
        container.Users.ShouldBeEmpty();
    }

    [Fact]
    public void When_CreateWithConfigConstructorAndOverrideDefaults_Then_BuildsContainerWithNewValues()
    {
        // Arrange
        const string repo = "repo/image";
        const string version = "1.0";
        var firstUser = new GreenMailUser("first@example.com", "pass123");
        var secondUser = new GreenMailUser("second@example.com", "pass321");

        // Act
        var container = _builder
            .WithAuthenticationDisabled(false)
            .WithUser(firstUser.Email, firstUser.Password)
            .WithUser(secondUser.Email, secondUser.Password)
            .WithImage($"{repo}:{version}")
            .Build();

        // Assert
        container.Image.Repository.ShouldBe(repo);
        container.Image.Tag.ShouldBe(version);
        container.AuthenticationDisabled.ShouldBeFalse();
        container.Users.ShouldBe([firstUser, secondUser]);
    }
}
