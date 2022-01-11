using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkforceManagement.BLL.IServices;
using WorkforceManagement.BLL.Services;
using WorkforceManagement.DAL.Entities;

namespace WorkforceManagement.WEB.AuthorizationPolicies
{
    public class TimeOffRequestAdminOrCreatorHandler : AuthorizationHandler<TimeOffRequestAdminOrCreatorRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;
        private readonly IUserManager _userManager;
        private readonly ITimeOffRequestService _timeOffRequestService;

        public TimeOffRequestAdminOrCreatorHandler(IHttpContextAccessor httpContextAccessor, IUserService userService,
            IUserManager userManager, ITimeOffRequestService timeOffRequestService)
        {
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _userManager = userManager;
            _timeOffRequestService = timeOffRequestService;
        }

        protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context, TimeOffRequestAdminOrCreatorRequirement requirement)
        {
            string currentUserName = context.User.Identity.Name;

            User current = await _userService.GetUserByNameAsync(currentUserName);

            Guid timeOffRequestId = Guid.Parse(_httpContextAccessor.HttpContext.GetRouteValue("Id").ToString());

            var allTimeOffRequests = await _timeOffRequestService.GetAllTimeOffsAsync();

            //TimeOffRequest timeOffRequest = allTimeOffRequests.FirstOrDefault(t => t.)
        }
    }
}
