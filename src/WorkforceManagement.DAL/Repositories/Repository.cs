using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WorkforceManagement.DAL.Data;
using WorkforceManagement.DAL.Entities;
using WorkforceManagement.DAL.IRepositories;

namespace WorkforceManagement.DAL.Repositories
{
    [ExcludeFromCodeCoverage]
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : AbstractEntity
    {
        protected readonly DatabaseContext _dataContext;
        protected readonly DbSet<TEntity> _entities;

        public Repository(DatabaseContext context)
        {
            _dataContext = context;
            _entities = _dataContext.Set<TEntity>();
        }

        public async Task<TEntity> FindByIdAsync(string id)
        {
            TEntity entity = await _entities.FindAsync(Guid.Parse(id));
            if (entity != null) return entity;
            throw new KeyNotFoundException($"An entity with the given ID does not exist!");
        }
        public virtual async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _entities.AsQueryable().Where(predicate).ToListAsync();
        }
        public async Task<List<TEntity>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }
        public async Task CreateAsync(TEntity entity)
        {
            await _entities.AddAsync(entity);
            await _dataContext.SaveChangesAsync();
        }
        public virtual async Task<TEntity> EditAsync(TEntity entity)
        {
            var result = _entities.Update(entity).Entity;
            await _dataContext.SaveChangesAsync();
            return result;
        }
        public async Task DeleteAsync(TEntity entity)
        {
            _entities.Remove(entity);
            await _dataContext.SaveChangesAsync();
        }
        public async Task DeleteCollectionAsync(IEnumerable<TEntity> entities)
        {
            _entities.RemoveRange(entities);
            await _dataContext.SaveChangesAsync();
        }
    }
}