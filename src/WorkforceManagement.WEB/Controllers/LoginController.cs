using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using WorkforceManagement.BLL.IServices;
using WorkforceManagement.DAL.Entities;
using WorkforceManagment.Models.DTO.Requests.UserRequests;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using WorkforceManagment.Models.DTO.Responses;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;
using System.Net.Http.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace WorkforceManagement.WEB.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : Controller
    {
        static HttpClient client = new();

        [HttpPost]
        public async Task<string> Login(string username, string password)
        {
            var values = new Dictionary<string, string>
            {
                { "username", username},
                { "password", password},
                { "grant_type", "password" },
                { "client_id", "workforce" },
                { "client_secret", "secret" }
            };

            var content = new FormUrlEncodedContent(values);

            HttpResponseMessage response = await client.PostAsync("https://localhost:5001/connect/token", content);

            return await response.Content.ReadAsStringAsync();
        }
    }
}
