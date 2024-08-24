namespace Domain.Common.People;

public class AddUserRequest(string login, string password)
{
    public string Login { get; set; } = login;
    public string Password { get; set; } = password;
}