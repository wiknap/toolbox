namespace Wiknap.Email.Tests.Integration.Fixture;

internal sealed class TestEmailClientConfiguration : IEmailClientConfiguration
{
    public TestEmailClientConfiguration(string smtpHost, int smtpPort, string imapHost, int imapPort, string login,
        string password)
    {
        SmtpHost = smtpHost;
        SmtpPort = smtpPort;
        ImapHost = imapHost;
        ImapPort = imapPort;
        Login = login;
        Password = password;
    }

    public string SmtpHost { get; }
    public int SmtpPort { get; }
    public string ImapHost { get; }
    public int ImapPort { get; }
    public string Login { get; }
    public string Password { get; }
    public string SenderName => "Admin";
}
