namespace TimeWebApi.Features.Employees.Commands.DeleteEmployee;

using MediatR;
using TimeWebApi.DAL.Employees.Interfaces;
using TimeWebApi.Features.Common.Messaging;

public sealed class DeleteEmployeeCommandHandler : ICommandHandler<DeleteEmployeeCommand, Unit>
{
    private readonly IEmployeeRepository _employeeRepository;

    public DeleteEmployeeCommandHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<Unit> Handle(DeleteEmployeeCommand command, CancellationToken cancellationToken)
    {
        await _employeeRepository.Delete(command.Id, cancellationToken);

        return Unit.Value;
    }
}
