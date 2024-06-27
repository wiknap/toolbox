namespace Wiknap.Email.Models;

public record Recipient(EmailAddress EmailAddress, RecipientType Type = RecipientType.To)
{
    public static implicit operator Recipient(string value) => new(new EmailAddress(value));
}
