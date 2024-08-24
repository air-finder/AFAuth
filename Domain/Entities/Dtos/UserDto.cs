using Domain.Entities.Enums;

namespace Domain.Entities.Dtos;

public class UserDto(Guid id, Guid personId, string login, UserRole role)
{
    public Guid Id { get; set; } = id;
    public Guid PersonId { get; set; } = personId;
    public string Login { get; set; } = login;
    public UserRole Role { get; set; } = role;

    public static implicit operator UserDto(User user)
    {
        return new UserDto(user.Id, user.PersonId, user.Login, user.Role);
    }
}