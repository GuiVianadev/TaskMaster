using TaskMaster.Dtos.TaskDTOs;

namespace TaskMaster.Mappers;

public static class TaskMapper
{
    public static TaskDTO ToTaskDto(Entities.Task task)
    {
        return new TaskDTO(task.Id, task.Title, task.Status, task.Description, task.UserId, task.Priority);
    }
    
    public static TaskListDTO ToTaskListDto(Entities.Task task)
    {
        return new TaskListDTO(task.Id, task.Title, task.Status, task.Priority);
    }
}