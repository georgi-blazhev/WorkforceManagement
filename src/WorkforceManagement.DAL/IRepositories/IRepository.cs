using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WorkforceManagement.DAL.Entities;

namespace WorkforceManagement.DAL.IRepositories
{
    public interface IRepository<TEntity> where TEntity : AbstractEntity
    {
        Task<TEntity> FindByIdAsync(string id);
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<List<TEntity>> GetAllAsync();
        Task CreateAsync(TEntity entity);
        Task<TEntity> EditAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task DeleteCollectionAsync(IEnumerable<TEntity> entities);
    }
}