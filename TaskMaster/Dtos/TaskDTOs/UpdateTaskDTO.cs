using TaskMaster.Entities.Enum;

namespace TaskMaster.Dtos.TaskDTOs;

public record UpdateTaskDTO(
        string Title,
        string Description, 
        Status Status,
        Priority Priority 
    );