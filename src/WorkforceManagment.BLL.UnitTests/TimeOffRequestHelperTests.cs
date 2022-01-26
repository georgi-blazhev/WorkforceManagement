using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using WorkforceManagement.DAL.Entities;
using WorkforceManagment.BLL.UnitTests;
using WorkforceManagement.BLL.Helpers;
using Xunit;

namespace WorkforceManagement.BLL.UnitTests
{
    public class TimeOffRequestHelperTests : BaseServiceTest
    {
        [Fact]
        public void Format_Type_Returns_Paid_Type()
        {
            string type = "paid";

            var sut = new TimeOffRequestHelper(TimeOffRequestRepository.Object);

            TimeOffRequestType requestType = TimeOffRequestType.Paid;

            TimeOffRequestType returnType = sut.FormatType(type);

            Assert.Equal(requestType, requestType);
        }

        [Fact]
        public void Format_Type_Returns_Unpaid_Type()
        {
            string type = "unpaid";

            var sut = new TimeOffRequestHelper(TimeOffRequestRepository.Object);

            TimeOffRequestType requestType = TimeOffRequestType.Paid;

            TimeOffRequestType returnType = sut.FormatType(type);

            Assert.Equal(requestType, requestType);
        }

        [Fact]
        public void Format_Type_Returns_SickLeave_Type()
        {
            string type = "sickleave";

            var sut = new TimeOffRequestHelper(TimeOffRequestRepository.Object);

            TimeOffRequestType requestType = TimeOffRequestType.Paid;

            TimeOffRequestType returnType = sut.FormatType(type);

            Assert.Equal(requestType, requestType);
        }

        [Fact]
        public void Format_Invalid_Type_Returns_Throws_Exception()
        {
            string type = "fake type";

            var sut = new TimeOffRequestHelper(TimeOffRequestRepository.Object);

            Assert.Throws<ArgumentException>(() => sut.FormatType(type));
        }

        [Fact]
        public void Format_Decision_Returns_Approve_Type()
        {
            string decision = "approve";

            Decision approved = Decision.Approve;
            var sut = new TimeOffRequestHelper(TimeOffRequestRepository.Object);

            Decision returnedDecision = sut.FormatDecision(decision);

            Assert.Equal(approved, returnedDecision);
        }

        [Fact]
        public void Format_Decision_Returns_Reject_Type()
        {
            string decision = "reject";

            Decision rejected = Decision.Reject;
            var sut = new TimeOffRequestHelper(TimeOffRequestRepository.Object);

            Decision returnedDecision = sut.FormatDecision(decision);

            Assert.Equal(rejected, returnedDecision);
        }

        [Fact]
        public void Format_Invalid_Decision_Throws_Exception()
        {
            string decision = "fake decision";

            var sut = new TimeOffRequestHelper(TimeOffRequestRepository.Object);

            Assert.Throws<ArgumentException>(() => sut.FormatDecision(decision));
        }

        [Fact]
        public async Task Get_Days_Off_Returns_Collection_Without_Holidays_And_Weekends()
        {
            TimeOffRequest timeOffRequest = new TimeOffRequest()
            {
                StartDate = new DateTime(2022, 03, 02),
                EndDate = new DateTime(2022, 03, 04),
                DaysOff = new List<DayOff>() { new DayOff(new DateTime(2022, 03, 02)), new DayOff(new DateTime(2022, 03, 04)) }
            };

            List<Holiday> holidays = new List<Holiday>() { new Holiday(new DateTime(2022, 03, 03)) };
            TimeOffRequestRepository.Setup(tr => tr.GetAllOfficialHolidays()).ReturnsAsync(holidays);

            var sut = new TimeOffRequestHelper(TimeOffRequestRepository.Object);

            await sut.GetDaysOff(timeOffRequest);

            TimeOffRequestRepository.Verify(tr => tr.GetAllOfficialHolidays(), Times.Once());
        }
    }
}
