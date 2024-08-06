using Application.Interfaces;
using Domain.Common;
using Domain.Common.People;
using Domain.Constants;
using Domain.Entities;
using Domain.Entities.Dtos;
using Domain.Exceptions;
using Domain.Repositories;
using Domain.SeedWork.Notification;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class PeopleService(IPeopleRepository userRepository) : BaseCrudService<Person>(userRepository), IPeopleService
{
    public async Task<BaseResponse<object>> CreateAsync(CreatePersonRequest request)
    {
        request.Check();
        await CheckDatabase(request.Email, request.Login);
        if (NotificationsWrapper.HasNotification()) throw new NotificationException();
        Person person = request;
        person.AddUser(new User(request.Login, request.Password));
        await userRepository.InsertWithSaveChangesAsync(person);
        return new GenericResponse<object>();
    }

    public async Task<BaseResponse<IEnumerable<PersonDto>>> GetDtoListAsync(int pageIndex, int pageSize)
    {
        var list = await GetListAsync(pageIndex, pageSize);
        return new GenericResponse<IEnumerable<PersonDto>>(list.Select(x => (PersonDto)x));
    }

    public async Task<BaseResponse<PersonDto>> GetDtoByIdAsync(Guid id)
    {
        var dto = await GetByIdAsync(id);
        if(dto == null) NotificationsWrapper.AddNotification(NotificationMessages.NotFoundEntity("Person"));
        if(NotificationsWrapper.HasNotification()) throw new NotificationException();
        return new GenericResponse<PersonDto>(dto!);
    }

    #region Private Methods

    private async Task CheckDatabase(string email, string login)
    {
        if ((await userRepository.GetAsync(x => x.Email == email)).Any())
            NotificationsWrapper.AddNotification(NotificationMessages.AlreadyRegistered("Email"));
        if ((await userRepository.GetAsync(x => x.Users.Any(x => x.Login == login))).Any())
            NotificationsWrapper.AddNotification(NotificationMessages.AlreadyRegistered("Login"));
    }

    #endregion
}