namespace TimeWebApi.DAL;

public interface IRepository<TEntity>
    where TEntity : class
{
    Task<int> Add(TEntity entity, CancellationToken cancellationToken);
    Task Delete(int id, CancellationToken cancellationToken);
    Task Update(TEntity entity, CancellationToken cancellationToken);
    Task<TEntity?> GetById(int id, CancellationToken cancellationToken);
}
