using TaskMaster.Entities.Enum;

namespace TaskMaster.Dtos.TaskDTOs;

public record TaskListDTO(
        int Id,
        string Title,
        Status status,
        Priority priority
    );