using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using Domain.Entities.Dtos;
using Infra.Security.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[Route("api/[controller]")]
[Authorize]
public abstract class BaseController() : Controller
{
    private string authToken => HttpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");
    private JwtSecurityToken token => new JwtSecurityTokenHandler().ReadJwtToken(authToken);
    protected UserDto? user => GetType<UserDto>(JwtClaims.CAIM_USER_PROFILE);
    protected IEnumerable<string>? scopes => GetType<IEnumerable<string>>(JwtClaims.CLAIM_SCOPES);
    private static JsonSerializerOptions? serializeOptions 
        => new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, WriteIndented = true };
    private T? GetType<T>(string type) 
        => JsonSerializer.Deserialize<T>(token.Claims.FirstOrDefault(x => x.Type == type)!.Value, serializeOptions);
}