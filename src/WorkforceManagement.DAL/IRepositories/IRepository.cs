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
        TEntity FindById(string id);
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<List<TEntity>> GetAllAsync();
        void Create(TEntity entity);
        TEntity Edit(TEntity entity);
        void Delete(TEntity entity);
        void DeleteCollection(IEnumerable<TEntity> entities);
    }
}
