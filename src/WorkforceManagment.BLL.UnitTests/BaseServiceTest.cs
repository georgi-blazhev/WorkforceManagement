using Moq;
using System;
using WorkforceManagement.BLL.IServices;
using WorkforceManagement.DAL.IRepositories;
using Xunit;

namespace WorkforceManagment.BLL.UnitTests
{
    public class BaseServiceTest
    {
        public BaseServiceTest()
        {
            UserManager = new Mock<IUserManager>();
            TeamRepository = new Mock<ITeamRepository>();
        }
        public Mock<IUserManager> UserManager { get; set; }
        public Mock<ITeamRepository> TeamRepository { get; set; }
    }
}
