using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WorkforceManagement.DAL.Data;
using WorkforceManagement.DAL.Entities;
using WorkforceManagement.DAL.IRepositories;

namespace WorkforceManagement.DAL.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : AbstractEntity
    {
        protected readonly DatabaseContext _dataContext;
        protected readonly DbSet<TEntity> _entities;

        public Repository(DatabaseContext context)
        {
            _dataContext = context;
            _entities = _dataContext.Set<TEntity>();
        }

        public TEntity FindById(string id)
        {
            TEntity entity = _entities.Find(Guid.Parse(id));
            if (entity != null) return entity;
            throw new KeyNotFoundException($"An entity with the given ID does not exist!");
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
        public void Create(TEntity entity)
        {
            _entities.Add(entity);
        }
        public void Delete(TEntity entity)
        {
            _entities.Remove(entity);
        }
        public void DeleteCollection(IEnumerable<TEntity> entities)
        {
            _entities.RemoveRange(entities);
        }
        public virtual TEntity Edit(TEntity entity)
        {
            return _entities.Update(entity).Entity;
        }
    }
}
