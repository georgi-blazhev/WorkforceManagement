using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Moq;
using WorkforceManagement.BLL.Services;
using WorkforceManagement.DAL.Entities;
using WorkforceManagment.BLL.UnitTests;
using Xunit;

namespace WorkforceManagement.BLL.UnitTests
{
    public class TimeOffRequestServiceTests : BaseServiceTest
    {
        [Fact]
        public async Task Create_TimeOffRequest()
        {
            User fakeApprover = new User() { UserName = "fake-approver" };
            List<User> fakeApprovers = new List<User> { fakeApprover };

            TimeOffRequest timeOffRequestStub = new TimeOffRequest()
            {
                StartDate = new DateTime(2021, 01, 01),
                EndDate = new DateTime(2021, 01, 12),
                Type = TimeOffRequestType.Paid,
                Status = Status.Created,
                Approvers = fakeApprovers
            };

            List<TimeOffRequest> timeOffRequests = new List<TimeOffRequest> { timeOffRequestStub };

            User creator = new User() { UserName = "fake" };
            List<DayOff> daysOff = new List<DayOff>() { new DayOff(new DateTime(2022, 03, 03)) };
            TimeOffRequestHelper.Setup(th => th.GetDaysOff(timeOffRequestStub)).ReturnsAsync(daysOff);
            TimeOffRequestHelper.Setup(th => th.FormatType(It.IsAny<string>())).Returns(TimeOffRequestType.Paid);
            var sut = new TimeOffRequestService(TimeOffRequestRepository.Object, EmailService.Object,
                TeamRepository.Object, TimeOffRequestHelper.Object);

            TimeOffRequestRepository.Setup(tr =>
            tr.FindAsync(It.IsAny<Expression<Func<TimeOffRequest, bool>>>()))
                .ReturnsAsync(timeOffRequests);

            await sut.CreateTimeOffAsync(timeOffRequestStub, "fake-type", creator);

            TimeOffRequestRepository.Verify(tr => tr.CreateTimeOffAsync(timeOffRequestStub), Times.Once());
        }

