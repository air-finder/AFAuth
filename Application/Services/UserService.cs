using Application.Interfaces;
using Domain.Common;
using Domain.Common.User;
using Domain.Constants;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Domain.SeedWork.Notification;
using Infra.Security;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class UserService(IUserRepository userRepository, IJwtService jwtService) : BaseCrudService<User>(userRepository), IUserService
{
    public async Task<BaseResponse<string>> Login(LoginRequest request)
    {
        request.Check();
        var user = await userRepository.Get(x => x.Login == request.Login).FirstOrDefaultAsync();
        if(user == null || !user.CheckPassword(request.Password)) NotificationsWrapper.AddNotification(NotificationMessages.InvalidCredentials);
        if (NotificationsWrapper.HasNotification()) throw new NotificationException();
        return new GenericResponse<string>(jwtService.CreateToken(user!));
    }
}