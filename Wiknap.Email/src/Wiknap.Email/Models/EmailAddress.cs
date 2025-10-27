namespace Wiknap.Email.Models;

public sealed record EmailAddress(string Email, string? Name = null)
{
    public static implicit operator EmailAddress(string value) => new(value);
}
