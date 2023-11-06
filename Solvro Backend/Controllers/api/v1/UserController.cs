using Microsoft.AspNetCore.Mvc;
using Solvro_Backend.DTOs;
using Solvro_Backend.Models;
using Solvro_Backend.Models.Database;
using Solvro_Backend.Models.Views;
using Solvro_Backend.Repositories;

namespace Solvro_Backend.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserRepository _userRepository;

        public UserController(IServiceProvider provider)
        {
            _userRepository = provider.GetRequiredService<IUserRepository>();
        }

        /// <summary>
        /// Gets all users in the system
        /// </summary>
        /// <response code="200">Ok</response>
        [HttpGet("user")]
        [ProducesResponseType(typeof(ApiResponse<UserView[]>), 200)]
        public IActionResult GetAllUsers()
        {
            var data = _userRepository.GetAllUsers().Select(u => new UserView(u));
            return ApiResponse<IEnumerable<UserView>>.Ok(data);
        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <response code="201">Created</response>
        [HttpPost("user")]
        [ProducesResponseType(typeof(ApiResponse<UserView>), 201)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
        {
            User user = new()
            {
                Specialization = dto.Specialization!.Value
            };

            user = await _userRepository.CreateUser(user);

            return ApiResponse<UserView>.Created(new UserView(user));
        }
    }
}
