using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using WorkforceManagement.BLL.IServices;
using Microsoft.AspNetCore.Authorization;

namespace WorkforceManagement.WEB.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class GetTokenController : Controller
    {
        private readonly IUserService _userService;
        private readonly HttpClient _client;

        public GetTokenController(IUserService userService, HttpClient client)
        {
            _userService = userService;
            _client = client;
        }

        [HttpPost]
        public async Task<string> GetToken(string usernameOrEmail, string password)
        {
            var username = await GetUsernameAsync(usernameOrEmail);

            var values = new Dictionary<string, string>
            {
                { "username", username},
                { "password", password},
                { "grant_type", "password" },
                { "client_id", "workforce" },
                { "client_secret", "secret" }
            };

            var content = new FormUrlEncodedContent(values);

            HttpResponseMessage response = await _client.PostAsync("https://workforcemanagementapi.azurewebsites.net/connect/token", content);

            return await response.Content.ReadAsStringAsync();
        }

        private async Task<string> GetUsernameAsync(string usernameOrEmail)
        {
            var user = await _userService.GetUserByEmailAsync(usernameOrEmail);
            if (user != null) return user.UserName;
            return usernameOrEmail;            
        }
    }        
}
