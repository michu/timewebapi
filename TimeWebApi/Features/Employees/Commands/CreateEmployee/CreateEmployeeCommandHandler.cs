namespace TimeWebApi.Features.Employees.Commands.CreateEmployee;

using TimeWebApi.DAL.Employees.Interfaces;
using TimeWebApi.Features.Common.Extensions;
using TimeWebApi.Features.Common.Messaging;

public sealed class CreateEmployeeCommandHandler : ICommandHandler<CreateEmployeeCommand, int>
{
    private readonly IEmployeeRepository _employeeRepository;

    public CreateEmployeeCommandHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<int> Handle(CreateEmployeeCommand command, CancellationToken cancellationToken)
    {
        await _employeeRepository.ThrowIfExistsByEmail(command.Email, cancellationToken);

        return await _employeeRepository.Add(command.ToEntity(), cancellationToken);
    }
}
