using Application.Interfaces;
using Domain.Common;
using Domain.Common.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers;
[Route("api/[controller]")]
[Authorize]
public class UserController(IUserService service) : BaseController
{
    [HttpPost]
    [SwaggerOperation(Summary = "Login")]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status400BadRequest)]
    [AllowAnonymous]
    public async Task<IActionResult> CreatePerson([FromBody] LoginRequest request)
        => Ok(await service.Login(request));

    [HttpPatch("password")]
    [SwaggerOperation(Summary = "Update password")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequest request)
        => Ok(await service.UpdatePassword(user!.Id, request));

    [HttpPost("password/token")]
    [SwaggerOperation(Summary = "Sends an email to update password if user exists")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequest request)
        => Ok(await service.ForgetPassword(authToken, request));

    [HttpPatch("password/token")]
    [SwaggerOperation(Summary = "Checks if code is the same as the sent")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ForgetPasswordCheck([FromBody] ForgetPasswordRequest request)
        => Ok(await service.ForgetPasswordCheck(request));

    [HttpPatch("password/token-update")]
    [SwaggerOperation(Summary = "Update password for checked email")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ForgetPasswordUpdate([FromBody] ForgetPasswordRequest request)
        => Ok(await service.ForgetPasswordUpdate(request));
}