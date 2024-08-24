using static System.String;

namespace Infra.Http.AFMail.Requests;
public class MailRequest(List<string> toMail, string subject, string body)
{
    public List<string> ToMail { get; set; } = toMail;
    public string Subject { get; set; } = subject;
    public string Body { get; set; } = body;
}