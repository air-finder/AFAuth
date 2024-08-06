using Application.Interfaces;
using Domain.Common;
using Domain.Common.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers;
[Route("api/[controller]")]
[Authorize]
public class UserController(IUserService service) : Controller
{
    [HttpPost]
    [SwaggerOperation(Summary = "Login")]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status400BadRequest)]
    [AllowAnonymous]
    public async Task<IActionResult> CreatePerson([FromBody] LoginRequest request)
    {
        return Ok(await service.Login(request));
    }
}