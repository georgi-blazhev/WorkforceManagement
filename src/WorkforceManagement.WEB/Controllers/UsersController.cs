using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkforceManagement.BLL.IServices;
using WorkforceManagement.DAL.Entities;
using WorkforceManagment.Models.DTO.Requests.UserRequests;
using WorkforceManagment.Models.DTO.Responses;

namespace WorkforceManagement.WEB.Controllers
{
    [Route("api/users")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("All")]
        public async Task<List<UserResponseModel>> AllUsers()
        {
            List<User> users = await _userService.GetAllUsersAsync();
            List<UserResponseModel> result = new();
            foreach (var user in users)
            {
                result.Add(new UserResponseModel()
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                });
            }
            return result;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Register(CreateUserModel model)
        {
            bool userWasCreated = await _userService.CreateUserAsync(model.UserName, model.Email, model.Password, model.FirstName, model.LastName, model.Role);
            if (userWasCreated) return Ok("User was successfully registered! ");
            return BadRequest("A User with such Email or Username already exsists! ");
        }

        [HttpPut("{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(EditUserModel model, string userId)
        {
            bool userWasEdited = await _userService.EditUserAsync(userId, model.NewUserName, model.Email, model.CurrentPassword, model.NewPassword, model.FirstName, model.LastName);
            if (userWasEdited) return Ok("User was successfully edited! ");
            return BadRequest("A User with such Email or Username already exsists! ");
        }

        [HttpDelete("{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task Delete(string userId)
        {
            await _userService.DeleteUserAsync(userId);
        }
    }

}
