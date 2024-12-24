namespace TimeWebApi.Features.Common.Extensions;

using TimeWebApi.DAL.TimeEntries.Interfaces;
using TimeWebApi.Features.Common.Exceptions;

public static class TimeEntryRepositoryExtensions
{
    public static async Task ThrowIfDoesNotExist(this ITimeEntryRepository repository, int id, CancellationToken cancellationToken)
    {
        var exists = await repository.Exists(id, cancellationToken);

        if (!exists)
        {
            throw new NotFoundException("Time Entry with given id does not exists.");
        }
    }

    public static async Task ThrowIfExistsByEmployeeIdAndDate(this ITimeEntryRepository repository, int employeeId, DateOnly date, CancellationToken cancellationToken)
    {
        var exists = await repository.ExistsByEmployeeIdAndDate(employeeId, date, cancellationToken);

        if (exists)
        {
            throw new ConflictException("Time Entry with given date already exists.");
        }
    }

    public static async Task ThrowIfExistsByEmployeeIdAndDate(this ITimeEntryRepository repository, int employeeId, DateOnly date, int excludeId, CancellationToken cancellationToken)
    {
        var exists = await repository.ExistsByEmployeeIdAndDate(employeeId, date, excludeId, cancellationToken);

        if (exists)
        {
            throw new ConflictException("Time Entry with given date already exists.");
        }
    }
}
