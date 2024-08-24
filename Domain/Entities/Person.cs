using Domain.Common.People;
using Domain.Constants;
using Domain.Entities.Enums;
using Domain.SeedWork.Notification;

namespace Domain.Entities;

public class Person : BaseEntity
{
    public Person() {}
    public Person(string name, string email, DateTime? birthday, string? personalCode, Gender gender, string? phone)
    {
        if (string.IsNullOrWhiteSpace(name)) NotificationsWrapper.AddNotification(NotificationMessages.NotEmptyEntity(nameof(Name)));
        if (string.IsNullOrWhiteSpace(email)) NotificationsWrapper.AddNotification(NotificationMessages.NotEmptyEntity(nameof(Email)));
        if (!string.IsNullOrWhiteSpace(personalCode) && !IsValidCPF(personalCode)) NotificationsWrapper.AddNotification(NotificationMessages.InvalidEntity("CPF"));
        if (!string.IsNullOrWhiteSpace(personalCode) && !IsValidPhone(phone)) NotificationsWrapper.AddNotification(NotificationMessages.InvalidEntity(nameof(Phone)));
        CheckEntity();

        Name = name;
        Email = email;
        Birthday = birthday;
        PersonalCode = personalCode;
        Gender = gender;
        Phone = phone;
        Users = [];
    }

    public string Name { get; private set; }
    public string Email { get; private set; }
    public DateTime? Birthday { get; private set; }
    public string? PersonalCode { get; init; }
    public Gender Gender { get; private set; }
    public string? Phone { get; private set; }
    
    #region Mapping
    public virtual List<User> Users { get; private set; }
    #endregion
  
    public void AddUser(User user)
    {
        user.PersonId = Id;
        Users.Add(user);
    }

    public static implicit operator Person(CreatePersonRequest request)
    {
        var person = new Person(
            request.Name,
            request.Email,
            request.Birthday,
            request.PersonCode,
            request.Gender,
            request.Phone
        );
        person.AddUser(new User(request.Login, request.Password));
        return person;
    }

    private bool IsValidCPF(string cpf)
    {
        int[] m1 = [10, 9, 8, 7, 6, 5, 4, 3, 2];
        int[] m2 = [11, 10, 9, 8, 7, 6, 5, 4, 3, 2];

        cpf = cpf.Trim().Replace(".", "").Replace("-", "");
        if (cpf.Length != 11)
            return false;

        for (int j = 0; j < 10; j++)
            if (j.ToString().PadLeft(11, char.Parse(j.ToString())) == cpf)
                return false;

        var tempCpf = cpf.Substring(0, 9);
        var acc = 0;

        for (var i = 0; i < 9; i++)
            acc += int.Parse(tempCpf[i].ToString()) * m1[i];

        var rest = acc % 11;
        if (rest < 2)
            rest = 0;
        else
            rest = 11 - rest;

        var digito = rest.ToString();
        tempCpf += digito;
        acc = 0;
        for (var i = 0; i < 10; i++)
            acc += int.Parse(tempCpf[i].ToString()) * m2[i];

        rest = acc % 11;
        if (rest < 2)
            rest = 0;
        else
            rest = 11 - rest;

        digito += rest.ToString();

        return cpf.EndsWith(digito);
    }
    private bool IsValidPhone(string? phone)
    {
        throw new NotImplementedException();
    }
}