using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EmployeePermissions.Infrastructure;
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly EmployeeDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public Repository(EmployeeDbContext dbContext, IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }
        public Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return _dbContext.Set<TEntity>().ToListAsync(cancellationToken);
        }

        public async Task<TEntity?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }

        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            await Task.FromResult(_dbContext.Set<TEntity>().Update(entity));
        }

        public TEntity? GetById(Guid id)
        {
            return _dbContext.Set<TEntity>().Find(id);
        }

        public List<TEntity> GetAll()
        {
            return _dbContext.Set<TEntity>().ToList();
        }
        
        public async Task DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null) _dbContext.Set<TEntity>().Remove(entity);
        }
    }
