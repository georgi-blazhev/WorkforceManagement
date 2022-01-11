using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkforceManagement.WEB.AuthorizationPolicies
{
    public class TimeOffRequestAdminOrCreatorRequirement : IAuthorizationRequirement
    {
        public TimeOffRequestAdminOrCreatorRequirement()
        {

        }
    }
}
