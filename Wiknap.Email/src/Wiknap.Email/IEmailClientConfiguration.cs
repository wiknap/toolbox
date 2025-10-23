namespace Wiknap.Email;

public interface IEmailClientConfiguration
{
    string SmtpHost { get; }
    int SmtpPort { get; }
    string ImapHost { get; }
    int ImapPort { get; }
    string Login { get; }
    string Password { get; }
    string? SenderEmail { get; }
    string SenderName { get; }
}
