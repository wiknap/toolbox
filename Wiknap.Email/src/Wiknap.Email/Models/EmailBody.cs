namespace Wiknap.Email.Models;

public record EmailBody(string Message, EmailContentType ContentType = EmailContentType.Text)
{
    public static implicit operator EmailBody(string value) => new(value);
}
