using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskMaster.Dtos.TaskDTOs;
using TaskMaster.Dtos.UserDTOs;
using TaskMaster.Exceptions;
using TaskMaster.Services.UserServices;

namespace TaskMaster.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        
        
        [HttpPost]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostUser  ([FromBody] CreateUserDTO request)
        {
            var result = await _userService.CreateUser(request);
            return Created(string.Empty, result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsers();
            return Ok(users);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUser(int id)
        {
            var result = await _userService.GetUserById(id);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutUser(int id, [FromBody] UpdateUserDTO request)
        {
            var result = await _userService.UpdateUser(id, request);
            return Ok(result);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUser(id);
            return NoContent();
        }
        
    }
}
