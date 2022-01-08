using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkforceManagement.BLL.IServices;
using WorkforceManagement.DAL.Entities;
using WorkforceManagment.Models.DTO.Requests.UserRequests;
using WorkforceManagment.Models.DTO.Responses;

namespace WorkforceManagement.WEB.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpGet]
        public async Task<List<UserResponse>> AllUsers()
        {
            List<User> users = await _userService.GetAllUsersAsync();
            List<UserResponse> result = new();
            foreach (var user in users)
            {
                result.Add(new UserResponse()
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                });
            }
            return result;
        }

        // TODO: Add log-in controller

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("/create")]
        public async Task Register(CreateUserModel model)
        {
            await _userService.CreateUserAsync(model.UserName, model.Email, model.Password, model.FirstName, model.LastName, model.Role);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("/edit/{userId}")]
        public async Task Edit(EditUserModel model, string userId)
        {
            await _userService.EditUserAsync(userId, model.NewUserName, model.Email, model.CurrentPassword, model.NewPassword, model.FirstName, model.LastName);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("/delete/{userId}")]
        public async Task Delete(string userId)
        {
            await _userService.DeleteUserAsync(userId);
        }
    }

}
