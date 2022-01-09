using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;

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
