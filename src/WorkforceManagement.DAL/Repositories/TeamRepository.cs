using System.Threading.Tasks;
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

        public async Task AssignUserToTeamAsync(User user, Team team)
        {
            team.Members.Add(user);
            await _dataContext.SaveChangesAsync();
        }

        public async Task UnssignUserToTeamAsync(User user, Team team)
        {
            team.Members.Remove(user);
            await _dataContext.SaveChangesAsync();
        }
    }
}