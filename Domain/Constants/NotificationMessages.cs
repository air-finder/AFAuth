namespace Domain.Constants;

public static class NotificationMessages
{
    public static string InvalidCredentials = "Invalid credentials!";
    public static string UnauthorizedAction = "Unauthorized action!";
    public static string AlreadyRegistered(string name)
    {
        return name + " is already registered!";
    }
    public static string NotEmptyEntity(string name)
    {
        return name + " can't be empty!";
    }

    public static string NotFoundEntity(string name)
    {
        return name + " not found!";
    }
}