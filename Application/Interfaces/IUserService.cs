using Domain.Common;
using Domain.Common.User;
using Domain.Entities;

namespace Application.Interfaces;

public interface IUserService : IBaseCrudService<User>
{
    public Task<BaseResponse<string>> Login(LoginRequest request);
    public Task<BaseResponse<object>> UpdatePassword(Guid id, UpdatePasswordRequest request);
    public Task<BaseResponse<object>> ForgetPassword(string baerer, ForgetPasswordRequest request);
    public Task<BaseResponse<object>> ForgetPasswordCheck(ForgetPasswordRequest request);
    public Task<BaseResponse<object>> ForgetPasswordUpdate(ForgetPasswordRequest request);
}