using System;
using System.Linq;
using Domain.Models;
using RestApi.Models;
using Domain.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet("GetUsers")]
        public async Task<ActionResult<IEnumerable<IUser>>> GetUsers()
        {
            ActionResult<IEnumerable<IUser>> result;
            var users = await _userService.GetUsersAsync();
            if (users is not null && users.Any())
            {
                result = Ok(users);
            }
            else
            {
                result = NoContent();
            }

            return result;
        }

        [HttpGet("GetUserById")]
        public async Task<ActionResult<IUser>> GetById([FromQuery] Guid id)
        {
            ActionResult<IUser> result;
            var user = await _userService.GetUserByIdAsync(id);

            if (user is not null)
            {
                result = Ok(user);
            }
            else
            {
                result = NotFound();
            }

            return result;
        }

        [HttpPost("AddUser")]
        public async Task<ActionResult<string>> AddUser([FromBody] UserCreationRequest userCreationRequest)
        {
            ActionResult<string> result;
            if (userCreationRequest == null || string.IsNullOrWhiteSpace(userCreationRequest.Email)
                || string.IsNullOrWhiteSpace(userCreationRequest.Name)
                || string.IsNullOrWhiteSpace(userCreationRequest.Surname)
                || string.IsNullOrWhiteSpace(userCreationRequest.PhoneNumber))
            {
                _logger.LogError("User creation request model must not be a null");
                result = BadRequest();
            }

            var newUser = await _userService.CreateUserAsync(userCreationRequest.Email,
                userCreationRequest.Name,
                userCreationRequest.Surname,
                userCreationRequest.PhoneNumber);

            result = Ok(newUser);

            return result;
        }

        [HttpPut("UpdateUser")]
        public async Task<ActionResult<IUser>> UpdateUser([FromQuery] Guid id, [FromBody] UserUpdateRequest userUpdateRequest)
        {
            ActionResult<IUser> result;
            var user = await _userService.GetUserByIdAsync(id);
            if (user is null)
            {
                _logger.LogInformation($"User with {id} does not found");
                result = NotFound($"User with {id} not found");
            }

            await _userService.UpdateUserAsync(id, userUpdateRequest.Email, userUpdateRequest.Name, userUpdateRequest.Surname, userUpdateRequest.PhoneNumber);
            result = Ok(userUpdateRequest);

            return result;
        }

        [HttpDelete("DeleteUserById")]
        public async Task<ActionResult> DeleteUser([FromQuery] Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user is null)
            {
                _logger.LogInformation($"User with {id} does not found");
                return NotFound($"User with {id} not found");
            }

            await _userService.DeleteUserAsync(id);
            return Ok();
        }
    }
}
