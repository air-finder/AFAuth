using Application.Interfaces;
using Domain.Common;
using Domain.Common.People;
using Domain.Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers;
[Route("api/[controller]")]
[Authorize]
public class PeopleController(IPeopleService service): Controller
{
    [HttpPost]
    [SwaggerOperation(Summary = "Creates a person with an user bound")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [AllowAnonymous]
    public async Task<IActionResult> CreatePerson([FromBody] CreatePersonRequest request)
    {
        return Ok(await service.CreateAsync(request));
    }
    
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Returns a person by Id")]
    [ProducesResponseType(typeof(BaseResponse<PersonDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
        => Ok(await service.GetDtoByIdAsync(id));

    [HttpGet]
    [SwaggerOperation(Summary = "Return a list of people")]
    [ProducesResponseType(typeof(BaseResponse<List<PersonDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAsync([FromQuery] int pageIndex, int pageSize)
        => Ok(await service.GetDtoListAsync(pageIndex, pageSize));
}