using Application;
using Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers;
[Route("api/[controller]")]
public abstract class BaseController<T>(IBaseCrudService<T> baseService) : Controller where T : class
{
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Generic GET by Id")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
        => Ok(await baseService.GetByIdAsync(id));

    [HttpGet]
    [SwaggerOperation(Summary = "Generic GET")]
    [ProducesResponseType(typeof(BaseResponse<List<object>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAsync([FromQuery] int pageIndex, int pageSize)
        => Ok(await baseService.GetListAsync(pageIndex, pageSize));
    
    // [HttpDelete("{id}")]
    // [SwaggerOperation(Summary = "Generic DELETE")]
    // [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    // [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    // public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    //     => Ok(await baseService.DeleteAsync(id));
}