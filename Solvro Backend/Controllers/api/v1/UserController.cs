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

        [HttpGet("user")]
        public IActionResult GetAllUsers()
        {
            var data = _userRepository.GetAllUsers().Select(u => new UserView(u));
            return ApiResponse.Ok(data);
        }

        [HttpPost("user")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
        {
            User user = new()
            {
                Specialization = dto.Specialization!.Value
            };

            user = await _userRepository.CreateUser(user);

            return ApiResponse.Created(new UserView(user));
        }
    }
}
