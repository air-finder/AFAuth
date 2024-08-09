using Application.Interfaces;
using Domain.Common;
using Domain.Common.People;
using Domain.Constants;
using Domain.Entities;
using Domain.Entities.Dtos;
using Domain.Exceptions;
using Domain.Repositories;
using Domain.SeedWork.Notification;

namespace Application.Services;

public class PeopleService(IPeopleRepository userRepository) : BaseCrudService<Person>(userRepository), IPeopleService
{
    public async Task<BaseResponse<object>> CreateAsync(CreatePersonRequest request)
    {
        request.Check();
        await CheckDatabase(request.Email, request.Login);
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
        => new GenericResponse<PersonDto>(await GetDtoByIdWithChecksAsync(id));

    public async Task<BaseResponse<object>> DeleteWithCheckAsync(Guid id, UserDto user)
    {
        var dto = await GetDtoByIdWithChecksAsync(id);
        if (dto.Users.All(x => x.Id != user.Id)) AddNotification(NotificationMessages.UnauthorizedAction);
        CheckNotification();
        await DeleteAsync(id);
        return new GenericResponse<object>();
    }

    #region Private Methods

    private async Task CheckDatabase(string email, string login)
    {
        if ((await userRepository.GetAsync(person => person.Email == email)).Any())
            AddNotification(NotificationMessages.AlreadyRegistered("Email"));
        if ((await userRepository.GetAsync(person => person.Users.Any(user => user.Login == login))).Any())
            AddNotification(NotificationMessages.AlreadyRegistered("Login"));
        CheckNotification();
    }

    private async Task<PersonDto> GetDtoByIdWithChecksAsync(Guid id)
    {
        var dto = await GetByIdAsync(id);
        if(dto == null) NotificationsWrapper.AddNotification(NotificationMessages.NotFoundEntity("Person"));
        if(NotificationsWrapper.HasNotification()) throw new NotificationException();
        return dto!;
    }

    #endregion
}