        [Fact]
        public async Task Delete_TimeOffRequest()
        {
            TimeOffRequest timeOffRequestStub = new TimeOffRequest()
            {
                StartDate = new DateTime(2021, 01, 01),
                EndDate = new DateTime(2021, 01, 12),
                Type = TimeOffRequestType.Paid,
                Status = Status.Created
            };

            TimeOffRequestRepository.Setup(tr => tr.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(timeOffRequestStub);

            var sut = new TimeOffRequestService(TimeOffRequestRepository.Object, EmailService.Object,
               TeamRepository.Object, TimeOffRequestHelper.Object);

            await sut.DeleteTimeOffAsync(It.IsAny<string>());

            TimeOffRequestRepository.Verify(tr => tr.DeleteAsync(It.IsAny<TimeOffRequest>()), Times.Once());
        }

        [Fact]
        public async Task Edit_TimeOffRequest()
        {
            TimeOffRequest timeOffRequestStub = new TimeOffRequest()
            {
                StartDate = new DateTime(2021, 01, 01),
                EndDate = new DateTime(2021, 01, 12),
                Type = TimeOffRequestType.Paid,
                Status = Status.Created
            };

            User current = new User() { UserName = "fake-name" };

            TimeOffRequestRepository.Setup(tr => tr.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(timeOffRequestStub);

            var sut = new TimeOffRequestService(TimeOffRequestRepository.Object, EmailService.Object,
               TeamRepository.Object, TimeOffRequestHelper.Object);

            await sut.EditTimeOffAsync(timeOffRequestStub, current);

            TimeOffRequestRepository.Verify(tr => tr.EditAsync(It.IsAny<TimeOffRequest>()), Times.Once());
        }

        [Fact]
        public async Task Get_TimeOff_By_Id_Returns_Single_Object()
        {
            TimeOffRequest timeOffRequestStub = new TimeOffRequest()
            {
                StartDate = new DateTime(2021, 01, 01),
                EndDate = new DateTime(2021, 01, 12),
                Type = TimeOffRequestType.Paid,
                Status = Status.Created
            };

            TimeOffRequestRepository.Setup(tr => tr.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(timeOffRequestStub);

            var sut = new TimeOffRequestService(TimeOffRequestRepository.Object, EmailService.Object,
               TeamRepository.Object, TimeOffRequestHelper.Object);

            var timeOff = await sut.GetTimeOffByIdAsync(It.IsAny<string>());

            Assert.Equal(timeOffRequestStub, timeOff);
        }

        [Fact]
        public async Task Get_All_Time_Off_Request_Returns_Whole_Collection()
        {
            List<TimeOffRequest> timeOffRequests = new List<TimeOffRequest>() { new TimeOffRequest(), new TimeOffRequest() };

            TimeOffRequestRepository.Setup(tr => tr.GetAllAsync()).ReturnsAsync(timeOffRequests);

            var sut = new TimeOffRequestService(TimeOffRequestRepository.Object, EmailService.Object,
               TeamRepository.Object, TimeOffRequestHelper.Object);

            var timeOffs = await sut.GetAllTimeOffsAsync();

            Assert.Equal(timeOffRequests, timeOffs);
        }

        [Fact]
        public async Task Get_All_Time_Off_By_User_Request_Returns_Current_User_Time_Off_Requests()
        {
            List<TimeOffRequest> timeOffRequests = new List<TimeOffRequest>() { new TimeOffRequest(), new TimeOffRequest() };
            User curent = new User() { UserName = "fake-name" };

            TimeOffRequestRepository.Setup(tr => tr.GetAllTimeOffsByUser(curent)).ReturnsAsync(timeOffRequests);

            var sut = new TimeOffRequestService(TimeOffRequestRepository.Object, EmailService.Object,
               TeamRepository.Object, TimeOffRequestHelper.Object);

            var timeOffs = await sut.GetTimeOffsByUserAsync(curent);

            Assert.Equal(timeOffRequests, timeOffs);
        }

        [Fact]
        public async Task Decide_Time_Off_Request_Registers_Decision()
        {
            TimeOffRequest timeOffRequestStub = new TimeOffRequest()
            {
                StartDate = new DateTime(2021, 01, 01),
                EndDate = new DateTime(2021, 01, 12),
                Type = TimeOffRequestType.Paid,
                Status = Status.Created
            };

            var requests = new List<TimeOffRequest>() { timeOffRequestStub };

            User approver = new User() { RequestsRequiringDecision = requests };
            TimeOffRequestRepository.Setup(tr => tr.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(timeOffRequestStub);

            var sut = new TimeOffRequestService(TimeOffRequestRepository.Object, EmailService.Object,
               TeamRepository.Object, TimeOffRequestHelper.Object);

            await sut.DecideTimeOffAsync(approver, It.IsAny<string>(), It.IsAny<string>());

            TimeOffRequestRepository.Verify(tr => tr.RegisterDecision(timeOffRequestStub, approver, It.IsAny<Decision>()), Times.Once());
        }

        [Fact]
        public async Task Get_Requests_Requiring_Decisions_Returns_Collection()
        {
            User teamLead = new User { UserName = "fake-team-lead" };
            Team team = new Team { Title = "fake-team", TeamLeader = teamLead };
            List<Team> allTeams = new List<Team> { team };
            TimeOffRequest timeOffRequest = new TimeOffRequest { Approvers = new List<User> { teamLead } };

            TeamRepository.Setup(tr => tr.GetAllAsync()).ReturnsAsync(allTeams);

            var sut = new TimeOffRequestService(TimeOffRequestRepository.Object, EmailService.Object,
               TeamRepository.Object, TimeOffRequestHelper.Object);
            List<TimeOffRequest> requestsRequiringDecision = new List<TimeOffRequest> { timeOffRequest };
            teamLead.RequestsRequiringDecision = requestsRequiringDecision;

            List<TimeOffRequest> result = await sut.RequireDecisionCurrentUserAsync(teamLead);

            Assert.Equal(requestsRequiringDecision, result);
        }
    }
}
