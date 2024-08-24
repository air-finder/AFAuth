using System.Text;
using Application.Interfaces;
using Domain.Common;
using Domain.Common.User;
using Domain.Constants;
using Domain.Entities;
using Domain.Repositories;
using Domain.SeedWork.Notification;
using Infra.Http.AFMail;
using Infra.Http.AFMail.Requests;
using Infra.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace Application.Services;

public class UserService(
    IUserRepository repository,
    IJwtService jwtService,
    IMailService mailService,
    IDistributedCache cache
) : BaseCrudService<User>(repository), IUserService
{
    public async Task<BaseResponse<string>> Login(LoginRequest request)
    {
        request.Check();
        var user = await repository.Get(x => x.Login == request.Login).FirstOrDefaultAsync();
        if(user == null || !user.CheckPassword(request.Password)) NotificationsWrapper.AddNotification(NotificationMessages.InvalidCredentials);
        CheckNotification();
        return new GenericResponse<string>(jwtService.CreateToken(user!));
    }

    public async Task<BaseResponse<object>> UpdatePassword(Guid id, UpdatePasswordRequest request)
    {
        request.Check();
        var user = await repository.GetByIDAsync(id);
        if (user == null) NotificationsWrapper.AddNotification(NotificationMessages.NotFoundEntity(nameof(user)));
        CheckNotification();
        user!.UpdatePassword(request.Password);
        await repository.SaveChangesAsync();
        return new GenericResponse<object>();
    }

    public async Task<BaseResponse<object>> ForgetPassword(ForgetPasswordRequest request)
    {
        request.Check();
        var user = await repository.Get(x => x.Login == request.User).FirstOrDefaultAsync();
        if (user == null) NotificationsWrapper.AddNotification(NotificationMessages.NotFoundEntity("user"));
        CheckNotification();

        var token = GenerateRandomPin(6);
        await cache.SetStringAsync(
            $"reset-code-{user!.Id}",
            token,
            new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) }
        );
        var mailRequest = new MailRequest(new List<string> { user.Person!.Email }, "seu token", token);
        await mailService.SendMail(jwtService.CreateToken(user), mailRequest);
        return new GenericResponse<object>();
    }

    public async Task<BaseResponse<object>> ForgetPasswordCheck(ForgetPasswordRequest request)
    {
        request.Check();
        var user = await repository.Get(x => x.Login == request.User).FirstOrDefaultAsync();
        if (user == null) NotificationsWrapper.AddNotification(NotificationMessages.NotFoundEntity("user"));
        CheckNotification();

        var cachedCode = await cache.GetStringAsync($"reset-code-{user!.Id}");
        if (cachedCode != request.Code) NotificationsWrapper.AddNotification(NotificationMessages.InvalidEntity(nameof(request.Code)));
        CheckNotification();

        return new GenericResponse<object>();
    }

    public async Task<BaseResponse<object>> ForgetPasswordUpdate(ForgetPasswordRequest request)
    {
        request.Check();
        var user = await repository.Get(x => x.Login == request.User).FirstOrDefaultAsync();
        if (user == null) NotificationsWrapper.AddNotification(NotificationMessages.NotFoundEntity("user"));
        CheckNotification();

        var cachedCode = await cache.GetStringAsync($"reset-code-{user!.Id}");
        if (cachedCode != request.Code) NotificationsWrapper.AddNotification("Too late to do this!");
        CheckNotification();

        user!.UpdatePassword(request.Password!);
        await repository.SaveChangesAsync();
        return new GenericResponse<object>();
    }

    #region Private Methods

    private string GenerateRandomPin(int pinLength)
    {
        var random = new Random();
        var builder = new StringBuilder();
        for (var i = 0; i < pinLength; i++)
        {
            builder.Append(random.Next(0, 9));
        }
        return builder.ToString();
    }
    #endregion
}