using Moq;
using WorkforceManagement.BLL.IHelpers;
using WorkforceManagement.BLL.IServices;
using WorkforceManagement.BLL.Services;
using WorkforceManagement.DAL.IRepositories;
using WorkforceManagement.DAL.Repositories;

namespace WorkforceManagment.BLL.UnitTests
{
    public class BaseServiceTest
    {
        public BaseServiceTest()
        {
            UserManager = new Mock<IUserManager>();
            TeamRepository = new Mock<ITeamRepository>();
            TimeOffRequestRepository = new Mock<ITimeOffRequestRepository>();
            TeamService = new Mock<ITeamService>();
            UserService = new Mock<IUserService>();
            TimeOffRequestService = new Mock<ITimeOffRequestService>();
            UserServiceHelper = new Mock<IUserServiceHelper>();
            EmailService = new Mock<IEmailService>();
            TimeOffRequestHelper = new Mock<ITimeOffRequestHelper>();
        }
        public Mock<IUserManager> UserManager { get; set; }
        public Mock<ITeamRepository> TeamRepository { get; set; }
        public Mock<ITimeOffRequestRepository> TimeOffRequestRepository { get; set; }
        public Mock<ITeamService> TeamService { get; set; }
        public Mock<IUserService> UserService { get; set; }
        public Mock<ITimeOffRequestService> TimeOffRequestService { get; set; }
        public Mock<IUserServiceHelper> UserServiceHelper{ get; set; }
        public Mock<IEmailService> EmailService { get; set; }
        public Mock<ITimeOffRequestHelper> TimeOffRequestHelper { get; set; }
    }
}
