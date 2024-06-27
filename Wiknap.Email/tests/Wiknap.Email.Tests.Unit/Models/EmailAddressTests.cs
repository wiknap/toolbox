using Shouldly;

using Wiknap.Email.Models;

using Xunit;

namespace Wiknap.Email.Tests.Unit.Models;

public sealed class EmailAddressTests : TestsBase
{
    [Fact]
    public void Given_ValidEmail_When_Constructor_Then_ValidEmailAddressCreatedWithEmptyName()
    {
        // Arrange
        var email = Faker.Internet.Email();

        // Act
        var address = new EmailAddress(email);

        // Assert
        address.Email.ShouldBe(email);
        address.Name.ShouldBeNull();
    }

    [Fact]
    public void Given_ValidEmailAndName_When_Constructor_Then_ValidEmailAddressCreated()
    {
        // Arrange
        var email = Faker.Internet.Email();
        var name = Faker.Person.FullName;

        // Act
        var address = new EmailAddress(email, name);

        // Assert
        address.Email.ShouldBe(email);
        address.Name.ShouldBe(name);
    }

    [Fact]
    public void Given_ValidEmail_When_ImplicitOperator_Then_ValidEmailAddressCreated()
    {
        // Arrange
        var email = Faker.Internet.Email();

        // Act
        EmailAddress address = email;

        // Assert
        address.Email.ShouldBe(email);
        address.Name.ShouldBeNull();
    }
}
