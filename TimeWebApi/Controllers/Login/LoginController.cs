namespace TimeWebApi.Controllers.Login;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TimeWebApi.Auth;
using TimeWebApi.Controllers.Login.Requests;
using TimeWebApi.Features.Employees.Queries.GetEmployeeByEmail;
using TimeWebApi.Resources;

[ApiController]
[Route("api/login")]
public sealed class LoginController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly JwtTokenGenerator _tokenGenerator;

    public LoginController(IMediator mediator, JwtTokenGenerator tokenGenerator)
    {
        _mediator = mediator;
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
    public async Task<IActionResult> Post([FromBody] LoginRequest request)
    {
        if (request.Email == "admin@example.com")
        {
            return Ok(_tokenGenerator.Generate(request.Email, [StaticData.Roles.Admin]));
        }

        var employee = await _mediator.Send(new GetEmployeeOrDefaultByEmailQuery { Email = request.Email });

        if (employee == null)
        {
            return Unauthorized();
        }

        var extraClaims = new Dictionary<string, string>
        {
            { StaticData.Claims.EmployeeId, employee.Id.ToString() },
            { ClaimTypes.Name, employee.FirstName },
            { ClaimTypes.Surname, employee.LastName },
        };

        return Ok(_tokenGenerator.Generate(request.Email, [StaticData.Roles.Employee], extraClaims.AsReadOnly()));
    }
}
