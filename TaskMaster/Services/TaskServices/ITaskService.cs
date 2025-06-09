using System.Threading.Tasks;
using TaskMaster.Dtos.TaskDTOs;

namespace TaskMaster.Services.TaskServices;

public interface ITaskService
{
    System.Threading.Tasks.Task<TaskDTO> CreateTask(CreateTaskDTO createTaskDto);
    System.Threading.Tasks.Task<TaskDTO> GetTaskById(int id);
    System.Threading.Tasks.Task<IEnumerable<TaskListDTO>> GetTasks(); 
    System.Threading.Tasks.Task<TaskDTO> UpdateTask(int id, UpdateTaskDTO updateTaskDto);
    System.Threading.Tasks.Task DeleteTask(int id);
}