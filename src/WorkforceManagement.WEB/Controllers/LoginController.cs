using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using WorkforceManagement.BLL.IServices;

namespace WorkforceManagement.WEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly IUserService _userService;
        static HttpClient client = new();

        public LoginController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<string> Login(string usernameOrEmail, string password)
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

            HttpResponseMessage response = await client.PostAsync("https://localhost:5001/connect/token", content);

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
