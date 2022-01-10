using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkforceManagement.DAL.Entities;
using WorkforceManagement.BLL.IServices;
using WorkforceManagement.Models.DTO.Requests.TeamRequests;
using WorkforceManagement.Models.DTO.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkforceManagment.Models.DTO.Responses;

namespace WorkforceManagement.WEB.Controllers
{
    [Route("api/timeoffrequests")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class TimeOffRequestsController : ControllerBase
    {
        public TimeOffRequestsController() : base()
        {

        }
    }
}