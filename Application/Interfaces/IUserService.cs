using Domain.Common;
using Domain.Common.User;
using Domain.Entities;

namespace Application.Interfaces;

public interface IUserService : IBaseCrudService<User>
{
    public Task<BaseResponse<string>> Login(LoginRequest request);
}