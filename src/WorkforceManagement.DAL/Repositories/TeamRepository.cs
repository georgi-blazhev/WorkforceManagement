using WorkforceManagement.DAL.Data;
using WorkforceManagement.DAL.Entities;
using WorkforceManagement.DAL.IRepositories;

namespace WorkforceManagement.DAL.Repositories
{
    public class TeamRepository<TEntity> : Repository<Team>, ITeamRepository<Team>
    {
        public TeamRepository(DatabaseContext context)
            : base(context)
        {
        }
    }
}


