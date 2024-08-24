using Domain.Entities.Enums;
using static System.String;

namespace Domain.Common.People;

public class CreatePersonRequest
{
    public string Login { get; set; } = Empty;
    public string Password { get; set; } = Empty;
    public string Name { get; set; } = Empty;
    public string Email { get; set; } = Empty;
    public string? PersonCode { get; set; }
    public string? Phone { get; set; }
    public DateTime? Birthday { get; set; }
    public Gender Gender { get; set; }
}