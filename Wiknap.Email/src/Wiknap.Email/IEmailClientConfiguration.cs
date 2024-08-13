namespace Wiknap.Email;

public interface IEmailClientConfiguration
{
    public string SmtpHost { get; }
    public int SmtpPort { get; }
    public string ImapHost { get; }
    public int ImapPort { get; }
    public string Login { get; }
    public string Password { get; }
    public string? SenderEmail { get; }
    public string SenderName { get; }
}
