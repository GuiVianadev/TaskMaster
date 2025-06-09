namespace TaskMaster.Dtos.TaskDTOs;
using TaskMaster.Entities.Enum;

public record TaskDTO(
    int Id,
    string Title,
    Status Status,
    string Description,
    int UserId,
    Priority Priority
    );