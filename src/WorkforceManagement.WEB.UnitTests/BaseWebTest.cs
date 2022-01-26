using AutoMapper;
using Microsoft.AspNetCore.Routing;
using Moq;
using WorkforceManagement.BLL.IServices;
using WorkforceManagement.BLL.Services;

namespace WorkforceManagement.WEB.UnitTests
{
    public class BaseWebTest
    {
        public BaseWebTest()
        {
            TeamService = new Mock<ITeamService>();
            UserService = new Mock<IUserService>();
            TimeOffRequestService = new Mock<ITimeOffRequestService>();
            Mapper = new Mock<IMapper>();
            LinkGenerator = new Mock<LinkGenerator>();
        }
        public Mock<ITeamService> TeamService { get; set; }
        public Mock<IUserService> UserService { get; set; }
        public Mock<ITimeOffRequestService> TimeOffRequestService { get; set; }
        public Mock<IMapper> Mapper { get; set; }
        public Mock<LinkGenerator> LinkGenerator { get; set; }
    }
}
