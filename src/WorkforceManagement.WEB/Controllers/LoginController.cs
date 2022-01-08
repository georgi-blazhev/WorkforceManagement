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

namespace WorkforceManagement.WEB.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : Controller
    {
        private IConfiguration _config;
        private readonly IUserService _userService;

        public LoginController(IConfiguration config, IUserService userService)
        {
            _config = config;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginUserModel login)
        {
            IActionResult response = Unauthorized();
            var user = await AuthenticateUser(login);

            if (user != null)
            {
                var tokenString = GenerateJSONWebToken(user);
                response = Ok(new { token = tokenString });
            }

            return response;
        }

        private string GenerateJSONWebToken(LoginUserModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              null,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<LoginUserModel> AuthenticateUser(LoginUserModel model)
        {
            LoginUserModel modelUser = new();
            User user = await _userService.GetUserByUsernameAndPassword(model.Username, model.Password);
            if (user == null) return null;
            modelUser.Username = model.Username;
            modelUser.Password = model.Password;
            return modelUser;
        }
    }
}
