using Microsoft.AspNetCore.Mvc;
using TaskMaster.Dtos.TaskDTOs;
using TaskMaster.Exceptions;
using TaskMaster.Services.TaskServices;

namespace TaskMaster.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost()]
        [ProducesResponseType(typeof(TaskDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostTask([FromBody] CreateTaskDTO request)
        {
            var result = await _taskService.CreateTask(request);
            return Ok(result);
        }

        [HttpGet()]
        [ProducesResponseType(typeof(IEnumerable<TaskDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTasks()
        {
            var result = await _taskService.GetTasks();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TaskDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTask(int id)
        {
            var result = await _taskService.GetTaskById(id);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(TaskDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskDTO request)
        {
            var result = await _taskService.UpdateTask(id, request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteTask(int id)
        {
            await _taskService.DeleteTask(id);
            return NoContent();
        }
    }
}
