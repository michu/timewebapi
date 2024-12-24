namespace TimeWebApi.Features.Employees.Commands.UpdateEmployee;

using MediatR;
using TimeWebApi.DAL.Employees.Interfaces;
using TimeWebApi.Features.Common.Extensions;
using TimeWebApi.Features.Common.Messaging;

public sealed class UpdateEmployeeCommandHandler : ICommandHandler<UpdateEmployeeCommand, Unit>
{
    private readonly IEmployeeRepository _employeeRepository;

    public UpdateEmployeeCommandHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<Unit> Handle(UpdateEmployeeCommand command, CancellationToken cancellationToken)
    {
        await _employeeRepository.ThrowIfDoesNotExist(command.Id, cancellationToken);
        await _employeeRepository.ThrowIfExistsByEmail(command.Email, command.Id, cancellationToken);

        await _employeeRepository.Update(command.ToEntity(), cancellationToken);

        return Unit.Value;
    }
}
