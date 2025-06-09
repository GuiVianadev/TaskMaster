using Microsoft.EntityFrameworkCore;
using TaskMaster.Data;
using TaskMaster.Dtos.TaskDTOs;
using TaskMaster.Exceptions;
using TaskMaster.Mappers;
using TaskMaster.Validator.TaskValidator;

namespace TaskMaster.Services.TaskServices;

public class TaskService : ITaskService
{
    private readonly AppDbContext  _context;

    public TaskService(AppDbContext  context)
    {
        _context = context;
    }


    public async Task<TaskDTO> CreateTask(CreateTaskDTO request)
    {
        var validator = new CreateTaskValidator();
        var result =  validator.Validate(request);
        
        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
               throw new ErrorOnValidationException(errorMessages);
        }
        
        var taskExist = await _context.Tasks.AnyAsync(t => t.Title == request.Title);
        if (taskExist)
        {
            throw new ArgumentException("Task with the same Title already exists");
        }
        
        var userExists = await _context.Users.AnyAsync(u => u.Id == request.UserId);
        if (!userExists)
        {
            throw new ArgumentException("User with this id doesn't exist");
        }
        
        var task = new Entities.Task(
            request.Title,
            request.Description,
            request.Priority,
            request.UserId
            );
        
        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync();

        return TaskMapper.ToTaskDto(task);
    }

    public async Task<TaskDTO> GetTaskById(int id)
    {
        var task = await GetTaskOrThrow(id);
        return TaskMapper.ToTaskDto(task);
    }

    public async Task<IEnumerable<TaskListDTO>> GetTasks()
    {
        var tasks = await _context.Tasks.ToListAsync();
        return tasks.Select(TaskMapper.ToTaskListDto);
    }

    public async Task<TaskDTO> UpdateTask(int id, UpdateTaskDTO request)
    {
        var task = await GetTaskOrThrow(id);
        var validator = new UpdateTaskValidator();
        
        var result = validator.Validate(request);
        
        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
        
        task.UpdateTitle(request.Title);
        task.UpdateDescription(request.Description);
        task.UpdatePriority(request.Priority);
        task.UpdateStatus(request.Status);
        
        await _context.SaveChangesAsync();
        
        return TaskMapper.ToTaskDto(task);
    }

    public async Task DeleteTask(int id)
    {
        var task = await GetTaskOrThrow(id);
         _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
    }

    public async Task<Entities.Task> GetTaskOrThrow(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
        {
            throw new KeyNotFoundException($"Task with id {id} not found");
        }
        return task;
    }
}