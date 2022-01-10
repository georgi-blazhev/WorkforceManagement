using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WorkforceManagement.DAL.Data;
using WorkforceManagement.DAL.Entities;
using WorkforceManagement.DAL.IRepositories;

namespace WorkforceManagement.DAL.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : AbstractEntity
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

        public async Task<TEntity> FindByNameAsync(string title)
        {
            TEntity entity = await _entities.FindAsync(title);

            if (entity != null) return entity;

            throw new KeyNotFoundException($"An entity with the given name does not exist!");
        }

        public virtual async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            IEnumerable<TEntity> entities = await _entities.AsQueryable().Where(predicate).ToListAsync();

            if (entities != null) return entities;

            throw new KeyNotFoundException($"An entity with the given Name does not exist!");
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

        public virtual TEntity Edit(TEntity entity)
        {
            var result = _entities.Update(entity).Entity;
            _dataContext.SaveChanges();
            return result;
        }

        public void Delete(TEntity entity)
        {
            _entities.Remove(entity);
             _dataContext.SaveChanges();
        }

        public void DeleteCollection(IEnumerable<TEntity> entities)
        {
            _entities.RemoveRange(entities);
            _dataContext.SaveChanges();

        }

        public async Task SaveChangesAsync()
        {
            await _dataContext.SaveChangesAsync();
        }
    }
}