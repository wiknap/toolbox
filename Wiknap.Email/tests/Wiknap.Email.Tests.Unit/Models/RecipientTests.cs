using Shouldly;

using Wiknap.Email.Models;

using Xunit;

namespace Wiknap.Email.Tests.Unit.Models;

public sealed class RecipientTests : TestsBase
{
    [Fact]
    public void Given_ValidEmail_When_Constructor_Then_ValidRecipientWithEmailAndEmptyNameAndTypeToCreated()
    {
        // Arrange
        var email = Faker.Internet.Email();

        // Act
        var recipient = new Recipient(email);

        // Assert
        recipient.EmailAddress.Email.ShouldBe(email);
        recipient.EmailAddress.Name.ShouldBeNull();
        recipient.Type.ShouldBe(RecipientType.To);
    }

    [Fact]
    public void Given_ValidEmailAndRecipientTypeCc_When_Constructor_Then_ValidRecipientWithEmailAndEmptyNameAndTypeCcCreated()
    {
        // Arrange
        var email = Faker.Internet.Email();

        // Act
        var recipient = new Recipient(email, RecipientType.Cc);

        // Assert
        recipient.EmailAddress.Email.ShouldBe(email);
        recipient.EmailAddress.Name.ShouldBeNull();
        recipient.Type.ShouldBe(RecipientType.Cc);
    }

    [Fact]
    public void Given_ValidEmailAndNameAndRecipientTypeBcc_When_Constructor_Then_ValidRecipientWithEmailAndNameAndTypeBccCreated()
    {
        // Arrange
        var email = Faker.Internet.Email();
        var name = Faker.Person.FullName;

        // Act
        var recipient = new Recipient(new EmailAddress(email, name), RecipientType.Bcc);

        // Assert
        recipient.EmailAddress.Email.ShouldBe(email);
        recipient.EmailAddress.Name.ShouldBe(name);
        recipient.Type.ShouldBe(RecipientType.Bcc);
    }

    [Fact]
    public void Given_ValidEmail_When_ImplicitOperator_Then_ValidRecipientWithEmailAndEmptyNameAndTypeToCreated()
    {
        // Arrange
        var email = Faker.Internet.Email();

        // Act
        Recipient recipient = email;

        // Assert
        recipient.EmailAddress.Email.ShouldBe(email);
        recipient.EmailAddress.Name.ShouldBeNull();
        recipient.Type.ShouldBe(RecipientType.To);
    }
}
