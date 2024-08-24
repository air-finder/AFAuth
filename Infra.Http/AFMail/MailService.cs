using System.Net.Http.Headers;
using System.Text;
using Infra.Http.AFMail.Requests;
using Newtonsoft.Json;

namespace Infra.Http.AFMail;

public class MailService(HttpClient _http) : IMailService
{
    public async Task SendMail(string authToken, MailRequest request)
    {
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
        await _http.PostAsync("api/mail", content);
    }
}