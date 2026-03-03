namespace Wiknap.Email.Models;

public sealed record ReceivedEmailMessage(string? Subject, EmailContent Content);
