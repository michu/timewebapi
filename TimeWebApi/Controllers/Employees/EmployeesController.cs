namespace TimeWebApi.Controllers.Employees;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeWebApi.Controllers.Employees.Mappings;
using TimeWebApi.Controllers.Employees.Requests;
using TimeWebApi.Controllers.Employees.Responses;
using TimeWebApi.Features.Employees.Commands.DeleteEmployee;
using TimeWebApi.Features.Employees.Queries.GetEmployee;
using TimeWebApi.Features.Employees.Queries.GetEmployees;
using TimeWebApi.Resources;

[ApiController]
[Route("/api/employees")]
public sealed class EmployeesController : ControllerBase
{
    private readonly IMediator _mediator;

    public EmployeesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Adds new employee
    /// </summary>
    /// <param name="request">The employee data</param>
    [Authorize(Roles = StaticData.Roles.Admin)]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> Create([FromBody] CreateEmployeeRequest request)
    {
        var employeeId = await _mediator.Send(request.ToCommand());

        return CreatedAtAction(nameof(GetById), new { id = employeeId }, null);
    }

    /// <summary>
    /// Deletes existing employee
    /// </summary>
    /// <param name="id">The id of employee</param>
    [Authorize(Roles = StaticData.Roles.Admin)]
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteEmployeeCommand { Id = id });

        return NoContent();
    }

    /// <summary>
    /// Gets existing employee details
    /// </summary>
    /// <param name="id">The id of employee</param>
    /// <returns>The employee details</returns>
    [Authorize(Roles = $"{StaticData.Roles.Admin},{StaticData.Roles.Employee}")]
    [HttpGet("{id:int}")]
    [ProducesResponseType<EmployeeResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetById(int id)
    {
        var employee = await _mediator.Send(new GetEmployeeQuery { Id = id });

        return Ok(employee.ToResponse());
    }

    /// <summary>
    /// Gets list of employees
    /// </summary>
    /// <returns>The list of employees</returns>
    [Authorize(Roles = StaticData.Roles.Admin)]
    [HttpGet]
    [ProducesResponseType<IEnumerable<EmployeeResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> GetAll()
    {
        var employees = await _mediator.Send(new GetEmployeesQuery());

        return Ok(employees.ToResponse());
    }

    /// <summary>
    /// Updates existing employee
    /// </summary>
    /// <param name="id">The id of employee</param>
    /// <param name="request">The employee data</param>
    [Authorize(Roles = $"{StaticData.Roles.Admin},{StaticData.Roles.Employee}")]
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateEmployeeRequest request)
    {
        await _mediator.Send(request.ToCommand(id));

        return NoContent();
    }
}
