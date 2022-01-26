using System.Threading.Tasks;
using WorkforceManagement.DAL.Entities;

namespace WorkforceManagement.DAL.IRepositories
{
    public interface ITeamRepository : IRepository<Team>
    {
        public Task AssignNewTeamLead(User user, Team team);
        public Task AssignUserToTeamAsync(User user, Team team);
        public Task UnssignUserToTeamAsync(User user, Team team);
    }
}