using System.Threading.Tasks;
using WorkforceManagement.DAL.Entities;

namespace WorkforceManagement.BLL.Services
{
    public interface IEmailService
    {
        Task SendAsync(string to, string subject, string message);
        Task SendEmailNotificationsForCreation(TimeOffRequest timeOff, User creator);
        Task SendEmailNotificationsForDecision(string timeOffId);
        Task SendEmailNotificationsForSickLeave(TimeOffRequest timeOff);
        Task TemplateEmailForTLDecision(User leader, TimeOffRequest timeOff);
        Task TemplateEmailForCreatorDecision(TimeOffRequest timeOff);
        Task TemplateEmailForSickLeave(User teamMember, TimeOffRequest timeOff);
        Task TemplateEmailForCreationTL(User leader, TimeOffRequest timeOff);
    }
}