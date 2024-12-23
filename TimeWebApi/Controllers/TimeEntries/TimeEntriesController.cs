namespace TimeWebApi.Controllers.TimeEntries;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeWebApi.Controllers.TimeEntries.Mappings;
using TimeWebApi.Controllers.TimeEntries.Requests;
using TimeWebApi.Controllers.TimeEntries.Responses;
using TimeWebApi.Features.TimeEntries.Commands.DeleteTimeEntry;
using TimeWebApi.Features.TimeEntries.Queries.GetTimeEntries;
using TimeWebApi.Resources;

[ApiController]
[Route("/api/employees/{id:int}/time-entries")]
public sealed class TimeEntriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public TimeEntriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Adds time entry for given employee
    /// </summary>
    /// <param name="id">The id of employee</param>
    /// <param name="request">The time entry data</param>
    [Authorize(Roles = $"{StaticData.Roles.Admin},{StaticData.Roles.Employee}")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> Create(int id, [FromBody] CreateTimeEntryRequest request)
    {
        await _mediator.Send(request.ToCommand(id));

        return Created();
    }

    /// <summary>
    /// Deletes time entry for given employee
    /// </summary>
    /// <param name="id">The if of employee</param>
    /// <param name="timeEntryId">The id of time entry</param>
    [Authorize(Roles = $"{StaticData.Roles.Admin},{StaticData.Roles.Employee}")]
    [HttpDelete("{timeEntryId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id, int timeEntryId)
    {
        await _mediator.Send(new DeleteTimeEntryCommand { EmployeeId = id, Id = timeEntryId });

        return NoContent();
    }

    /// <summary>
    /// Gets the list of time entries for given employee
    /// </summary>
    /// <param name="id">The id of employee</param>
    /// <returns>The list of time entries</returns>
    [Authorize(Roles = $"{StaticData.Roles.Admin},{StaticData.Roles.Employee}")]
    [HttpGet]
    [ProducesResponseType<IEnumerable<TimeEntryResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetByEmployeeId(int id)
    {
        var timeEntries = await _mediator.Send(new GetTimeEntriesQuery { EmployeeId = id });

        return Ok(timeEntries.ToResponse());
    }

    /// <summary>
    /// Updates time entry for given employee
    /// </summary>
    /// <param name="id">The id of employee</param>
    /// <param name="timeEntryId">The id of time entry</param>
    /// <param name="request">The time entry data</param>
    [Authorize(Roles = $"{StaticData.Roles.Admin},{StaticData.Roles.Employee}")]
    [HttpPut("{timeEntryId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> Update(int id, int timeEntryId, [FromBody] UpdateTimeEntryRequest request)
    {
        await _mediator.Send(request.ToCommand(id, timeEntryId));

        return NoContent();
    }
}
