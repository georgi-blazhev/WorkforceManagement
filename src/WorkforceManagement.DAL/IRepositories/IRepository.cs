using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WorkforceManagement.DAL.Entities;

namespace WorkforceManagement.DAL.IRepositories
{
    public interface IRepository<TEntity> where TEntity : AbstractEntity
    {
        Task<TEntity> FindByIdAsync(string id);
        Task<TEntity> FindByNameAsync(string name);
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<List<TEntity>> GetAllAsync();
        Task CreateAsync(TEntity entity);
        TEntity Edit(TEntity entity);
        void Delete(TEntity entity);
        void DeleteCollection(IEnumerable<TEntity> entities);
        Task SaveChangesAsync();
    }
}
