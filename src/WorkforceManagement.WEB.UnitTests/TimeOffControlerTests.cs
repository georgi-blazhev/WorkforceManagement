using Moq;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using WorkforceManagement.DAL.Entities;
using WorkforceManagement.Models.TimeOff;
using WorkforceManagement.WEB.Controllers;
using Xunit;

namespace WorkforceManagement.WEB.UnitTests
{
    public class TimeOffControlerTests : BaseWebTest
    {
        [Fact]
        public async Task Create_TimeOffRequest_Calls_Service()
        {
            var sut = new TimeOffRequestsController(TimeOffRequestService.Object, UserService.Object, Mapper.Object, LinkGenerator.Object);
            User user = new User() { UserName = "fake-user" };
            TimeOffRequest timeOffRequest = new TimeOffRequest() { Id = Guid.Parse("16c5cea8-f0ff-42d2-bfe4-7dee294f720d") };
            UserService.Setup(u => u.GetCurrentUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            var model = new CreateTimeOffModel()
            {
                StartDate = new DateTime(2022, 01, 01),
                EndDate = new DateTime(2022, 01, 02),
                Reason = "fake - reason -",
                TimeOffType = "fake-type"
            };

            Mapper.Setup(map => map.Map<TimeOffRequest>(model)).Returns(timeOffRequest);

            TimeOffRequestService.Setup(ts => ts.CreateTimeOffAsync(timeOffRequest, "fake-type", user)).ReturnsAsync(timeOffRequest);

            await sut.Create(model);
  
            TimeOffRequestService.Verify(ts => ts.CreateTimeOffAsync(It.IsAny<TimeOffRequest>(), It.IsAny<string>(), user), Times.Once());
        }

        [Fact]
        public async Task Delete_Calls_Delete_From_Service()
        {
            var sut = new TimeOffRequestsController(TimeOffRequestService.Object, UserService.Object, Mapper.Object, LinkGenerator.Object);

            await sut.Delete(It.IsAny<string>());

            TimeOffRequestService.Verify(ts => ts.DeleteTimeOffAsync(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public async Task Edit_Calls_Edit_Async_From_Service()
        {
            TimeOffRequest timeOffRequest = new TimeOffRequest() { Id = Guid.Parse("16c5cea8-f0ff-42d2-bfe4-7dee294f720d") };
            EditTimeOffModel editTimeOffModel = new EditTimeOffModel();
            User user = new User() { UserName = "fake-user" };

            Mapper.Setup(m => m.Map<TimeOffRequest>(editTimeOffModel)).Returns(timeOffRequest);

            var sut = new TimeOffRequestsController(TimeOffRequestService.Object, UserService.Object, Mapper.Object, LinkGenerator.Object);
            UserService.Setup(u => u.GetCurrentUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

            await sut.Edit(editTimeOffModel, "16c5cea8-f0ff-42d2-bfe4-7dee294f720d");

            TimeOffRequestService.Verify(ts => ts.EditTimeOffAsync(timeOffRequest, user), Times.Once());
        }

        [Fact]
        public async Task Get_Calls_Find_By_Id()
        {

            TimeOffRequest timeOffRequest = new TimeOffRequest() { Id = Guid.Parse("16c5cea8-f0ff-42d2-bfe4-7dee294f720d") };

            var sut = new TimeOffRequestsController(TimeOffRequestService.Object, UserService.Object, Mapper.Object, LinkGenerator.Object);

            await sut.Get(timeOffRequest.Id.ToString());

            TimeOffRequestService.Verify(ts => ts.GetTimeOffByIdAsync(timeOffRequest.Id.ToString()), Times.Once());
        }

        [Fact]
        public async Task Get_All_Calls_Find_All()
        {
            var sut = new TimeOffRequestsController(TimeOffRequestService.Object, UserService.Object, Mapper.Object, LinkGenerator.Object);

            await sut.GetAll();

            TimeOffRequestService.Verify(ts => ts.GetAllTimeOffsAsync(), Times.Once());
        }

        [Fact]
        public async Task Get_All_Current_User_Calls_Repository()
        {
            var sut = new TimeOffRequestsController(TimeOffRequestService.Object, UserService.Object, Mapper.Object, LinkGenerator.Object);
            User user = new User() { UserName = "fake-user" };
            UserService.Setup(us => us.GetCurrentUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

            await sut.GetAllCurrentUser();

            TimeOffRequestService.Verify(ts => ts.GetTimeOffsByUserAsync(user), Times.Once());
        }
    }
}
