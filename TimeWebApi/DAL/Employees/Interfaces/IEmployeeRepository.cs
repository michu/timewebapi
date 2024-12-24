namespace TimeWebApi.DAL.Employees.Interfaces;

using TimeWebApi.Domain.Models;

public interface IEmployeeRepository : IRepository<Employee>
{
    Task<bool> Exists(int id, CancellationToken cancellationToken);
    Task<bool> ExistsByEmail(string email, CancellationToken cancellationToken);
    Task<bool> ExistsByEmail(string email, int excludeId, CancellationToken cancellationToken);
    Task<IEnumerable<Employee>> GetAll(CancellationToken cancellationToken);
    Task<Employee?> GetByEmail(string email, CancellationToken cancellationToken);
}
