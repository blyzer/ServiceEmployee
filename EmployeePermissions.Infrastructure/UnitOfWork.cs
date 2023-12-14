using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore.Storage;

namespace EmployeePermissions.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly EmployeeDbContext _dbContext;
    private IDbContextTransaction _transaction;
    private bool _disposed = false;
    private readonly ConcurrentDictionary<string, object> _repositories;

    public UnitOfWork(EmployeeDbContext dbContext, IDbContextTransaction transaction)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _transaction = transaction;
        _repositories = new ConcurrentDictionary<string, object>();
    }

    public IRepository<T> GetRepository<T>() where T : class
    {
        var type = typeof(T).Name;

        if (!_repositories.ContainsKey(type))
        {
            var repositoryType = typeof(Repository<>);
            
            var repositoryInstance = Activator.CreateInstance(
                repositoryType.MakeGenericType(typeof(T)), _dbContext);

            if (repositoryInstance != null) _repositories.TryAdd(type, repositoryInstance);
        }

        return (IRepository<T>)_repositories[type];
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _transaction.DisposeAsync();

        _transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
    }
    
    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            await (_transaction.CommitAsync()).ConfigureAwait(false);
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            await _transaction.DisposeAsync();
        }
    }
    
    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _transaction.RollbackAsync(cancellationToken);
        }
        finally
        {
            await _transaction.DisposeAsync();
        }
    }

    public Task CommitAsync(CancellationToken cancellationToken = default)
    {
        return _transaction.CommitAsync(cancellationToken);
    }

    public void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _transaction.Dispose();

                _dbContext.Dispose();
            }

            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}

