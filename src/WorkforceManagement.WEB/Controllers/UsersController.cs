using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkforceManagement.BLL.IServices;
using WorkforceManagement.DAL.Entities;
using WorkforceManagement.Models.User;

namespace WorkforceManagement.WEB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;


        public UsersController(IUserService userService, IMapper mapper, LinkGenerator linkGenerator)
        {
            _userService = userService;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<ActionResult<ViewUserModel>> Get(string userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            return _mapper.Map<ViewUserModel>(user);
        }

        [HttpGet]
        [Route("All")]
        public async Task<ViewUserModel[]> AllUsers()
        {
            List<User> allUsers = await _userService.GetAllUsersAsync();
            return _mapper.Map<ViewUserModel[]>(allUsers);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateUserModel model)
        {
            User newUser = _mapper.Map<User>(model);
            newUser = await _userService.CreateUserAsync(newUser, model.Password, model.Role);

            string location = GenerateLocation(newUser);
            if (string.IsNullOrWhiteSpace(location)) return BadRequest("Error at URI creation! ");

            return Created(location, _mapper.Map<ViewUserModel>(newUser));
        }

        [HttpPut("{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(EditUserModel model, string userId)
        {
            var userWithUpdates = _mapper.Map<User>(model);
            userWithUpdates.Id = userId;
            await _userService.EditUserAsync(userWithUpdates, model.CurrentPassword, model.NewPassword);

            return Ok("User was successfully edited! ");
        }

        [HttpDelete("{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(string userId)
        {
            await _userService.DeleteUserAsync(userId);
            return Ok("User was successfully deleted! ");
        }

        private string GenerateLocation(User newUser)
        {
            return _linkGenerator.GetPathByAction("Get",
                        "Users",
                        new { userId = newUser.Id });
        }
    }
}
