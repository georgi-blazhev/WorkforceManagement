﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using WorkforceManagement.BLL.IServices;
using WorkforceManagement.BLL.Services;
using WorkforceManagement.DAL.Entities;

namespace WorkforceManagement.WEB.AuthorizationPolicies
{
    [ExcludeFromCodeCoverage]
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

            Guid timeOffRequestId = Guid.Parse(_httpContextAccessor.HttpContext.GetRouteValue("timeOffId").ToString());

            var allTimeOffRequests = await _timeOffRequestService.GetAllTimeOffsAsync();

            TimeOffRequest timeOffRequest = allTimeOffRequests.FirstOrDefault(t => t.Id == timeOffRequestId);

            if (timeOffRequest == null)
            {
                context.Fail();
                await Task.CompletedTask;
                return;
            }

            if (timeOffRequest.CreatorId == current.Id)
            {
                context.Succeed(requirement);
            }

            if (_userManager.GetUserRolesAsync(current).Result.Contains("Admin"))
            {
                context.Succeed(requirement);
            }

            await Task.CompletedTask;
            return;
        }
    }
}
