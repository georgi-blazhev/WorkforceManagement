using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using WorkforceManagement.DAL.Entities;
using WorkforceManagement.DAL.IRepositories;
using WorkforceManagement.DAL.Repositories;

namespace WorkforceManagement.BLL.Services
{
    [ExcludeFromCodeCoverage]
    public class EmailService : IEmailService
    {
        private readonly Settings _settings;
        private readonly ITimeOffRequestRepository _timeOffRequestRepository;
        private readonly ITeamRepository _teamRepository;

        public EmailService(IOptions<Settings> Settings, ITimeOffRequestRepository timeOffRequestRepository,
            ITeamRepository teamRepository)
        {
            _settings = Settings.Value;
            _timeOffRequestRepository = timeOffRequestRepository;
            _teamRepository = teamRepository;
        }

        public async Task SendAsync(string to, string subject, string message)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_settings.From));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart("plain") { Text = message };


            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_settings.Provider, _settings.Port, true);
            await smtp.AuthenticateAsync(_settings.From, _settings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
            smtp.Dispose();
        }
        public async Task SendEmailNotificationsForCreation(TimeOffRequest timeOff, User creator)
        {
            if (timeOff.Type == TimeOffRequestType.SickLeave)
            {
                await SendEmailNotificationsForSickLeave(timeOff);
                return;
            }

            foreach (var leader in timeOff.Approvers)
                await TemplateEmailForCreationTL(leader, timeOff);

            await SendAsync(creator.Email,
                "New time off request",
                "You have created a new time off request \n" +
                 $"starting from: {timeOff.StartDate.ToString("dd/MM/yyyy")} \n" +
                 $"ending on {timeOff.EndDate.ToString("dd/MM/yyyy")} \n" +
                 $"type: {timeOff.Type.ToString()}\n" +
                 $"for a total of {timeOff.DaysOff.Count()} days off work \n");

        }
        public async Task SendEmailNotificationsForDecision(string timeOffId)
        {
            var currentRequest = await _timeOffRequestRepository.FindByIdAsync(timeOffId);

            if (currentRequest.Status == Status.Rejected || currentRequest.Status == Status.Approved)
            {
                foreach (var leader in currentRequest.Approvers)
                    await TemplateEmailForTLDecision(leader, currentRequest);
                await TemplateEmailForCreatorDecision(currentRequest);
            }
        }
        public async Task SendEmailNotificationsForSickLeave(TimeOffRequest timeOff)
        {
            var allTeams = await _teamRepository.GetAllAsync();
            foreach (var team in allTeams)
            {
                if (!timeOff.Approvers.Contains(team.TeamLeader))
                    continue;
                foreach (var teamMember in team.Members)
                {
                    if (teamMember.Id != timeOff.Creator.Id)
                    {
                        await TemplateEmailForSickLeave(teamMember, timeOff);
                    }
                }
            }

            await SendAsync(timeOff.Creator.Email,
                    "You have requested a sick leave",
                    "Your new sick leave has been submitted \n" +
                    $"starting from: {timeOff.StartDate.ToString("dd/MM/yyyy")} \n" +
                    $"ending on {timeOff.EndDate.ToString("dd/MM/yyyy")} \n" +
                    $"type: {timeOff.Type.ToString()}\n" +
                    $"for a total of {timeOff.DaysOff.Count()} days off work \n");
        }
        public async Task TemplateEmailForTLDecision(User leader, TimeOffRequest timeOff)
        {
            await SendAsync(leader.Email,
                     "Time off request update",
                     $"{timeOff.Creator.FirstName} {timeOff.Creator.LastName} had a time off request \n" +
                     $"starting from: {timeOff.StartDate.ToString("dd/MM/yyyy")} \n" +
                     $"ending on {timeOff.EndDate.ToString("dd/MM/yyyy")} \n" +
                     $"for a total of {timeOff.DaysOff.Count()} days off work \n" +
                     $"type: {timeOff.Type.ToString()}\n" +
                     $"which has been {timeOff.Status.ToString().ToLower()}");
        }
        public async Task TemplateEmailForCreatorDecision(TimeOffRequest timeOff)
        {
            await SendAsync(timeOff.Creator.Email,
                    "Time off request update",
                    "Your time off request \n" +
                    $"starting from: {timeOff.StartDate.ToString("dd/MM/yyyy")} \n" +
                    $"ending on {timeOff.EndDate.ToString("dd/MM/yyyy")} \n" +
                    $"for a total of {timeOff.DaysOff.Count()} days off work \n" +
                    $"type: {timeOff.Type.ToString()}\n" +
                    $"has been {timeOff.Status.ToString().ToLower()}");
        }
        public async Task TemplateEmailForSickLeave(User teamMember, TimeOffRequest timeOff)
        {
            await SendAsync(teamMember.Email,
                    "New sick leave request",
                     $"{timeOff.Creator.FirstName} {timeOff.Creator.LastName} from your team has created a sick leave request \n" +
                     $"starting from: {timeOff.StartDate.ToString("dd/MM/yyyy")} \n" +
                     $"ending on {timeOff.EndDate.ToString("dd/MM/yyyy")} \n" +
                     $"type: {timeOff.Type.ToString()}\n" +
                     $"for a total of {timeOff.DaysOff.Count()} days off work \n");

        }
        public async Task TemplateEmailForCreationTL(User leader, TimeOffRequest timeOff)
        {
            await SendAsync(leader.Email,
                    "New time off request",
                    $"{timeOff.Creator.FirstName} {timeOff.Creator.LastName} has created a new time off request \n" +
                    $"starting from: {timeOff.StartDate.ToString("dd/MM/yyyy")} \n" +
                    $"ending on {timeOff.EndDate.ToString("dd/MM/yyyy")} \n" +
                    $"type: {timeOff.Type.ToString()}\n" +
                    $"for a total of {timeOff.DaysOff.Count()} days off work \n");
        }
    }
}

public class Settings
{
    public string Provider { get; set; }
    public int Port { get; set; }
    public string From { get; set; }
    public string Password { get; set; }
}
