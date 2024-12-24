namespace TimeWebApi.Features.Common.Extensions;

using TimeWebApi.DAL.Employees.Interfaces;
using TimeWebApi.Features.Common.Exceptions;

public static class EmployeeRepositoryExtensions
{
    public static async Task ThrowIfDoesNotExist(this IEmployeeRepository repository, int id, CancellationToken cancellationToken)
    {
        var exists = await repository.Exists(id, cancellationToken);

        if (!exists)
        {
            throw new NotFoundException("Employee with given id does not exists.");
        }
    }

    public static async Task ThrowIfExistsByEmail(this IEmployeeRepository repository, string email, CancellationToken cancellationToken)
    {
        var exists = await repository.ExistsByEmail(email, cancellationToken);

        if (exists)
        {
            throw new ConflictException("Employee with given email already exists.");
        }
    }

    public static async Task ThrowIfExistsByEmail(this IEmployeeRepository repository, string email, int excludeId, CancellationToken cancellationToken)
    {
        var exists = await repository.ExistsByEmail(email, excludeId, cancellationToken);

        if (exists)
        {
            throw new ConflictException("Employee with given email already exists.");
        }
    }
}
