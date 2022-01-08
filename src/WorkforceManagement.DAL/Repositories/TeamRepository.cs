using WorkforceManagement.DAL.Data;
using WorkforceManagement.DAL.Entities;
using WorkforceManagement.DAL.IRepositories;

namespace WorkforceManagement.DAL.Repositories
{
    public class TeamRepository : Repository<Team>, ITeamRepository
    {
        public TeamRepository(DatabaseContext context)
            : base(context)
        {
        }
    }
}


