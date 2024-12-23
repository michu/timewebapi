namespace TimeWebApi.Controllers.Login;

using Microsoft.AspNetCore.Mvc;
using TimeWebApi.Auth;
using TimeWebApi.Controllers.Login.Requests;
using TimeWebApi.Resources;

[ApiController]
[Route("api/login")]
public sealed class LoginController : ControllerBase
{
    private JwtTokenGenerator _tokenGenerator;

    public LoginController(JwtTokenGenerator tokenGenerator)
    {
        _tokenGenerator = tokenGenerator;
    }

    /// <summary>
    /// Logins user
    /// </summary>
    /// <param name="request">The login data</param>
    /// <returns>The bearer token</returns>
    [HttpPost]
    [ProducesResponseType<string>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Post([FromBody] LoginRequest request)
    {
        return request.Email switch
        {
            "admin@example.com" => Ok(_tokenGenerator.Generate(request.Email, [StaticData.Roles.Admin])),
            "unknown@example.com" => Unauthorized(),
            _ => Ok(_tokenGenerator.Generate(request.Email, [StaticData.Roles.Employee]))
        };
    }
}
