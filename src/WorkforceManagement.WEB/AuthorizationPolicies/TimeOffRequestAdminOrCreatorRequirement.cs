using Microsoft.AspNetCore.Authorization;
using System.Diagnostics.CodeAnalysis;

namespace WorkforceManagement.WEB.AuthorizationPolicies
{
    [ExcludeFromCodeCoverage]
    public class TimeOffRequestAdminOrCreatorRequirement : IAuthorizationRequirement
    {
        public TimeOffRequestAdminOrCreatorRequirement()
        {

        }
    }
}
