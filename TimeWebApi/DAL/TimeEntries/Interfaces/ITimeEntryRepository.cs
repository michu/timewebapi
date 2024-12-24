namespace TimeWebApi.DAL.TimeEntries.Interfaces;

using TimeWebApi.Domain.Models;

public interface ITimeEntryRepository : IRepository<TimeEntry>
{
    Task<bool> Exists(int id, CancellationToken cancellationToken);
    Task<bool> ExistsByEmployeeIdAndDate(int employeeId, DateOnly date, CancellationToken cancellationToken);
    Task<bool> ExistsByEmployeeIdAndDate(int employeeId, DateOnly date, int excludeId, CancellationToken cancellationToken);
    Task<IEnumerable<TimeEntry>> GetByEmployeeId(int employeeId, CancellationToken cancellationToken);
}
