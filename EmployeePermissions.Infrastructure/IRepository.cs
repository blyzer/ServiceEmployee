namespace EmployeePermissions.Infrastructure;

public interface IRepository<TEntity>
{
    Task<TEntity?> GetByIdAsync(Guid id);
    Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(Guid id);
}