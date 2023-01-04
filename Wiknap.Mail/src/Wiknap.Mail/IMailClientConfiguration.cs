namespace Wiknap.Mail;

public interface IMailClientConfiguration
{
    public string SmtpHost { get; }
    public int SmtpPort { get; }
    public string ImapHost { get; }
    public int ImapPort { get; }
    public string Login { get; }
    public string Password { get; }
    public string SenderName { get; }
}