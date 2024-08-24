using Infra.Http.AFMail.Requests;

namespace Infra.Http.AFMail;

public interface IMailService
{
    public Task SendMail(string authToken, MailRequest request);
}