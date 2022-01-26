using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkforceManagement.BLL.IHelpers;
using WorkforceManagement.DAL.Entities;
using WorkforceManagement.DAL.IRepositories;
using WorkforceManagement.DAL.Repositories;

namespace WorkforceManagement.BLL.Services
{
    public class TimeOffRequestService : ITimeOffRequestService
    {
        private readonly ITimeOffRequestRepository _timeOffRequestRepository;
        private readonly IEmailService _emailService;
        private readonly ITeamRepository _teamRepository;
        private readonly ITimeOffRequestHelper _helper;

        public TimeOffRequestService(ITimeOffRequestRepository timeOffRequestRepository, IEmailService emailService,
            ITeamRepository teamRepository, ITimeOffRequestHelper timeOffRequestHelper)
        {
            _timeOffRequestRepository = timeOffRequestRepository;
            _emailService = emailService;
            _teamRepository = teamRepository;
            _helper = timeOffRequestHelper;
        }

        public async Task<TimeOffRequest> GetTimeOffByIdAsync(string timeOffId)
        {
            return await _timeOffRequestRepository.FindByIdAsync(timeOffId);
        }
        public async Task<IEnumerable<TimeOffRequest>> GetAllTimeOffsAsync()
        {
            return await _timeOffRequestRepository.GetAllAsync();
        }
        public async Task<IEnumerable<TimeOffRequest>> GetTimeOffsByUserAsync(User user)
        {
            return await _timeOffRequestRepository.GetAllTimeOffsByUser(user);
        }
        public async Task<TimeOffRequest> CreateTimeOffAsync(TimeOffRequest newTimeOffRequest, string type, User currentUser)
        {
            if (newTimeOffRequest.StartDate > newTimeOffRequest.EndDate)
                throw new ArgumentException("Start date can not be later than End date");

            await _helper.CheckOverlapping(newTimeOffRequest, currentUser);

            var now = DateTime.Now;
            newTimeOffRequest.Type = _helper.FormatType(type);
            newTimeOffRequest.Status = Status.Created;
            newTimeOffRequest.CreatedAt = now;
            newTimeOffRequest.LastChange = now;
            newTimeOffRequest.Creator = currentUser;
            newTimeOffRequest.DaysOff = await _helper.GetDaysOff(newTimeOffRequest);

            await _timeOffRequestRepository.CreateTimeOffAsync(newTimeOffRequest);
            var justCreatedRequest = _timeOffRequestRepository.FindAsync(t => t.CreatedAt == now).Result.FirstOrDefault();
            if (justCreatedRequest.Approvers != null)
            {
                await _emailService.SendEmailNotificationsForCreation(justCreatedRequest, currentUser);
            }
            return justCreatedRequest;
        }
        public async Task EditTimeOffAsync(TimeOffRequest timeOffRequest, User creator)
        {
            if (timeOffRequest.StartDate > timeOffRequest.EndDate)
                throw new ArgumentException("Start date can not be later than End date");

            await _helper.CheckOverlapping(timeOffRequest, creator);

            var originalTimeOff = await _timeOffRequestRepository.FindByIdAsync(timeOffRequest.Id.ToString());

            originalTimeOff.StartDate = timeOffRequest.StartDate;
            originalTimeOff.EndDate = timeOffRequest.EndDate;
            originalTimeOff.Reason = timeOffRequest.Reason;
            originalTimeOff.LastChange = DateTime.Now;

            await _timeOffRequestRepository.EditAsync(originalTimeOff);
        }
        public async Task DeleteTimeOffAsync(string timeOffId)
        {
            var timeOff = await _timeOffRequestRepository.FindByIdAsync(timeOffId);
            await _timeOffRequestRepository.DeleteAsync(timeOff);
        }
        public async Task DecideTimeOffAsync(User currentUser, string timeOffId, string decision)
        {
            var timeOff = await _timeOffRequestRepository.FindByIdAsync(timeOffId);
      
            if (timeOff.Status == Status.Approved || timeOff.Status == Status.Rejected)
                throw new ArgumentException($"This Time Off Request has already been {timeOff.Status}! ");

            if (!currentUser.RequestsRequiringDecision.Contains(timeOff))
                throw new UnauthorizedAccessException("User does not have right to approve this request");

            var decisionEntry = _helper.FormatDecision(decision);
            await _timeOffRequestRepository.RegisterDecision(timeOff, currentUser, decisionEntry);
            await _timeOffRequestRepository.RegisterRequestStatusChangeIfNeeded(timeOff, currentUser);

            if (timeOff.Status == Status.Approved || timeOff.Status == Status.Rejected)
                await _emailService.SendEmailNotificationsForDecision(timeOffId);
        }  
        public async Task<List<TimeOffRequest>> RequireDecisionCurrentUserAsync(User currentUser)
        {
            var allTeams = await _teamRepository.GetAllAsync();
            bool isTeamLeader = allTeams.Any(t => t.TeamLeader == currentUser);

            if (!isTeamLeader)
                throw new UnauthorizedAccessException("Current user is not a team leader");

            return currentUser.RequestsRequiringDecision.Where(r => r.Type != TimeOffRequestType.SickLeave).ToList();
        }
    }
}
